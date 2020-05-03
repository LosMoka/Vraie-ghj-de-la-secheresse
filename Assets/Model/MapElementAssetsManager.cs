using System;
using System.Collections.Generic;
using EnvLevelEditor;
using Model;
using UnityEngine;

namespace ModelToView
{
    public class MapElementAssetsManager : MonoBehaviour
    {
        private Dictionary<string, MapElement> m_name_to_id_map_element;
        private Dictionary<string, MapTrap> m_name_to_id_map_trap;
        public List<MapElementView> mapElementViewPrefabs;

        void Awake()
        {
            m_name_to_id_map_element = new Dictionary<string, MapElement>();
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            foreach (var mapElementViewPrefab in mapElementViewPrefabs)
            {
                registerMapElementViewPrefab(mapElementViewPrefab);
            }
        }

        public void registerMapElementViewPrefab(MapElementView mapElementView)
        {
            m_name_to_id_map_element[mapElementView.gameObject.name] = mapElementView.mapElement;
            MapManager.registerMapElement(mapElementView.mapElement);
        }

        public MapElement getMapElementByName(string name)
        {
            if (m_name_to_id_map_element.ContainsKey(name))
                return m_name_to_id_map_element[name];
            return null;
        }
    }
}