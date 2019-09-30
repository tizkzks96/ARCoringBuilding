using System;

public enum GroundType
{
    EMPTY = 0,
    BUILDING_SITE = 1,
    ENVIROMENT_DECO = 2,
    ROAD = 3,
    WARTER = 4
}

namespace UnityEngine
{
    [Serializable]
    public class GroundInfo
    {
        public string Name;
        public int ID;
        public GameObject GroundPrefab;
        public GroundType groundType;

    }
}
    
