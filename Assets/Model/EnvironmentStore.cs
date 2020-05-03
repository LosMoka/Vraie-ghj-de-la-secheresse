using System;
using System.Collections.Generic;

namespace Model
{
    public class EnvironmentStore
    {
        public List<MapTrap> MapTraps { get; }
        public List<MapElement> MapElements{ get; }
        public List<MapMalus> MapMalus{ get; }
        private Environment m_environment;

        public EnvironmentStore(Environment environment)
        {
            m_environment = environment;
            MapTraps = new List<MapTrap>();
            MapElements = new List<MapElement>();
            MapMalus = new List<MapMalus>();
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

        public void devOnlyAddMapElement(MapElement mapElement)
        {
            MapElements.Add(mapElement);
        }

        public void devOnlyAddMapTrap(MapTrap mapTrap)
        {
            MapTraps.Add(mapTrap);
        }

        public void devOnlyAddMapMalus(MapMalus mapMalus)
        {
            MapMalus.Add(mapMalus);
        }
    }
}