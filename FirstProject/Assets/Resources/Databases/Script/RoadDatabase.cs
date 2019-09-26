namespace UnityEngine
{
    public class RoadDatabase : ScriptableObject
    {
        #region singleton
        private static RoadDatabase m_Instance;
        public static RoadDatabase Instance
        {
            get {
                if (m_Instance == null)
                    m_Instance = Resources.Load("Databases/RoadDatabase") as RoadDatabase;

                return m_Instance;
            }
        }
        #endregion

        public RoadInfo[] road;


        public RoadInfo Get(int index)
        {
            return (road[index]);
        }

        public RoadInfo GetByID(int ID)
        {
            for (int i = 0; i < this.road.Length; i++)
            {
                if (this.road[i].ID == ID)
                    return this.road[i];
            }
            return null;
        }

        public RoadInfo GetByName(string name)
        {
            for (int i = 0; i < this.road.Length; i++)
            {
                if (road[i].Name.Equals(name))
                    return this.road[i];
            }
            return null;
        }
    }
}
