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

        public ClientUdp(ValueWrapper<bool> isRunning, int port, Client tcpClient)
        {
            isRunning = m_is_running;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            m_client = new UdpClient(ipep);
            m_players_endpoint = new List<IPEndPoint>();
            m_endpoint_mutex = new Mutex();
            
            tcpClient.addNetworkMessageHandler("CC", data =>
            {
                m_endpoint_mutex.WaitOne();
                string[] ips = data.Split('\n');
                if (ips.Length < 3)
                {
                    Debug.LogError("On a pas 3 clients, c'est pas normal");
                    return;
                }
                
                for (int i = 0; i < ips.Length; i++)
                {
                    m_endpoint_to_data[ips[i]] = "";
                    m_endpoint_to_data_pending[ips[i]] = false;
                    m_players_endpoint.Add(new IPEndPoint(IPAddress.Parse(ips[i]), port));
                }
                
                m_receive_thread.Start();
                m_endpoint_mutex.ReleaseMutex();
            } );
            
            
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
                string ip = sender.Address.ToString();

                m_endpoint_mutex.WaitOne();
                if (!m_endpoint_to_data.ContainsKey(ip))
                    continue;

                m_endpoint_to_data[ip] = str;
                m_endpoint_to_data_pending[ip] = true;
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