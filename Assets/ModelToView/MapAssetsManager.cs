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
        public EnvLevelEditorView envLevelEditorView;

        void Awake()
        {
            m_id_to_map_element_game_object = new Dictionary<int, GameObject>();
            m_id_to_map_trap_game_object = new Dictionary<int, GameObject>();
            m_name_to_id_map_element = new Dictionary<string, MapElement>();
            m_name_to_id_map_trap = new Dictionary<string, MapTrap>();
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            foreach (var mapElementViewPrefab in mapElementViewPrefabs)
            {
                registerMapElementViewPrefab(mapElementViewPrefab);
            }
            envLevelEditorView.build();
        }

        public void registerMapElementViewPrefab(MapElementView mapElementView)
        {
            m_id_to_map_element_game_object[mapElementView.mapElement.Id] = mapElementView.gameObject;
            m_name_to_id_map_element[mapElementView.gameObject.name] = mapElementView.mapElement;
        }
        public void registerMapTrapViewPrefab(MapTrapView mapTrapView)
        {
            m_id_to_map_trap_game_object[mapTrapView.mapTrap.Id] = mapTrapView.gameObject;
            m_name_to_id_map_trap[mapTrapView.gameObject.name] = mapTrapView.mapTrap;
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