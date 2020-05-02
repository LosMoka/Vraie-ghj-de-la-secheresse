using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class TrapTriggerView : MonoBehaviour
{
    public Button buttonExample;
    public Transform Content;
    private List<MapTrap> m_mapTraps;

    private Dictionary<GameObject, MapTrap> mapTrapDico;

    private void newTrap(MapTrap i)
    {
        Button newButton = Instantiate(buttonExample, Content, true);
        newButton.gameObject.SetActive(true);
        mapTrapDico[newButton.gameObject] = i;
    }

    private void OnClickTrap(GameObject i)
    {
        MapTrap jsp = mapTrapDico[i];
    }

    // Start is called before the first frame update
    void Start()
    {
        mapTrapDico = new Dictionary<GameObject, MapTrap>();

        //récup les traps
        //m_mapTraps =
        foreach (MapTrap i in m_mapTraps)
        {
            newTrap(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
