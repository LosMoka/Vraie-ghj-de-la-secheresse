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
        private Environment m_environment;
        private EnvironmentStore m_environmentStore;
        private Dictionary<string, MapElement> m_name_to_id_map_element;
        public MapAssetsManager mapAssetsManager;
        public Button ElementButtonPrefab, TrapButtonPrefab, InventoryButtonPrefab;
        public Dictionary<Button, MapElement> m_button_to_map_element;
        public Dictionary<Button, MapTrap> m_button_to_map_trap;

        void Awake()
        {
            m_name_to_id_map_element = new Dictionary<string, MapElement>();
            m_button_to_map_element = new Dictionary<Button, MapElement>();
            m_button_to_map_trap = new Dictionary<Button, MapTrap>();
        }

        // Start is called before the first frame update
        void Start()
        {
            GameObject gameManagerGameObject = GameObject.Find("GameManager");
            if (gameManagerGameObject == null)
            {
                //TODO <a remove>
                m_environment = new Environment(100000);
                m_environmentStore = new EnvironmentStore(m_environment);
                m_environmentStore.devOnlyAddMapElement(mapAssetsManager.getMapElementByName("TileTerreSprite"));
                m_environmentStore.devOnlyAddMapElement(mapAssetsManager.getMapElementByName("TilePierreSprite"));
                m_environmentStore.devOnlyAddMapElement(mapAssetsManager.getMapElementByName("TileEauSprite"));
                //TODO </a remove>
            }
            else
            {
                GameManager gameManager = gameManagerGameObject.GetComponent<GameManager>();
                m_environmentStore = gameManager.EnvironmentStore;
                m_environment = gameManager.environmentInstance;
            }


            foreach (var mapElement in m_environmentStore.MapElements)
            {
                Sprite sprite = mapAssetsManager.getMapElementViewPrefab(mapElement).GetComponent<SpriteRenderer>().sprite;
                Button newButton = Instantiate(ElementButtonPrefab, ElementButtonPrefab.transform.parent);
                newButton.image.sprite = sprite;
                //newButton.GetComponent<RectTransform>().sizeDelta = sprite.rect.size;
                m_button_to_map_element[newButton] = mapElement;
            }
            
            ElementButtonPrefab.gameObject.SetActive(false);
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void onClickElement(Button button)
        {
            MapElement mapElement = m_button_to_map_element[button];
            bool isBuyable = m_environmentStore.buyEnvironmentPerk(mapElement);
            Debug.Log("onClickPlus ES" + " " + gameObject.name + " ; " + mapElement.Id + " ; " + mapElement+" is buyable:"+isBuyable);
        }

        public void onClickMoins()
        {
            Debug.Log("onClickMoins ES");
        }
    }

}
