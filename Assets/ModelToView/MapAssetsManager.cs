using System.Collections.Generic;
using UnityEngine;

namespace ModelToView
{
    public class MapAssetsManager : MonoBehaviour
    {
        private Dictionary<int, GameObject> m_id_to_map_element_game_object, m_id_to_map_trap_game_object;

        public void registerMapElementViewPrefab(MapElementView mapElementView)
        {
            m_id_to_map_element_game_object[mapElementView.mapElement.Id] = mapElementView.gameObject;
        }
        public void registerMapElementViewPrefab(MapTrapView mapTrapView)
        {
            m_id_to_map_element_game_object[mapTrapView.mapTrap.Id] = mapTrapView.gameObject;
        }
    }
}