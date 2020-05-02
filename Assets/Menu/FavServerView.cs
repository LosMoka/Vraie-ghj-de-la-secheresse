using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using ModelToView;
using Network;

public class FavServerView : MonoBehaviour
{
    public GameObject EditModeGO, ViewModeGO;

    public Text NameText, IPText, PortText, AutreText;
    public InputField NameIF, IPIF, PortIF, AutreIF;
    public GameObject ServerPrefabGO;
    public Transform ContentTransform;
    private List<ButtonServerView> m_serverlist;
    private ButtonServerView m_currentServer;
    private ServerSearcher m_server_searcher;

    public Button JoinButton;

    private char sep = '~';

    private bool m_editMode;

    public GameManager gameManager;

    //sauvegarde
    private string m_favserv_file_path;
    private DateTime m_last_server_update;
    private long m_time_between_two_server_check;

    // Start is called before the first frame update
    void Start()
    {
        m_favserv_file_path = "./Ressources/FavServer.txt";
        m_server_searcher = new ServerSearcher();
        m_serverlist = new List<ButtonServerView>();
        loadOptions();
        if (m_serverlist.Count>0)
            m_currentServer = m_serverlist[0];
        refreshInfos();
        m_last_server_update = DateTime.Now;
        m_time_between_two_server_check = 2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (NameIF.isFocused)
            {
                EventSystem.current.SetSelectedGameObject(IPIF.gameObject, null);
                IPIF.OnPointerClick(new PointerEventData(EventSystem.current));
            }
            else if(IPIF.isFocused)
            {
                EventSystem.current.SetSelectedGameObject(PortIF.gameObject, null);
                PortIF.OnPointerClick(new PointerEventData(EventSystem.current));
            }
            else if (PortIF.isFocused)
            {
                EventSystem.current.SetSelectedGameObject(AutreIF.gameObject, null);
                AutreIF.OnPointerClick(new PointerEventData(EventSystem.current));
            }
            else if (AutreIF.isFocused)
            {
                EventSystem.current.SetSelectedGameObject(NameIF.gameObject, null);
                NameIF.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (m_editMode)
                OnClickBackEditMode();
            else
                OnClickJoin();
        }

        foreach (var buttonServerView in m_serverlist)
        {
            buttonServerView.setUp(m_server_searcher.serverIsUp(buttonServerView.getIP(),
                Convert.ToInt32(buttonServerView.getPortTCP())));
        }

        if (DateTime.Now.Second >= m_last_server_update.Second + m_time_between_two_server_check || DateTime.Now.Minute>m_last_server_update.Minute)
        {
            m_server_searcher.resetServerIsUp();
            m_last_server_update = DateTime.Now;
        }
    }
        

    public void refreshInfos()
    {
        if (m_currentServer == null)
            return;
        NameText.text = m_currentServer.getName();
        IPText.text = m_currentServer.getIP();
        PortText.text = m_currentServer.getPortTCP();
        AutreText.text = m_currentServer.getPortUDP();
    }

    public void setcurrentServer(ButtonServerView server)
    {
        m_currentServer = server;
        gameManager.ButtonServerView = server;
        refreshInfos();
    }

    public void OnClickNew()
    {
        m_currentServer = newServer("", "", "","");
        OnClickEdit();
    }

    public void OnClickDel()
    {
        if (m_currentServer==null)
            return;
        Destroy(m_currentServer.gameObject);
        m_serverlist.Remove(m_currentServer);
        saveOptions();
        loadOptions();
        if (m_serverlist.Count>0)
            m_currentServer = m_serverlist[0];
        refreshInfos();
    }

    public void OnClickEdit()
    {
        if (m_currentServer == null)
            return;
        m_editMode = true;
        NameIF.text = m_currentServer.getName();
        IPIF.text = m_currentServer.getIP();
        PortIF.text = m_currentServer.getPortTCP();
        AutreIF.text = m_currentServer.getPortUDP();
        EditModeGO.SetActive(m_editMode);
        ViewModeGO.SetActive(!m_editMode);
        EventSystem.current.SetSelectedGameObject(NameIF.gameObject, null);
        NameIF.OnPointerClick(new PointerEventData(EventSystem.current));
        JoinButton.interactable = false;
    }
    public void OnClickBackEditMode()
    {
        m_editMode = false;
        EditModeGO.SetActive(m_editMode);
        ViewModeGO.SetActive(!m_editMode);
        m_currentServer.updateInfos(NameIF.text, IPIF.text, PortIF.text,AutreIF.text);
        string name = m_currentServer.getName(), ip = m_currentServer.getIP(), port = m_currentServer.getPortTCP(), autre = m_currentServer.getPortUDP();
        saveOptions();
        loadOptions();
        m_currentServer = getServerByInfos(name, ip, port, autre);
        refreshInfos();
        JoinButton.interactable = true;
    }
    public void OnClickJoin()
    {
        if (m_currentServer == null)
            return;
        gameManager.connectToServer(m_currentServer.getIP(), Convert.ToInt32(m_currentServer.getPortTCP()),Convert.ToInt32(m_currentServer.getPortUDP()));
    }
    public void OnClickHost()
    {
        if (m_currentServer == null)
            return;
        
        gameManager.hostServer(Convert.ToInt32(m_currentServer.getPortTCP()),Convert.ToInt32(m_currentServer.getPortUDP()));
    }

    private ButtonServerView newServer(string name, string ip, string portTCP, string portUDP)
    {
        GameObject newServerGO = Instantiate(ServerPrefabGO, ContentTransform.transform, true);
        newServerGO.SetActive(true);
        ButtonServerView newServerScript = newServerGO.GetComponent<ButtonServerView>();
        m_serverlist.Add(newServerScript);
        newServerScript.updateInfos(name, ip, portTCP, portUDP);
        m_server_searcher.serverIsUp(ip, Convert.ToInt32(portTCP));
        return (newServerScript);
    }

    private void clearFavServ()
    {
        foreach (ButtonServerView i in m_serverlist)
        {
            Destroy(i.gameObject);
        }
        m_serverlist.Clear();
    }

    private void saveOptions()
    {
        TextWriter writer;
        writer = new StreamWriter(m_favserv_file_path);
        bool first = true;
        foreach (ButtonServerView i in m_serverlist)
        {
            if (!first)
            {
                writer.Write("\r");
            }
            writer.Write(i.getName()
                + sep
                + i.getIP()
                + sep
                + i.getPortTCP()
                +sep
                + i.getPortUDP());
            first = false;
        }
        writer.Close();
    }
    private void loadOptions()
    {
        clearFavServ();
        string[] Serv;
        if (!File.Exists(m_favserv_file_path))
            File.WriteAllText(m_favserv_file_path, "");
        Serv = File.ReadAllLines(m_favserv_file_path);
        foreach (string i in Serv)
        {
            string[] infos = i.Split(sep);
            newServer(infos[0], infos[1], infos[2], infos[3]);
        }
    }
    private ButtonServerView getServerByInfos(string name, string ip, string port, string autre)
    {
        ButtonServerView server = null;
        foreach (ButtonServerView i in m_serverlist)
        {
            if (i.getName() == name && i.getIP()==ip && i.getPortTCP()==port && i.getPortUDP()==autre)
            {
                server = i;
            }
        }
        refreshInfos();
        return (server);
    }
}
