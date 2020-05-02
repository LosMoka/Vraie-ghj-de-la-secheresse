using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Environment
    {
        private int m_gold;
        public List<MapTrap> MapTrapsLeft { get; }
        public List<MapElement> Maplements { get; }

        public Environment(int gold)
        {
            m_gold = gold;
            Maplements = new List<MapElement>();
            MapTrapsLeft = new List<MapTrap>();
        }
        
        public bool canBuyThisTrap(MapTrap mapTrap)
        {
            return m_gold >= mapTrap.Cost;
        }

        public void buyThisTrap(MapTrap mapTrap)
        {
            if (!canBuyThisTrap(mapTrap))
            {
                Debug.LogError("can't but this trap");
                canBuyThisTrap(mapTrap);
                return;
            }

            m_gold -= mapTrap.Cost;

            MapTrapsLeft.Add(mapTrap);
        }

        public bool canBuyThisElement(MapElement mapElement)
        {
            foreach (var element in Maplements)
            {
                if (element.Id == mapElement.Id)
                    return false;
            }
            
            return m_gold >= mapElement.Cost;
        }

        public void buyThisElementperk(MapElement mapElement)
        {
            if (!canBuyThisElement(mapElement))
            {
                Debug.LogError("can't but this element");
                canBuyThisElement(mapElement);
                return;
            }
            
            m_gold -= mapElement.Cost;

            Maplements.Add(mapElement);
        }
    }
}