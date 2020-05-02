using System;
using System.Collections.Generic;

namespace Model
{
    public class EnvironmentStore
    {
        private List<MapTrap> m_map_traps;
        private List<MapElement> m_map_elements;
        private Environment m_environment;

        public EnvironmentStore(Environment environment)
        {
            m_environment = environment;
            m_map_traps = new List<MapTrap>();
            m_map_elements = new List<MapElement>();
        }
        
        public enum EnvironmentPerkClass
        {
            TRAP,ELEMENT
        }

        public bool buyEnvironmentPerk(MapTrap mapTrap)
        {
            if (!m_environment.canBuyThisTrap(mapTrap))
                return false;
            m_environment.buyThisTrap(mapTrap);
            return true;
        }
        public bool buyEnvironmentPerk(MapElement mapElement)
        {
            if (!m_environment.canBuyThisElement(mapElement)) 
                return false;
            m_environment.buyThisElementperk(mapElement);
            return true;
        }
    }
}