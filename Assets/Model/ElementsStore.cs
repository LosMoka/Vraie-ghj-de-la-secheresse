using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModelToView;

namespace Model
{

    public class ElementsStore : MonoBehaviour
    {
        public string NameText;
        private Environment m_environment;
        private EnvironmentStore m_environmentStore;
        private Dictionary<string, MapElement> m_name_to_id_map_element;
        public List<MapElementView> mapElementViewPrefabs;


        void Awake()
        {
            m_name_to_id_map_element = new Dictionary<string, MapElement>();
        }

        // Start is called before the first frame update
        void Start()
        {
            m_environment = new Environment(100000);
            m_environmentStore = new EnvironmentStore(m_environment);
            foreach (var mapElementViewPrefab in mapElementViewPrefabs)
            {
                registerMapElementViewPrefab(mapElementViewPrefab);
            }
        }

        // Update is called once per frame
        void Update()
        {

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

        public void onClickPlus()
        {
            MapElement mapElement = getMapElementByName(NameText);
            //bool isBuyable = m_environmentStore.buyEnvironmentPerk(gameObject);
            Debug.LogError("onClickPlus ES" + " " + gameObject.name + " ; " + NameText + " ; " + mapElement);
        }

        public void onClickMoins()
        {
            Debug.LogError("onClickMoins ES");
        }
    }

}
