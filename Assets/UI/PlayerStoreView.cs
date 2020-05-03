using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using ModelToView;
using Network;
public class PlayerStoreView : MonoBehaviour
{
    PlayerStore m_player_store;
    public List<Transform> Attack, Defense, Movement;
    public GameObject Bucheron, Pyro;
    private Client m_client;

    // Start is called before the first frame update
    void Start()
    {
        m_player_store = new PlayerStore();
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        if (gameManagerGameObject == null)
        {
            m_player_store = new PlayerStore();
            m_client = null;
        }
        else
        {
            GameManager gameManager = gameManagerGameObject.GetComponent<GameManager>();
            m_player_store = gameManager.PlayerStore;
            m_client = gameManager.Client;
        }
        initializeButtons();
    }

    public void initializeButtons()
    {
        //mettre les ints en les cherchant dans PlayerStore
        int attack = m_player_store.AttackPerks.Count;
        int defense = m_player_store.DefensePerks.Count;
        int movement = m_player_store.DisplacementPerks.Count;
        
        //int attack=2, defense=1, movement=3;



        //TODO : get si c'est un bucheron ou un pyro
        bool isBucheron = false;

        Bucheron.SetActive(isBucheron);
        Pyro.SetActive(!isBucheron);

        foreach (Transform j in Attack)
        {
            foreach (Transform i in j)
            {
                i.GetComponent<Button>().interactable = false;
                if (i.GetComponent<PerksView>().index == attack)
                {
                    i.GetComponent<Button>().interactable = true;
                }
            }
        }
        foreach (Transform j in Defense)
        {
            foreach (Transform i in j)
            {
                i.GetComponent<Button>().interactable = false;
                if (i.GetComponent<PerksView>().index == defense)
                {
                    i.GetComponent<Button>().interactable = true;
                }
            }
        }
        foreach (Transform j in Movement)
        {
            foreach (Transform i in j)
            {
                i.GetComponent<Button>().interactable = false;
                if (i.GetComponent<PerksView>().index == movement)
                {
                    i.GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BuyPerks(Button i, PlayerStore.PerkClass clas, int index)
    {
        Debug.Log(index);
        bool j = m_player_store.buyPlayerPerk(clas, index);
        if (j)
        {
            i.interactable = false;
        }
    }
    public void next()
    {

    }
}
