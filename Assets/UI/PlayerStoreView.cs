﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using ModelToView;

public class PlayerStoreView : MonoBehaviour
{
    PlayerStore m_player_store;
    public Transform Attack, Defense, Movement;
    public GameObject Bucheron, Pyro;
    // Start is called before the first frame update
    void Start()
    {
        m_player_store = new PlayerStore();
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        if (gameManagerGameObject == null)
        {
            m_player_store = new PlayerStore();
        }
        else
        {
            GameManager gameManager = gameManagerGameObject.GetComponent<GameManager>();
            //METTRE LE BON PLAYERSTORE !!!

            //m_player_store = gameManager.
            //m_client = gameManager.Client;
        }
    }

    public void initializeButtons()
    {
        //mettre les ints en les cherchant dans PlayerStore
        /*
        int attack = m_player_store.AttackPerks.Count;
        int defense = m_player_store.DefensePerks.Count;
        int movement = m_player_store.DisplacementPerks.Count;
        */
        int attack=2, defense, movement;

        bool isBucheron = true;

        Bucheron.SetActive(isBucheron);
        Pyro.SetActive(!isBucheron);

        foreach (Transform i in Attack)
        {
            i.GetComponent<Button>().interactable = false;
            if (i.GetComponent<PerksView>().index == attack)
            {
                i.GetComponent<Button>().interactable = true;
            }
        }
        foreach (Transform i in Defense)
        {
            i.GetComponent<Button>().interactable = false;
            if (i.GetComponent<PerksView>().index == attack)
            {
                i.GetComponent<Button>().interactable = true;
            }
        }
        foreach (Transform i in Movement)
        {
            i.GetComponent<Button>().interactable = false;
            if (i.GetComponent<PerksView>().index == attack)
            {
                i.GetComponent<Button>().interactable = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BuyPerks(Button i, PlayerStore.PerkClass clas, int index)
    {
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