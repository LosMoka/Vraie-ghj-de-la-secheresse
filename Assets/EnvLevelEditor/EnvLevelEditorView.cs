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
        public Tilemap tilemap;
        public Button buttonPrefab;
        public MapAssetsManager mapAssetsManager;
        private Tile selectedTile;
        private Dictionary<Button, MapElement> m_button_to_map_element;
        public Grid grid;
        
        // Start is called before the first frame update
        void Start()
        {
        }
        
        public void build(){

        //TODO <a remove>
            Model.Environment env = new Environment(999999);
            env.devOnlyAddMapElement(mapAssetsManager.getMapElementByName("GrassPrefab"));
            env.devOnlyAddMapElement(mapAssetsManager.getMapElementByName("RockPrefab"));
            //TODO </a remove>
            
            m_button_to_map_element = new Dictionary<Button, MapElement>();
            foreach (var mapElement in env.MapElements)
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
            MapElementView mapElementView = mapAssetsManager.getMapElementViewPrefab(m_button_to_map_element[button]).GetComponent<MapElementView>();
            
            selectedTile = new Tile();
            selectedTile.sprite = mapElementView.GetComponent<SpriteRenderer>().sprite;
        }

        
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && selectedTile != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // get the collision point of the ray with the z = 0 plane
                Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
                Vector3Int position = grid.WorldToCell(worldPoint);
                tilemap.SetTile(position, selectedTile);
            }
        }
        
    }
}
