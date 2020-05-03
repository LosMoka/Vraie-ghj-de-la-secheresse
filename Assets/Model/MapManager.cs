

using System;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class MapManager
{
    public Dictionary<string, MapElement> MapElements { get; }
    public Dictionary<string, MapTrap> MapTraps { get; }

    private static Dictionary<int, MapElement> m_id_to_map_elements;
    private static Dictionary<int, MapTrap> m_id_to_map_traps;

    public MapManager()
    {
        MapElements = new Dictionary<string, MapElement>();
        MapTraps = new Dictionary<string, MapTrap>();
    }

    public static void registerMapElement(MapElement mapElement)
    {
        if(m_id_to_map_elements==null)
            m_id_to_map_elements = new Dictionary<int, MapElement>();
        m_id_to_map_elements[mapElement.Id] = mapElement;
    }

    public static void registerMapTrap(MapTrap mapTrap)
    {
        if(m_id_to_map_traps==null)
            m_id_to_map_traps = new Dictionary<int, MapTrap>();
        m_id_to_map_traps[mapTrap.Id] = mapTrap;
    }

    public void setMapElement(Vector3 vec, MapElement mapElement)
    {
        Point2i point = new Point2i((int)vec.x,(int)vec.y);
        MapElements[point.ToString()] = mapElement;
        
        
        Point2i sympoint = new Point2i(-(int)vec.x,(int)vec.y);
        MapElements[sympoint.ToString()] = mapElement;
    }
    public void setMapTrap(Vector3 vec, MapTrap mapTrap)
    {
        Point2i point = new Point2i((int)vec.x,(int)vec.y);
        MapTraps[point.ToString()] = mapTrap;
        
        Point2i sympoint = new Point2i(-(int)vec.x,(int)vec.y);
        MapTraps[sympoint.ToString()] = mapTrap;
    }

    public string saveToString()
    {
        string str= "";

        foreach (var mapElement in MapElements)
        {
            str += mapElement.Key + " " + mapElement.Value.Id+" ";
        }

        str += "\n";
        foreach (var mapTrap in MapTraps)
        {
            str += mapTrap.Key + " " + mapTrap.Value.Id+" ";
        }

        return str;
    }

    public void loadFromString(string str)
    {
        string[] map = str.Split('\n');
        string[] splitedMapElement = map[0].Split(' ');
        string[] splitedMapTrap = map[1].Split(' ');

        MapElements.Clear();
        MapTraps.Clear();
        
        for (int i = 0; 2 * i + 1 < splitedMapElement.Length; i++)
        {
            string pos = splitedMapElement[2 * i];
            int id = Convert.ToInt32(splitedMapElement[2 * i + 1]);
            MapElements[pos] = m_id_to_map_elements[id];
        }
        for (int i = 0; 2 * i + 1 < splitedMapTrap.Length; i++)
        {
            string pos = splitedMapTrap[2 * i];
            int id = Convert.ToInt32(splitedMapTrap[2 * i + 1]);
            MapTraps[pos] = m_id_to_map_traps[id];
        }
    }
}
