using System.Collections.Generic;
using Model;
using ModelToView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace EnvLevelEditor
{
    public class EnvLevelEditorView : MonoBehaviour
    {
        public Button buttonPrefab;
        public MapAssetsManager mapAssetsManager;
        private Tile selectedTile;
        private Dictionary<Button, MapElement> m_button_to_map_element;
        private Dictionary<Button, MapTrap> m_button_to_map_trap;
        public Grid grid;
        private MapManager m_map_manager;
        private Model.Environment m_environment;
        private MapElement m_selected_map_element;
        private MapTrap m_selected_map_trap;
        public MapLoader mapLoader;

        // Start is called before the first frame update
        void Start()
        {
        }
        
        public void build()
        {
            GameObject gameManagerGameObject = GameObject.Find("GameManager");
            if (gameManagerGameObject == null)
            {
                m_map_manager = new MapManager();
                //TODO <a remove>
                m_environment = new Environment(999999);
                m_environment.devOnlyAddMapElement(mapAssetsManager.getMapElementByName("GrassPrefab"));
                m_environment.devOnlyAddMapElement(mapAssetsManager.getMapElementByName("RockPrefab"));
                //TODO </a remove>
            }
            else
            {
                GameManager gameManager = gameManagerGameObject.GetComponent<GameManager>();
                m_map_manager = gameManager.MapManager;
                m_environment = gameManager.environmentInstance;
            }
            
            m_button_to_map_element = new Dictionary<Button, MapElement>();
            m_button_to_map_trap = new Dictionary<Button, MapTrap>();
            foreach (var mapElement in m_environment.MapElements)
            {
                Sprite sprite = mapAssetsManager.getMapElementViewPrefab(mapElement).GetComponent<SpriteRenderer>().sprite;
                Button newButton = Instantiate(buttonPrefab, buttonPrefab.transform.parent);
                newButton.image.sprite = sprite;
                newButton.GetComponent<RectTransform>().sizeDelta = sprite.rect.size;
                m_button_to_map_element[newButton] = mapElement;
            }
            buttonPrefab.gameObject.SetActive(false);
        }

        public void OnClick(Button button)
        {
            if (m_button_to_map_element.ContainsKey(button))
            {
                MapElementView mapElementView = mapAssetsManager
                    .getMapElementViewPrefab(m_button_to_map_element[button]).GetComponent<MapElementView>();

                m_selected_map_element = mapElementView.mapElement;
                m_selected_map_trap = null;
            }else if (m_button_to_map_trap.ContainsKey(button))
            {
                MapTrapView mapTrapView = mapAssetsManager
                    .getMapTrapViewPrefab(m_button_to_map_trap[button]).GetComponent<MapTrapView>();

                m_selected_map_trap = mapTrapView.mapTrap;
                m_selected_map_element = null;
            }
        }

        
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && (m_selected_map_trap != null || m_selected_map_element!=null))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // get the collision point of the ray with the z = 0 plane
                Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
                Vector3Int position = grid.WorldToCell(worldPoint);
                //tilemap.SetTile(position, selectedTile);
                if(m_selected_map_element!=null)
                    m_map_manager.setMapElement(position,m_selected_map_element);
                if(m_selected_map_trap!=null)
                    m_map_manager.setMapTrap(position,m_selected_map_trap);
                mapLoader.loadMap(m_map_manager);
            }
        }
        
    }
}
