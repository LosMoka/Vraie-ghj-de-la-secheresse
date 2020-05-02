

using System.Collections.Generic;
using Model;
using UnityEngine;

public class MapManager
{
    public Dictionary<string, MapElement> MapElements { get; }
    public Dictionary<string, MapTrap> MapTraps { get; }

    public MapManager()
    {
        MapElements = new Dictionary<string, MapElement>();
        MapTraps = new Dictionary<string, MapTrap>();
    }

    public void setMapElement(Vector3 vec, MapElement mapElement)
    {
        Point2i point = new Point2i((int)vec.x,(int)vec.y);
        MapElements[point.ToString()] = mapElement;
    }
    public void setMapTrap(Vector3 vec, MapTrap mapTrap)
    {
        Point2i point = new Point2i((int)vec.x,(int)vec.y);
        MapTraps[point.ToString()] = mapTrap;
    }
}
