using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class PerksView : MonoBehaviour
{
    public PlayerStoreView psv;
    public int index;
    public PlayerStore.PerkClass clas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onlick()
    {
        psv.BuyPerks(this.GetComponent<Button>(), clas, index);
    }
}
