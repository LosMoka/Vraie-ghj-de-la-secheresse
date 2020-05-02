using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonServerView : MonoBehaviour
{
    public Text ServerNameText;
    public Image voyantImage;
    public FavServerView favServerView;
    private string m_name, m_ip, m_port_tcp,m_port_udp;
    private bool m_isUp;

    private void Start()
    {
        m_isUp = false;
    }

    public void OnClick()
    {
        favServerView.setcurrentServer(GetComponent<ButtonServerView>());
    }
	
    public void updateInfos(string name, string ip, string portTCP, string portUDP)
    {
        m_name = name;
        ServerNameText.text = m_name;
        m_ip = ip;
        m_port_tcp = portTCP;
        m_port_udp = portUDP;
    }

    public void setUp(bool isUp)
    {
        m_isUp = isUp;
        if (m_isUp)
        {
            voyantImage.color = Color.green;
        }
        else
        {
            voyantImage.color = Color.red;
        }
    }

    public string getName()
    {
        return (m_name);
    }
    public string getIP()
    {
        return (m_ip);
    }
    public string getPortTCP()
    {
        return (m_port_tcp);
    }
    public string getPortUDP()
    {
        return (m_port_udp);
    }
    public string getAutre()
    {
        return "";
    }
}
