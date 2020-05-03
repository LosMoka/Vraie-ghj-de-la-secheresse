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
        public Client Client { get; private set; }
        private Server m_server;
        public ClientUdp ClientUdp { get; private set; }
        private ValueWrapper<bool> m_is_running;
        private MapManager m_map_manager;

        private delegate void ExecOnMainThreadDelegate();

        private Queue<ExecOnMainThreadDelegate> m_exec_on_main_thread_delegates;
        private Mutex m_exec_on_main_thread_delegate_mutex;

        public ButtonServerView ButtonServerView { get; set; }

        public Model.Environment environmentInstance { get; }
        public Model.EnvironmentStore EnvironmentStore { get; }
        public MapManager MapManager { get; }

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
            Client = new Client(m_is_running,ip,portTCp,portUDP);
            ClientUdp = new ClientUdp(m_is_running,portUDP,Client, ip, portTCp, delegate
            {
                m_exec_on_main_thread_delegate_mutex.WaitOne();
                m_exec_on_main_thread_delegates.Enqueue(() => { SceneManager.LoadScene(nextSceneToLoad); });
                m_exec_on_main_thread_delegate_mutex.ReleaseMutex();
            });
            Client.connect();
            Debug.Log("connect with ip="+ip+" port TCP="+portTCp+" port Udp="+portUDP);
        }

        public void hostServer(int portTCP, int portUDP)
        {
            m_server = new Server(m_is_running,portTCP);
            connectToServer("127.0.0.1", portTCP, portUDP,"EnvStore");
        }
    }
}