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
        
        public bool buyEnvironmentPerk(EnvironmentPerkClass perkClass, int index)
        {
            switch (perkClass)
            {
                case EnvironmentPerkClass.TRAP:
                    if (!m_environment.canBuyThisTrap(m_map_traps[index])) 
                        return false;
                    m_environment.buyThisTrap(m_map_traps[index]);
                    break;
                case EnvironmentPerkClass.ELEMENT:
                    if (!m_environment.canBuyThisElement(m_map_elements[index])) 
                        return false;
                    m_environment.buyThisElementperk(m_map_elements[index]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(perkClass), perkClass, null);
            }

            return true;

        }

    }
}