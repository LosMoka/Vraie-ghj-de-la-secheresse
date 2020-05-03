using System.Collections.Generic;
using ModelToView;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
    public class MapLoader : MonoBehaviour
    {
        public Tilemap ElementTilemap;
        public Tilemap TrapTilemap;
        public MapAssetsManager mapAssetsManager;

        public void loadMap(MapManager mapManager)
        {
            Dictionary<int, Tile> id_to_map_element_tile = new Dictionary<int, Tile>();
            Dictionary<int, Tile> id_to_map_trap_tile = new Dictionary<int, Tile>();

            resetTileMap(ElementTilemap);
            resetTileMap(TrapTilemap);

            mapAssetsManager = GameObject.Find("MapAssetManager").GetComponent<MapAssetsManager>();

            foreach (var mapElement in mapManager.MapElements)
            {
                Tile tile;
                if (id_to_map_element_tile.ContainsKey(mapElement.Value.Id))
                    tile = id_to_map_element_tile[mapElement.Value.Id];
                else
                {
                    tile = id_to_map_element_tile[mapElement.Value.Id] = Tile.CreateInstance<Tile>();
                    tile.sprite = mapAssetsManager.getMapElementViewPrefab(mapElement.Value).GetComponent<SpriteRenderer>().sprite;
                }
                Point2i point = new Point2i(mapElement.Key);
                
                Vector3Int vec = new Vector3Int(point.x,point.y,0);

                ElementTilemap.SetTile(vec,tile);
            }
            
            foreach (var mapTrap in mapManager.MapTraps)
            {
                Tile tile;
                if (id_to_map_trap_tile.ContainsKey(mapTrap.Value.Id))
                    tile = id_to_map_trap_tile[mapTrap.Value.Id];
                else
                {
                    tile = id_to_map_trap_tile[mapTrap.Value.Id] = Tile.CreateInstance<Tile>();
                    tile.sprite = mapAssetsManager.getMapTrapViewPrefab(mapTrap.Value).GetComponent<SpriteRenderer>().sprite;
                }
                Point2i point = new Point2i(mapTrap.Key);
                
                Vector3Int vec = new Vector3Int(point.x,point.y,0);

                TrapTilemap.SetTile(vec,tile);
            }
        }

        private void resetTileMap(Tilemap tilemap)
        {
            for (int i = 0; i < tilemap.size.x; i++)
            {
                for (int j = 0; j < tilemap.size.y; j++)
                {
                    int x = tilemap.origin.x + i;
                    int y = tilemap.origin.y + j;
                    
                    tilemap.SetTile(new Vector3Int(x,y,0), null);
                }
            }
        }
    }
}