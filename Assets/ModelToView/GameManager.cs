using System;
using Network;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace ModelToView
{
    public class GameManager : MonoBehaviour
    {
        private Client m_client;
        private Server m_server;
        private ClientUdp m_client_udp;
        private ValueWrapper<bool> m_is_running;

        public ButtonServerView ButtonServerView { get; set; }

        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject); 
            m_is_running = new ValueWrapper<bool>(true);
            m_server = null;
        }

        public void Update()
        {
        }

        public void connectToServer(string ip, int portTCp, int portUDP)
        {
            m_client = new Client(m_is_running,ip,portTCp,portUDP);
            m_client_udp = new ClientUdp(m_is_running,portUDP,m_client, ip, portTCp);
            m_client.connect();
            Debug.Log("connect with ip="+ip+" port TCP="+portTCp+" port Udp="+portUDP);
        }

        public void hostServer(int portTCP, int portUDP)
        {
            m_server = new Server(m_is_running,portTCP);
            connectToServer("127.0.0.1", portTCP, portUDP);
        }
    }
}