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
        public List<PlayerPerk> DefensePerks{ get; }
        public List<PlayerPerk> DisplacementPerks{ get; }

        public PlayerStore()
        {
            AttackPerks = new List<PlayerPerk>();
            DefensePerks = new List<PlayerPerk>();
            DisplacementPerks = new List<PlayerPerk>();
        }
        
        public PlayerStore(Player player)
        {
            m_player = player;
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

            if (!m_player.canBuyThisPerk(perk)) 
                return false;
            
            m_player.buyThisPerk(perk);
            
            return true;

        }
    }
}