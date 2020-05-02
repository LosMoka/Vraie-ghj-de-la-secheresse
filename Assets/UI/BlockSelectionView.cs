using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using ModelToView;

public class BlockSelectionView : MonoBehaviour
{
    public Transform TrapContent, ElementContent;
    public Button TrapButtonExample, ElementButtonExample;

    public List<MapTrap> m_map_traps_left;
    public List<MapElement> m_map_elements;

    private Dictionary<GameObject, MapElement> mapElementDic;
    private Dictionary<GameObject, MapTrap> mapTrapDic;

    public void newTrap(MapTrap infos)
    {
        Button newButton = Instantiate(TrapButtonExample, TrapContent, true);
        newButton.gameObject.SetActive(true);
        mapTrapDic[newButton.gameObject] = infos;
        //ajouter aussi le style
    }
    public void newElement(MapElement infos)
    {
        Button newButton = Instantiate(ElementButtonExample, ElementContent, true);
        newButton.gameObject.SetActive(true);
        mapElementDic[newButton.gameObject] = infos;
        //ajouter aussi le style
    }

    // Start is called before the first frame update
    void Start()
    {
        mapElementDic = new Dictionary<GameObject, MapElement>();
        mapTrapDic = new Dictionary<GameObject, MapTrap>();

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        m_map_traps_left = gameManager.environmentInstance.MapTrapsLeft;
        m_map_elements = gameManager.environmentInstance.Maplements;

        foreach(MapTrap i in m_map_traps_left)
        {
            newTrap(i);
        }
        foreach(MapElement i in m_map_elements)
        {
            newElement(i);
        }
    }

    public void OnClickTrap(GameObject i)
    {
        MapTrap jsp = mapTrapDic[i];
    }
    public void OnClickElement(GameObject i)
    {
        MapElement jsp = mapElementDic[i];
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
