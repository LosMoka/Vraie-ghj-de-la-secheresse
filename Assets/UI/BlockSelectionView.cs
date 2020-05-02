using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSelectionView : MonoBehaviour
{
    public Transform TrapContent, BlockContent;
    public Button TrapButtonExample, BlockButtonExample;

    public void newBlock(bool isTrap) //faut voir quels infos vous voulez me balancer
    {
        if (isTrap)
        {
            Button newButton = Instantiate(TrapButtonExample, TrapContent, true);
            newButton.gameObject.SetActive(true);
        }
        else
        {
            Button newButton = Instantiate(BlockButtonExample, BlockContent, true);
            newButton.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
