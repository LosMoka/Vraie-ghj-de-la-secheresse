using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModelToView;

namespace Model
{

    public class ElementsStore : MonoBehaviour
    {
        public string NameText;
        private Environment m_environment;
        private EnvironmentStore m_environmentStore;

        // Start is called before the first frame update
        void Start()
        {
            m_environment = new Environment(100000);
            m_environmentStore = new EnvironmentStore(m_environment);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void onClickPlus()
        {
            //bool isBuyable = m_environmentStore.buyEnvironmentPerk(gameObject);
            NameText = gameObject.name;
            Debug.LogError("onClickPlus ES" + " " + gameObject.name);
        }

        public void onClickMoins()
        {
            Debug.LogError("onClickMoins ES");
        }
    }

}
