﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using UnityEngine.SceneManagement;

namespace Network
{
    public class Client : INetworkMessageHandlerRegister
    {
        private static void read_thread(ValueWrapper<bool> isRunning, Client client){
            while(isRunning.Value) {
                client.read();
            }
        }

        private Thread m_read_tcp_thread, m_send_tcp_thread;
        private TcpClient m_tcp_client;
        private UdpClient m_udp_client;
        private Dictionary<string, List<NetworkMessageHandler> > m_network_message_handlers;
        private Mutex m_sending_queue_mutex,m_tcp_client_mutex;
        private string m_ip;
        private int m_port;
        private Queue<string> m_sending_queue;
        private FileStream m_read_file_stream, m_send_file_stream;
        private string m_log_dir_path;
        private string m_separator_token;
        private string m_last_msg_recieved;
        private Queue<string> m_all_data_recieve;
        private Mutex m_all_data_recieve_mutex;
        private Thread m_exec_thread;
        private ValueWrapper<bool> m_is_running;

        public void addNetworkMessageHandler(string token, NetworkMessageHandler handler)
        {
            if(!m_network_message_handlers.ContainsKey(token))
                m_network_message_handlers[token] = new List<NetworkMessageHandler>();
            m_network_message_handlers[token].Add(handler);
        }


        public Client(ValueWrapper<bool> isRunning, string ip, int port)
        {
            m_read_tcp_thread = new Thread(() => read_thread(isRunning, this));
            m_network_message_handlers = new Dictionary<string, List<NetworkMessageHandler>>();
            m_ip = ip;
            m_port = port;
            m_sending_queue = new Queue<string>();
            m_sending_queue_mutex = new Mutex();
            m_tcp_client_mutex = new Mutex();
            m_last_msg_recieved = "";
            m_all_data_recieve = new Queue<string>();
            m_all_data_recieve_mutex = new Mutex();
            m_separator_token = "\n#\n#\n";
            m_is_running = isRunning;

            if (!Directory.Exists("./Logs/"))
                Directory.CreateDirectory("./Logs/");

            m_log_dir_path = "./Logs/log_" + DateTime.Now.ToString().Replace(':', '_').Replace('/', '_') + "/";
            Directory.CreateDirectory(m_log_dir_path);
            

            m_send_tcp_thread = new Thread(sendThreadFunction);
            m_exec_thread = new Thread(execThreadFunction);
            m_exec_thread.Start();
        }

        private void sendThreadFunction()
        {
            while (m_is_running.Value)
            {
                m_sending_queue_mutex.WaitOne();
                if (m_sending_queue.Any())
                {
                    string f = "";
                    while (m_sending_queue.Any())
                        f += m_sending_queue.Dequeue() + m_separator_token;
                    try
                    {
                        m_send_file_stream = File.Open(m_log_dir_path + "send.txt", FileMode.Append);
                        byte[] b = Encoding.UTF8.GetBytes(f);
                        m_send_file_stream.Write(b, 0, b.Length);
                        m_send_file_stream.Close();
                    }
                    catch (IOException e)
                    {

                    }

                    m_sending_queue_mutex.ReleaseMutex();
                    m_tcp_client_mutex.WaitOne();
                    write(f);
                    m_tcp_client_mutex.ReleaseMutex();
                }
                else
                    m_sending_queue_mutex.ReleaseMutex();

                Thread.Sleep(150);
            }
        }

