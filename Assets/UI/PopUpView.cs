using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Threading;

public class PopUpView : MonoBehaviour
{
    public Button Rbutton, Lbutton;
    public Text textSpace;
    public delegate void Type();
    private Type RbuttonClick, LbuttonClick;
    public Image fond;
    public GameObject ButtonPopUp, LoadingPopUp;
    public Image BackgroundBar, LoadingBar;
    private Queue<Tuple<string, string, string, bool, Type, Type>> m_pop_up_queue;
    private Mutex m_pop_up_queue_mutex;
    private static PopUpView instance;
    public GameObject hideOrShowGameObject;

    public void setButtonPopUp(string Text, string RbuttonText, string LbuttonText, bool TwoButton, Type RightButton, Type LeftButton)
    {
        setImagePopUp(null);
        hideOrShowGameObject.SetActive(true);
        ButtonPopUp.SetActive(true);
        textSpace.text = Text;
        Rbutton.GetComponentInChildren<Text>().text = RbuttonText;
        Lbutton.GetComponentInChildren<Text>().text = LbuttonText;
        RbuttonClick = RightButton;
        LbuttonClick = LeftButton;
        if (TwoButton) { Rbutton.gameObject.SetActive(true); Lbutton.gameObject.SetActive(true); }
        else { Rbutton.gameObject.SetActive(true); Lbutton.gameObject.SetActive(false); }
    }

    public static void enqueuePopUp(string Text, string RbuttonText, string LbuttonText, bool TwoButton,
        Type RightButton, Type LeftButton)
    {
        instance._enqueuePopUp(Text, RbuttonText, LbuttonText, TwoButton, RightButton, LeftButton);
    }

    private void _enqueuePopUp(string Text, string RbuttonText, string LbuttonText, bool TwoButton,
        Type RightButton, Type LeftButton)
    {
        m_pop_up_queue_mutex.WaitOne();
        m_pop_up_queue.Enqueue(new Tuple<string, string, string, bool, Type, Type>(Text, RbuttonText, LbuttonText, TwoButton, RightButton, LeftButton));
        m_pop_up_queue_mutex.ReleaseMutex();
    }


    public void setImagePopUp(Sprite i)
    {
        hideOrShowGameObject.SetActive(true);
        fond.gameObject.SetActive(true);
        if (i == null)
        {
            fond.color = new Color(0f, 0f, 0f, 0f);
        }
        else
        {
            fond.color = new Color(0f, 0f, 0f, 255f);
            fond.sprite = i;
        }
    }

    public void loadingBarPopUp(float avancement, Sprite background)
    {
        hideOrShowGameObject.SetActive(true);
        LoadingPopUp.SetActive(true);
        setImagePopUp(background);
        Vector2 maxSize = BackgroundBar.rectTransform.sizeDelta - new Vector2(10, 10);
        if (avancement < 0)
        {
            LoadingBar.rectTransform.sizeDelta = new Vector2(0f, maxSize.y);
        }
        if (avancement > 1)
        {
            LoadingBar.rectTransform.sizeDelta = new Vector2(maxSize.x, maxSize.y);
            LoadingBar.rectTransform.localPosition = BackgroundBar.rectTransform.localPosition;
        }
        else
        {
            LoadingBar.rectTransform.sizeDelta = new Vector2(avancement * maxSize.x, maxSize.y);
            LoadingBar.rectTransform.localPosition = new Vector3((avancement / 2 - 0.5f) * maxSize.x, 0f, 0f) + BackgroundBar.rectTransform.localPosition;
        }
    }

    public void OnRbuttonClick()
    {
        RbuttonClick?.Invoke();
        clearPopUp();
    }
    public void OnLbuttonClick()
    {
        LbuttonClick?.Invoke();
        clearPopUp();
    }

    public void clearPopUp()
    {
        hideOrShowGameObject.SetActive(false);
        fond.gameObject.SetActive(false);
        ButtonPopUp.SetActive(false);
        LoadingPopUp.SetActive(false);
        RbuttonClick = null;
        LbuttonClick = null;
    }
    // Start is called before the first frame update
    void Awake()
    {
        hideOrShowGameObject.SetActive(false);
        clearPopUp();
        m_pop_up_queue_mutex = new Mutex();
        m_pop_up_queue = new Queue<Tuple<string, string, string, bool, Type, Type>>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (hideOrShowGameObject.activeSelf) return;
        m_pop_up_queue_mutex.WaitOne();
        if (m_pop_up_queue.Any())
        {
            var t = m_pop_up_queue.Dequeue();
            setButtonPopUp(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6);
        }
        m_pop_up_queue_mutex.ReleaseMutex();
    }
}
