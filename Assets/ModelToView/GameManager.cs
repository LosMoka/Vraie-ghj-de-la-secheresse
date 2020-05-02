using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        private delegate void ExecOnMainThreadDelegate();

        private Queue<ExecOnMainThreadDelegate> m_exec_on_main_thread_delegates;
        private Mutex m_exec_on_main_thread_delegate_mutex;

        public ButtonServerView ButtonServerView { get; set; }

        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject); 
            m_is_running = new ValueWrapper<bool>(true);
            m_server = null;
            m_exec_on_main_thread_delegates = new Queue<ExecOnMainThreadDelegate>();
            m_exec_on_main_thread_delegate_mutex = new Mutex();
        }

        public void Update()
        {
            m_exec_on_main_thread_delegate_mutex.WaitOne();
            while (m_exec_on_main_thread_delegates.Any())
            {
                m_exec_on_main_thread_delegates.Dequeue()();
            }
            m_exec_on_main_thread_delegate_mutex.ReleaseMutex();
        }

        public void connectToServer(string ip, int portTCp, int portUDP, string nextSceneToLoad)
        {
            m_client = new Client(m_is_running,ip,portTCp,portUDP);
            m_client_udp = new ClientUdp(m_is_running,portUDP,m_client, ip, portTCp, delegate
            {
                m_exec_on_main_thread_delegate_mutex.WaitOne();
                m_exec_on_main_thread_delegates.Enqueue(() => { SceneManager.LoadScene(nextSceneToLoad); });
                m_exec_on_main_thread_delegate_mutex.ReleaseMutex();
            });
            m_client.connect();
            Debug.Log("connect with ip="+ip+" port TCP="+portTCp+" port Udp="+portUDP);
        }

        public void hostServer(int portTCP, int portUDP)
        {
            m_server = new Server(m_is_running,portTCP);
            connectToServer("127.0.0.1", portTCP, portUDP,"EnvStore");
        }
    }
}