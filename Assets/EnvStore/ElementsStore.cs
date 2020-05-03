using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModelToView;
using UnityEngine.SceneManagement;

namespace Model
{

    public class ElementsStore : MonoBehaviour
    {
        private Environment m_environment;
        private EnvironmentStore m_environmentStore;
        private Dictionary<string, MapElement> m_name_to_id_map_element;
        private Dictionary<string, MapTrap> m_name_to_id_map_trap;
        public MapAssetsManager mapAssetsManager;
        public Button ElementButtonPrefab, TrapButtonPrefab, InventoryButtonPrefab;
        public Dictionary<Button, MapElement> m_button_to_map_element;
        public Dictionary<Button, MapTrap> m_button_to_map_trap;

        void Awake()
        {
            m_name_to_id_map_element = new Dictionary<string, MapElement>();
            m_name_to_id_map_trap = new Dictionary<string, MapTrap>();
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
                //TODO </a remove>
            }
            else
            {
                GameManager gameManager = gameManagerGameObject.GetComponent<GameManager>();
                m_environmentStore = gameManager.EnvironmentStore;
                m_environment = gameManager.environmentInstance;
            }

            mapAssetsManager = GameObject.Find("MapAssetManager").GetComponent<MapAssetsManager>();
            m_environmentStore.devOnlyAddMapElement(mapAssetsManager.getMapElementByName("TileTerreSprite"));
            m_environmentStore.devOnlyAddMapElement(mapAssetsManager.getMapElementByName("TilePierreSprite"));
            m_environmentStore.devOnlyAddMapElement(mapAssetsManager.getMapElementByName("TileEauSprite"));
            m_environmentStore.devOnlyAddMapMalus(mapAssetsManager.getMapMalusByName("TimerSprite"));
            m_environmentStore.devOnlyAddMapMalus(mapAssetsManager.getMapMalusByName("GlissadeSprite"));
            m_environmentStore.devOnlyAddMapMalus(mapAssetsManager.getMapMalusByName("NegatifSprite"));
            m_environmentStore.devOnlyAddMapMalus(mapAssetsManager.getMapMalusByName("NoirBlancSprite"));
            m_environmentStore.devOnlyAddMapMalus(mapAssetsManager.getMapMalusByName("SlowSprite"));
            m_environmentStore.devOnlyAddMapMalus(mapAssetsManager.getMapMalusByName("VisionSprite"));
            m_environmentStore.devOnlyAddMapTrap(mapAssetsManager.getMapTrapByName("DestructionSprite"));
            m_environmentStore.devOnlyAddMapTrap(mapAssetsManager.getMapTrapByName("FenceSprite"));
            m_environmentStore.devOnlyAddMapTrap(mapAssetsManager.getMapTrapByName("FireTrapSprite"));
            m_environmentStore.devOnlyAddMapTrap(mapAssetsManager.getMapTrapByName("MobTrapSprite"));
            m_environmentStore.devOnlyAddMapTrap(mapAssetsManager.getMapTrapByName("TrapSprite"));
            m_environmentStore.devOnlyAddMapTrap(mapAssetsManager.getMapTrapByName("WindSprite"));

            foreach (var mapElement in m_environmentStore.MapElements)
            {
                Sprite sprite = mapAssetsManager.getMapElementViewPrefab(mapElement).GetComponent<SpriteRenderer>().sprite;
                Button newButton = Instantiate(ElementButtonPrefab, ElementButtonPrefab.transform.parent);
                newButton.image.sprite = sprite;
                //newButton.GetComponent<RectTransform>().sizeDelta = sprite.rect.size;
                m_button_to_map_element[newButton] = mapElement;
            }

            foreach (var mapTrap in m_environmentStore.MapTraps)
            {
                Sprite sprite = mapAssetsManager.getMapTrapViewPrefab(mapTrap).GetComponent<SpriteRenderer>().sprite;
                Button newButton = Instantiate(TrapButtonPrefab, TrapButtonPrefab.transform.parent);
                newButton.image.sprite = sprite;
                //newButton.GetComponent<RectTransform>().sizeDelta = sprite.rect.size;
                m_button_to_map_trap[newButton] = mapTrap;
            }

            ElementButtonPrefab.gameObject.SetActive(false);
            TrapButtonPrefab.gameObject.SetActive(false);
            InventoryButtonPrefab.gameObject.SetActive(false);
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void onClickElement(Button button)
        {
            MapElement mapElement = m_button_to_map_element[button];
            bool isBuyable = m_environmentStore.buyEnvironmentPerk(mapElement);

            Button b = Instantiate(InventoryButtonPrefab,InventoryButtonPrefab.transform.parent);
            b.image.sprite = button.image.sprite;
            b.gameObject.SetActive(true);
        }

        public void onClickTrap(Button button)
        {
            MapTrap mapTrap = m_button_to_map_trap[button];
            bool isBuyable = m_environmentStore.buyEnvironmentPerk(mapTrap);
            
            Button b = Instantiate(InventoryButtonPrefab,InventoryButtonPrefab.transform.parent);
            b.image.sprite = button.image.sprite;
            b.gameObject.SetActive(true);
        }

        public void onReadyButtonClick()
        {
            SceneManager.LoadScene("EnvLevelEditor");
        }
    }

}
