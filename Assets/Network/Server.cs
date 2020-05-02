using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Network
{
    public class Server
    {
        private TcpListener m_tcp_listener;
        private List<TcpClient> m_tcp_clients;
        private Mutex m_tcp_clients_mutex;
        private Thread m_accept_client_thread;

        public Server(int port)
        {
            m_tcp_clients = new List<TcpClient>();
            m_tcp_clients_mutex = new Mutex();

            m_tcp_listener = new TcpListener(port);
            m_tcp_listener.Start();
            
            m_accept_client_thread = new Thread(acceptClientThreadFunction);
        }

        private void acceptClientThreadFunction()
        {
            int nbClient = 0;

            while (nbClient < 2)
            {
                TcpClient client = m_tcp_listener.AcceptTcpClient();
                m_tcp_clients_mutex.WaitOne();
                m_tcp_clients.Add(client);
                m_tcp_clients_mutex.ReleaseMutex();
                nbClient++;
            }
        }
    }
}