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
    public bool isBucheron;
    private static int m_id_counter;

    public void Awake()
    { 
        if (isPrefab)
        {
            isPrefab = false;
            playerPerk = new PlayerPerk(type, cost, isBucheron);
            m_id_counter++;
        }
    }
    public PlayerPerk getplayerPerk()
    {
        return (playerPerk);
    }
}
