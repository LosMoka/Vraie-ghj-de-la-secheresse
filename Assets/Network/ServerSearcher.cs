﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Network
{
    public class ServerSearcher
    {
        private Dictionary<string, Thread> m_server_is_up_delegates;
        private Dictionary<string, bool> m_server_is_up_state;
        private Mutex m_server_is_up_mutex;

        public void resetServerIsUp()
        {
            m_server_is_up_delegates.Clear();
            m_server_is_up_state.Clear();
        }
        public bool serverIsUp(string ip, int port)
        {
            if (ip == "localhost")
                ip = "127.0.0.1";
            m_server_is_up_mutex.WaitOne();
            bool containsServerIsUp = m_server_is_up_state.ContainsKey(ip + ":" + port);
            m_server_is_up_mutex.ReleaseMutex();

            if (containsServerIsUp)
                return m_server_is_up_state[ip + ":" + port];
            
            m_server_is_up_mutex.WaitOne();
            m_server_is_up_state[ip + ":" + port] = false;
            m_server_is_up_mutex.ReleaseMutex();
            
            m_server_is_up_delegates[ip+":"+port] = new Thread(() =>
            {
                try
                {
                    UdpClient udpClient = new UdpClient(ip, port);
                    byte[] b = Encoding.UTF8.GetBytes("AreYouUp");
                    udpClient.Send(b, b.Length);

                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                    byte[] rb = udpClient.Receive(ref ipEndPoint);
                    m_server_is_up_mutex.WaitOne();
                    m_server_is_up_state[ip + ":" + port] = true;
                    m_server_is_up_mutex.ReleaseMutex();
                }
                catch(Exception e)
                {
                    
                }

                try
                {
                    m_server_is_up_mutex.ReleaseMutex();
                }
                catch(Exception e)
                {
                    
                }
            });
            m_server_is_up_delegates[ip+":"+port].Start();

            return false;
        }

        public ServerSearcher()
        {
            m_server_is_up_delegates = new Dictionary<string, Thread>();
            m_server_is_up_state = new Dictionary<string, bool>();
            m_server_is_up_mutex = new Mutex();
        }
    }
}