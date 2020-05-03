using System;
using System.Collections.Generic;
using EnvLevelEditor;
using Model;
using UnityEngine;

namespace ModelToView
{
    public class MapAssetsManager : MonoBehaviour
    {
        private Dictionary<int, GameObject> m_id_to_map_element_game_object, m_id_to_map_trap_game_object;
        private Dictionary<string, MapElement> m_name_to_id_map_element;
        private Dictionary<string, MapTrap> m_name_to_id_map_trap;
        public List<MapElementView> mapElementViewPrefabs;
        public List<MapTrapView> mapTrapViewPrefabs;
        public List<MapMalusView> mapMalusViewPrefabs;
        private Dictionary<string, MapMalus> m_name_to_id_map_malus;

        void Awake()
        {
            m_id_to_map_element_game_object = new Dictionary<int, GameObject>();
            m_id_to_map_trap_game_object = new Dictionary<int, GameObject>();
            m_name_to_id_map_element = new Dictionary<string, MapElement>();
            m_name_to_id_map_trap = new Dictionary<string, MapTrap>();
            m_name_to_id_map_malus = new Dictionary<string, MapMalus>();
        }

        private void Start()
        {
            foreach (var mapElementViewPrefab in mapElementViewPrefabs)
            {
                registerMapElementViewPrefab(mapElementViewPrefab);
            }
            foreach(var mapTrapViewPrefab in mapTrapViewPrefabs)
            {
                registerMapTrapViewPrefab(mapTrapViewPrefab);
            }
            foreach(var mapMalusViewPrefab in mapMalusViewPrefabs)
            {
                registerMapMalusViewPrefab(mapMalusViewPrefab);
            }
        }

        private void registerMapMalusViewPrefab(MapMalusView mapMalusView)
        {
            m_id_to_map_trap_game_object[mapMalusView.mapMalus.Id] = mapMalusView.gameObject;
            m_name_to_id_map_malus[mapMalusView.gameObject.name] = mapMalusView.mapMalus;
        }

        public void registerMapElementViewPrefab(MapElementView mapElementView)
        {
            m_id_to_map_element_game_object[mapElementView.mapElement.Id] = mapElementView.gameObject;
            m_name_to_id_map_element[mapElementView.gameObject.name] = mapElementView.mapElement;
            MapManager.registerMapElement(mapElementView.mapElement);
        }
        public void registerMapTrapViewPrefab(MapTrapView mapTrapView)
        {
            m_id_to_map_trap_game_object[mapTrapView.mapTrap.Id] = mapTrapView.gameObject;
            m_name_to_id_map_trap[mapTrapView.gameObject.name] = mapTrapView.mapTrap;
            MapManager.registerMapTrap(mapTrapView.mapTrap);
        }

        public MapElement getMapElementByName(string name)
        {
            if (m_name_to_id_map_element.ContainsKey(name))
                return m_name_to_id_map_element[name];
            return null;
        }
        public MapTrap getMapTrapByName(string name)
        {
            if (m_name_to_id_map_trap.ContainsKey(name))
                return m_name_to_id_map_trap[name];
            return null;
        }

        public GameObject instantiateMapElementView(MapElement mapElement)
        {
            GameObject gameObject = Instantiate(m_id_to_map_element_game_object[mapElement.Id],
                m_id_to_map_element_game_object[mapElement.Id].transform.parent);

            return gameObject;
        }
        public GameObject instantiateMapElementView(MapTrap mapTrap)
        {
            GameObject gameObject = Instantiate(m_id_to_map_trap_game_object[mapTrap.Id],
                m_id_to_map_trap_game_object[mapTrap.Id].transform.parent);

            return gameObject;
        }

        public GameObject getMapElementViewPrefab(MapElement mapElement)
        {
            if (m_id_to_map_element_game_object.ContainsKey(mapElement.Id))
                return m_id_to_map_element_game_object[mapElement.Id];
            return null;
        }

        public GameObject getMapTrapViewPrefab(MapTrap mapTrap)
        {
            if (m_id_to_map_trap_game_object.ContainsKey(mapTrap.Id))
                return m_id_to_map_trap_game_object[mapTrap.Id];
            return null;
        }
    }
}