        private void execThreadFunction()
        {
            while (m_is_running.Value)
            {
                m_all_data_recieve_mutex.WaitOne();
                while (m_all_data_recieve.Any())
                {
                    string dataRecieve = m_all_data_recieve.Dequeue();
                    try
                    {
                        m_all_data_recieve_mutex.ReleaseMutex();
                    }
                    catch (ApplicationException e)
                    {
                        //TODO : trouver pourquoi ça fait ça
                    }

                    if (dataRecieve == m_last_msg_recieved && dataRecieve != "NT")
                        continue;
                    m_last_msg_recieved = dataRecieve;

                    try
                    {
                        m_read_file_stream = File.Open(m_log_dir_path + "read.txt", FileMode.Append);
                        byte[] b = Encoding.UTF8.GetBytes(dataRecieve + m_separator_token);
                        m_read_file_stream.Write(b, 0, b.Length);
                        m_read_file_stream.Close();
                    }
                    catch (IOException e)
                    {

                    }

                    string iss = dataRecieve;

                    string str = "";
                    std.getline(ref iss, ref str, ' ');

                    if (str == "")
                        return;

                    //Debug.Log("client " + m_player_id + " read : " + dataRecieve + "\n");

                    try
                    {
                        if (m_network_message_handlers.ContainsKey(str))
                        {
                            foreach (var networkMessageHandler in m_network_message_handlers[str])
                            {
                                networkMessageHandler(iss);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error while handling " + str);
                        Debug.Log(e.Message);
                        
                        m_exec_thread = new Thread(execThreadFunction);
                        m_exec_thread.Start();
                        throw;
                    }

                    m_all_data_recieve_mutex.WaitOne();
                }

                try
                {
                    m_all_data_recieve_mutex.ReleaseMutex();
                }
                catch (Exception e) //Oula pas beau
                {
                }

                Thread.Sleep(150);
            }
        }

        public void connect()
        {
            m_tcp_client = new TcpClient(m_ip,m_port);
            m_udp_client = new UdpClient(m_ip, m_port);
            m_read_tcp_thread.Start();
            m_send_tcp_thread.Start();
        }
        
        public static void ReturnToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        void read()
        {
            m_tcp_client_mutex.WaitOne();
            m_tcp_client_mutex.ReleaseMutex();
            {
                string dataRecieve = "";
                NetworkStream ns = m_tcp_client.GetStream();

                byte[] bytes = new byte[4096];
                int bytesRead;
                do
                {
                    bytesRead = ns.Read(bytes, 0, bytes.Length);

                    if (bytesRead==0)
                    {
                        Thread.Sleep(1000);
                        bytesRead = ns.Read(bytes, 0, bytes.Length);
                        if (bytesRead == 0)
                        {
                            m_is_running.Value = false;
                            //PopUpView.enqueuePopUp("Halp, on a perdu la co au serveur !", "Snif...", null, false, ReturnToMenu,null);
                            return;
                        }
                    }

                    dataRecieve += uncompress(bytes);

                    while (dataRecieve.Contains(m_separator_token))
                    {
                        string[] sDataRecieve = dataRecieve.Split(new[] {m_separator_token},
                            StringSplitOptions.RemoveEmptyEntries);
                        m_all_data_recieve_mutex.WaitOne();
                        m_all_data_recieve.Enqueue(sDataRecieve[0]);
                        m_all_data_recieve_mutex.ReleaseMutex();
                        dataRecieve = "";
                        for (int i = 1; i < sDataRecieve.Length; i++)
                            dataRecieve += sDataRecieve[i] + m_separator_token;
                    }

                } while (bytesRead >= bytes.Length);
            }
        }

        private string uncompress(byte[] data)
        {
            try
            {
                MemoryStream msi = new MemoryStream(data);
                MemoryStream mso = new MemoryStream();
                GZipStream decompressionStream = new GZipStream(msi, CompressionMode.Decompress);
                int cnt;
                byte[] bytes = new byte[4096];
                while ((cnt = decompressionStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    mso.Write(bytes, 0, cnt);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
            catch (IOException e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.Source);
            }

            return "";
        }

        public void send(string str, Point2i p1, Point2i p2) {
            send(str+" "+p1.ToString()+" "+p2.ToString());
        }

        private void write(string str)
        {
            NetworkStream ns = m_tcp_client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            ns.Write(buffer,0,buffer.Length);
        }
        
        public void send(string str)
        {
            m_sending_queue_mutex.WaitOne();
            //Debug.Log("send : "+str);
            m_sending_queue.Enqueue(str);
            m_sending_queue_mutex.ReleaseMutex();
        }

        void join() {
            m_read_tcp_thread.Join();
        }

        ~Client() {
        }

        void close() {
            m_tcp_client.Close();
        }
    }
}