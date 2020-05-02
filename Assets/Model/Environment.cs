using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Environment
    {
        private int m_gold;
        private List<MapTrap> m_map_traps_left;
        private List<MapElement> m_map_elements;

        public Environment(int gold)
        {
            m_gold = gold;
            m_map_elements = new List<MapElement>();
            m_map_traps_left = new List<MapTrap>();
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
            
            m_map_traps_left.Add(mapTrap);
        }

        public bool canBuyThisElement(MapElement mapElement)
        {
            foreach (var element in m_map_elements)
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
            
            m_map_elements.Add(mapElement);
        }
    }
}