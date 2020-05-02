using System.Collections.Generic;
using ModelToView;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    public class MapLoader : MonoBehaviour
    {
        public Tilemap tilemap;
        public MapAssetsManager mapAssetsManager;

        public void loadMap(MapManager mapManager)
        {
            Dictionary<int, Tile> id_to_map_element_tile = new Dictionary<int, Tile>();
            Dictionary<int, Tile> id_to_map_trap_tile = new Dictionary<int, Tile>();
            
            foreach (var mapElement in mapManager.MapElements)
            {
                Tile tile;
                if (id_to_map_element_tile.ContainsKey(mapElement.Value.Id))
                    tile = id_to_map_element_tile[mapElement.Value.Id];
                else
                {
                    tile = id_to_map_element_tile[mapElement.Value.Id] = new Tile();
                    tile.sprite = mapAssetsManager.getMapElementViewPrefab(mapElement.Value).GetComponent<SpriteRenderer>().sprite;
                }
                Point2i point = new Point2i(mapElement.Key);
                
                Vector3Int vec = new Vector3Int(point.x,point.y,0);
                
                tilemap.SetTile(vec,tile);
            }
            
            foreach (var mapTrap in mapManager.MapTraps)
            {
                Tile tile;
                if (id_to_map_trap_tile.ContainsKey(mapTrap.Value.Id))
                    tile = id_to_map_trap_tile[mapTrap.Value.Id];
                else
                {
                    tile = id_to_map_trap_tile[mapTrap.Value.Id] = new Tile();
                    tile.sprite = mapAssetsManager.getMapTrapViewPrefab(mapTrap.Value).GetComponent<SpriteRenderer>().sprite;
                }
                Point2i point = new Point2i(mapTrap.Key);
                
                Vector3Int vec = new Vector3Int(point.x,point.y,0);
                
                tilemap.SetTile(vec,tile);
            }
        }
    }
}