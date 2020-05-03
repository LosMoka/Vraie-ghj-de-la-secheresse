using Model;
using UnityEngine;

namespace ModelToView
{
    public class MapElementView : MonoBehaviour
    {
        public MapElement mapElement;
        public bool isPrefab;
        public int cost;
        private static int m_id_counter = 0;
        public void Awake()
        {
            if (isPrefab)
            {
                isPrefab = false;
                mapElement = new MapElement(m_id_counter, cost);
                m_id_counter++;
            }
        }
    }
}