using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonServerView : MonoBehaviour
{
    public Text ServerNameText;
    public Image voyantImage;
    public FavServerView favServerView;
    private string m_name, m_ip, m_port, m_autre;
    private bool m_isUp;

    private void Start()
    {
        m_isUp = false;
    }

    public void OnClick()
    {
        favServerView.setcurrentServer(GetComponent<ButtonServerView>());
    }

    public void updateInfos(string name, string ip, string port, string autre)
    {
        m_name = name;
        ServerNameText.text = m_name;
        m_ip = ip;
        m_port = port;
        m_autre = autre;
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
    public string getPort()
    {
        return (m_port);
    }
    public string getAutre()
    {
        return (m_autre);
    }
}
