using Model;
using UnityEngine;

namespace ModelToView
{
    public class MapMalusView : MonoBehaviour
    {
        public MapMalus mapMalus;
        public bool isPrefab;
        public int cost;
        private static int m_id_counter = 0;

        public void Awake()
        {
            if (isPrefab)
            {
                isPrefab = false;
                mapMalus = new MapMalus(m_id_counter, cost);
                m_id_counter++;
            }
        }
    }
}