using System.Collections;
using System.Collections.Generic;
using Model;
using ModelToView;
using Network;
using UnityEngine;

public class EnvPhase1 : MonoBehaviour
{
    private Client m_client;
    public Grid grid;
    private FakeClient m_fake_client;
    private string m_map_as_string;
    private MapManager m_map_manager;
    public MapLoader mapLoader;

    // Start is called before the first frame update
    void Start()
    {
        m_map_as_string = null;
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        if (gameManagerGameObject == null)
        {
            m_fake_client = GameObject.Find("FakeClient").GetComponent<FakeClient>();
            m_map_as_string = m_fake_client.map;
            m_map_manager = new MapManager();
        }
        else
        {
            GameManager gameManager = gameManagerGameObject.GetComponent<GameManager>();
            m_map_manager = gameManager.MapManager;
            m_client = gameManager.Client;
            
            m_client.addNetworkMessageHandler("MAP ", delegate(string data)
            {
                m_map_as_string = data;
            });
        }
        
        mapLoader.mapAssetsManager = GameObject.Find("MapAssetManager").GetComponent<MapAssetsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_map_as_string != null)
        {
            m_map_manager.loadFromString(m_map_as_string);
            mapLoader.loadMap(m_map_manager);
            m_map_as_string = null;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = grid.WorldToCell(worldPoint);
            Point2i pos = new Point2i(position.x, position.y);
            //tilemap.SetTile(position, selectedTile);
            if (m_map_manager.isTrapHere(pos))
            {
                Debug.Log("trap here");
                //sa position est pos
            }
        }
    }
}
