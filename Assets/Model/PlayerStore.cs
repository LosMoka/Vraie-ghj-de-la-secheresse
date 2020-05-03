using System;
using System.Collections.Generic;

namespace Model
{
    public class PlayerStore
    {
        public enum PerkClass
        {
            ATTACK,DEFENSE,DISPLACEMENT
        }
        
        private Player m_player;
        public List<PlayerPerk> AttackPerks { get; }
        public int AttackPerkCount { get; private set; }
        public List<PlayerPerk> DefensePerks{ get; }
        public int DefensePerkCount { get; private set; }
        public List<PlayerPerk> DisplacementPerks{ get; }
        public int DisplacementPerkCount { get; private set; }

        public PlayerStore()
        {
            AttackPerks = new List<PlayerPerk>();
            DefensePerks = new List<PlayerPerk>();
            DisplacementPerks = new List<PlayerPerk>();
            AttackPerkCount = 0;
            DefensePerkCount = 0;
            DisplacementPerkCount = 0;
        }
        
        public PlayerStore(Player player)
        {
            m_player = player;
        }

        public bool buyPlayerPerk(PlayerPerk perk)
        {
            if (!m_player.canBuyThisPerk(perk)) 
                return false;
            
            m_player.buyThisPerk(perk);
            
            return true;
        }

        public bool buyPlayerPerk(PerkClass perkClass, int index)
        {
            PlayerPerk perk;

            switch (perkClass)
            {
                case PerkClass.ATTACK:
                    perk = AttackPerks[index];
                    break;
                case PerkClass.DEFENSE:
                    perk = DefensePerks[index];
                    break;
                case PerkClass.DISPLACEMENT:
                    perk = DisplacementPerks[index];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(perkClass), perkClass, null);
            }

            bool success = buyPlayerPerk(perk);

            if (success)
            {
                switch (perkClass)
                {
                    case PerkClass.ATTACK:
                        AttackPerkCount++;
                        break;
                    case PerkClass.DEFENSE:
                        DefensePerkCount++;
                        break;
                    case PerkClass.DISPLACEMENT:
                        DisplacementPerkCount++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(perkClass), perkClass, null);
                }
            }

            return success;
        }
        
        public bool BucheronOuMagicien { get; set; }

        public void onlyDevAddAttackPerk(PlayerPerk p)
        {
            AttackPerks.Add(p);
        }
        public void onlyDevAddDefensePerk(PlayerPerk p)
        {
            AttackPerks.Add(p);
        }
        public void onlyDevAddDisplacementPerk(PlayerPerk p)
        {
            AttackPerks.Add(p);
        }
    }
}