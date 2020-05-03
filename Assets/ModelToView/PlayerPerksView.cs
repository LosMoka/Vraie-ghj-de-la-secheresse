using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
public class PlayerPerksView : MonoBehaviour
{
    public PlayerPerk playerPerk;
    public bool isPrefab;
    public PlayerPerk.PerkType type;
    public int cost;
    private static int m_id_counter;

    public void Awake()
    { 
        if (isPrefab)
        {
            isPrefab = false;
            playerPerk = new PlayerPerk(type, cost);
            m_id_counter++;
        }
    }
}
