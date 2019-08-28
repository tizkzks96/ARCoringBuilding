﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class BuildingDatabase : ScriptableObject
    {
        #region singleton
        private static BuildingDatabase m_Instance;
        public static BuildingDatabase Instance
        {
            get {
                if (m_Instance == null)
                    m_Instance = Resources.Load("Databases/WallDatabase") as BuildingDatabase;

                return m_Instance;
            }
        }
        #endregion

        public BuildingInfo[] buildings;


        public BuildingInfo Get(int index) {
            return (buildings[index]);
        }

        public BuildingInfo GetByID(int ID) {
            for (int i = 0; i < this.buildings.Length; i++) {
                if (this.buildings[i].ID == ID)
                    return this.buildings[i];
            }
            return null;
        }
    }
}
    
