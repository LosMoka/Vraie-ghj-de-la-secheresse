using System;
using Model;
using UnityEngine;

namespace ModelToView
{
    public class MapTrapView : MonoBehaviour
    {
        public MapTrap mapTrap;
        public bool isPrefab;
        public int cost;
        public MapAssetsManager mapAssetsManager;
        private static int m_id_counter = 0;

        public void Awake()
        {
            if (isPrefab)
            {
                isPrefab = false;
                mapTrap = new MapTrap(m_id_counter, cost);
                m_id_counter++;
            }
        }
    }
}