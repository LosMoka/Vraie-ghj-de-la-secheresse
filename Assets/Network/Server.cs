using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Utils;

namespace Network
{
    public class Server
    {
        private TcpListener m_tcp_listener;
        private List<TcpClient> m_tcp_clients;
        private Dictionary<int,string> m_client_ip_address;
        private Mutex m_tcp_clients_mutex;
        private Thread m_accept_client_thread, m_udp_thread;
        private UdpClient m_udp_client;
        private ValueWrapper<bool> m_is_running;
        private string m_separator_token;

        public Server(ValueWrapper<bool> isRunning, int port)
        {
            m_separator_token = "\n#\n#\n";
            m_is_running = isRunning;
            m_tcp_clients = new List<TcpClient>();
            m_tcp_clients_mutex = new Mutex();
            m_client_ip_address = new Dictionary<int, string>();
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            m_udp_client = new UdpClient(ipep);

            m_tcp_listener = new TcpListener( IPAddress.Any, port);
            
            m_udp_thread = new Thread(udpServerThread);
            m_accept_client_thread = new Thread(acceptClientThreadFunction);
            m_udp_thread.Start();
            m_accept_client_thread.Start();
            m_tcp_listener.Start();
            
            Debug.Log("server is listening on "+port);
        }

        private void udpServerThread()
        {
            while (m_is_running.Value)
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = m_udp_client.Receive(ref sender);
                string str = Encoding.UTF8.GetString(data);
                string ip = sender.Address.ToString()+":"+sender.Port;

                if (str == "AreYouUp")
                {
                    m_udp_client.Send(data, data.Length, sender);
                    continue;
                }

                try
                {
                    int id = Convert.ToInt32(str);
                    m_client_ip_address[id] = ip;
                    Debug.Log("recieve ip = " + ip);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void acceptClientThreadFunction()
        {
            int nbClient = 0;

            while (nbClient < 3)
            {
                TcpClient client = m_tcp_listener.AcceptTcpClient();
                m_tcp_clients_mutex.WaitOne();
                m_tcp_clients.Add(client);
                Debug.Log("new client nbClient="+nbClient);
                sendTo(nbClient, "GIP "+nbClient);
                m_tcp_clients_mutex.ReleaseMutex();
                nbClient++;
            }

            while (m_client_ip_address.Count != m_tcp_clients.Count)
            {
                Thread.Sleep(100);
            }

            string str = m_client_ip_address.Aggregate("CC ", (current, clientipAddress) => current + (clientipAddress.Value + "\n"));
            Debug.Log("send str : "+str);

            send(str);
        }

        private void send(string str)
        {
            for(int i=0;i<m_tcp_clients.Count;i++)
                sendTo(i,str);
        }

        private void sendTo(int clientId, string str)
        {
            str += m_separator_token;
            TcpClient client = m_tcp_clients[clientId];
            NetworkStream stream = client.GetStream();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            stream.Write(buffer,0,buffer.Length);
        }
    }
}