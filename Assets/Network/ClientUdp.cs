using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Utils;

namespace Network
{
    public class ClientUdp
    {
        private List<IPEndPoint> m_players_endpoint;
        private UdpClient m_client;
        private Mutex m_endpoint_mutex;
        private Dictionary<string, string> m_endpoint_to_data;
        private Dictionary<string, bool> m_endpoint_to_data_pending;
        private Thread m_receive_thread;
        private ValueWrapper<bool> m_is_running;
        private Client m_tcp_client;
        private IPEndPoint m_endpoint;

        public int Id
        {
            get;
            private set;
        }

        public ClientUdp(ValueWrapper<bool> isRunning, int portUDP, Client tcpClient, string _ip, int portTCP)
        {
            m_is_running = isRunning;
            m_endpoint = new IPEndPoint(IPAddress.Any, portUDP);
            
            m_client = new UdpClient(m_endpoint);

            m_players_endpoint = new List<IPEndPoint>();
            m_endpoint_mutex = new Mutex();
            m_endpoint_to_data = new Dictionary<string, string>();
            m_endpoint_to_data_pending = new Dictionary<string, bool>();
            m_tcp_client = tcpClient;
            
            tcpClient.addNetworkMessageHandler("CC", data =>
            {
                m_endpoint_mutex.WaitOne();
                string[] ip_ports = data.Split('\n');
                if (ip_ports.Length < 3)
                {
                    Debug.LogError("On a pas 3 clients, c'est pas normal");
                    return;
                }
                
                for (int i = 0; i < ip_ports.Length; i++)
                {
                    string ip = ip_ports[i].Split(':')[0];
                    int port= Convert.ToInt32(ip_ports[i].Split(':')[1]);
                    m_endpoint_to_data[ip_ports[i]] = "";
                    m_endpoint_to_data_pending[ip_ports[i]] = false;
                    m_players_endpoint.Add(new IPEndPoint(IPAddress.Parse(ip),port));
                }
                
                m_receive_thread.Start();
                m_endpoint_mutex.ReleaseMutex();
            } );
            
            tcpClient.addNetworkMessageHandler("GIP", data =>
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                Id = Convert.ToInt32(data);
                m_client.Send(buffer, buffer.Length, new IPEndPoint(IPAddress.Parse(_ip),portTCP));
            });
            m_receive_thread = new Thread(receive);
            
        }

        public void send(string str)
        {
            m_endpoint_mutex.WaitOne();
            byte[] data = Encoding.UTF8.GetBytes(str);
            for(int i=0;i<m_players_endpoint.Count;i++)
                m_client.Send(data, data.Length, m_players_endpoint[i]);
            m_endpoint_mutex.ReleaseMutex();
        }

        public void receive()
        {
            while (m_is_running.Value)
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = m_client.Receive(ref sender);
                string str = Encoding.UTF8.GetString(data);
                string port = sender.Port+"";

                m_endpoint_mutex.WaitOne();
                if (!m_endpoint_to_data.ContainsKey(port))
                    continue;

                m_endpoint_to_data[port] = str;
                m_endpoint_to_data_pending[port] = true;
                m_endpoint_mutex.ReleaseMutex();
            }
        }

        public List<string> pullData()
        {
            List<string> data = new List<string>();
            m_endpoint_mutex.WaitOne();
            for (int i = 0; i < m_players_endpoint.Count; i++)
            {
                string ip1 = m_players_endpoint[i].Address.ToString();
                if (m_endpoint_to_data_pending[ip1])
                {
                    string str = m_endpoint_to_data[ip1];
                    m_endpoint_to_data_pending[ip1] = false;

                    data.Add(str);
                }
            }
            m_endpoint_mutex.ReleaseMutex();

            return data;
        }
    }
}