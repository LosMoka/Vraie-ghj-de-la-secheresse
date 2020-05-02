using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Player
    {
        public enum HitType
        {
            NORMAL, DEADLY
        }
        public Vector3 Position { get; private set; }
        public PlayerPerk.PerkAction State { get; private set; }
        
        private int m_competence_point_left;
        private PlayerPerk m_cac_attack, m_dist_att, m_counter_perk, m_dash_perk, m_jump_perk, m_passive_perk, m_bonus_perk;
        public bool HaveShield { get; private set; }
        public bool Alive { get; private set; }
        private DateTime m_shield_broken_datetime;
        private long m_time_before_shield_is_get;

        public Player(Vector3 position)
        {
            Position = position;
            State = PlayerPerk.PerkAction.NONE;
            HaveShield = true;
            Alive = true;
            m_time_before_shield_is_get = 20; //TODO : mettre la bonne valeur
        }

        public void hit(HitType hitType)
        {
            if (hitType == HitType.NORMAL)
            {
                if (HaveShield)
                {
                    HaveShield = false;
                    m_shield_broken_datetime = DateTime.Now;
                    return;
                }
            }

            if (Alive)
            {
                Alive = false;
                State = PlayerPerk.PerkAction.DIEING;
                return;
            }
        }

        public void update()
        {
            if (!HaveShield && DateTime.Now.Ticks >= m_shield_broken_datetime.Ticks + m_time_before_shield_is_get)
            {
                HaveShield = true;
                State = PlayerPerk.PerkAction.GET_SHIELD;
            }
        }

        public void performAction(PlayerPerk.PerkAction action)
        {
            State = action;
        }
        
        public void addCompetencePoint(int competencePoint)
        {
            m_competence_point_left += competencePoint;
        }

        public void movePlayer(Vector3 vec)
        {
            Position = vec;
        }

        public bool canBuyThisPerk(PlayerPerk playerPerk)
        {
            return m_competence_point_left >= playerPerk.CompetencePointCost;
        }

        public void buyThisPerk(PlayerPerk playerPerk)
        {
            if (!canBuyThisPerk(playerPerk))
            {
                Debug.LogError("can buy this perk");
                canBuyThisPerk(playerPerk);
                return;
            }

            m_competence_point_left -= playerPerk.CompetencePointCost;

            switch (playerPerk.Type)
            {
                case PlayerPerk.PerkType.CAC:
                    m_cac_attack = playerPerk;
                    break;
                case PlayerPerk.PerkType.DISTANCE:
                    m_dist_att = playerPerk;
                    break;
                case PlayerPerk.PerkType.COUNTER:
                    m_counter_perk = playerPerk;
                    break;
                case PlayerPerk.PerkType.DASH:
                    m_dash_perk = playerPerk;
                    break;
                case PlayerPerk.PerkType.JUMP:
                    m_jump_perk = playerPerk;
                    break;
                case PlayerPerk.PerkType.PASSIVE:
                    m_passive_perk = playerPerk;
                    break;
                case PlayerPerk.PerkType.BONUS:
                    m_bonus_perk = playerPerk;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}