using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class FavServerView : MonoBehaviour
{
    public GameObject EditModeGO, ViewModeGO;

    public Text NameText, IPText, PortText, AutreText;
    public InputField NameIF, IPIF, PortIF, AutreIF;
    public GameObject ServerPrefabGO;
    public Transform ContentTransform;
    private List<ButtonServerView> m_serverlist;
    private ButtonServerView m_currentServer;

    public Button JoinButton;

    private char sep = '~';

    private bool m_editMode;

    //sauvegarde
    private string m_favserv_file_path;

    // Start is called before the first frame update
    void Start()
    {
        m_favserv_file_path = "./Ressources/FavServer.txt";
        m_serverlist = new List<ButtonServerView>();
        loadOptions();
        if (m_serverlist.Count>0)
            m_currentServer = m_serverlist[0];
        refreshInfos();
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
    }

    public void refreshInfos()
    {
        if (m_currentServer == null)
            return;
        NameText.text = "Name : " + m_currentServer.getName();
        IPText.text = "IP : " + m_currentServer.getIP();
        PortText.text = "Port : " + m_currentServer.getPort();
        AutreText.text = m_currentServer.getAutre();
    }

    public void setcurrentServer(ButtonServerView server)
    {
        m_currentServer = server;
        refreshInfos();
    }

    public void OnClickNew()
    {
        m_currentServer = newServer("", "", "", "");
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
        PortIF.text = m_currentServer.getPort();
        AutreIF.text = m_currentServer.getAutre();
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
        m_currentServer.updateInfos(NameIF.text, IPIF.text, PortIF.text, AutreIF.text);
        string name = m_currentServer.getName(), ip = m_currentServer.getIP(), port = m_currentServer.getPort(), autre=m_currentServer.getAutre();
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

        //tu fais ce que tu veux avec ça
        
        Debug.Log(m_currentServer.getIP());
        Debug.Log(m_currentServer.getPort());
    }

    private ButtonServerView newServer(string name, string ip, string port, string autre)
    {
        GameObject newServerGO = Instantiate(ServerPrefabGO, ContentTransform.transform, true);
        newServerGO.SetActive(true);
        ButtonServerView newServerScript = newServerGO.GetComponent<ButtonServerView>();
        m_serverlist.Add(newServerScript);
        newServerScript.updateInfos(name, ip, port, autre);
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
                + i.getPort()
                + sep
                + i.getAutre());
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
            if (i.getName() == name && i.getIP()==ip && i.getPort()==port && i.getAutre()==autre)
            {
                server = i;
            }
        }
        refreshInfos();
        return (server);
    }
}
