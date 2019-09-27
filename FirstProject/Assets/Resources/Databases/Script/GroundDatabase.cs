namespace UnityEngine
{
    public class GroundDatabase : ScriptableObject
    {
        #region singleton
        private static GroundDatabase m_Instance;
        public static GroundDatabase Instance
        {
            get {
                if (m_Instance == null)
                    m_Instance = Resources.Load("Databases/GroundDatabase") as GroundDatabase;

                return m_Instance;
            }
        }
        #endregion

        public GroundInfo[] ground;


        public GroundInfo Get(int index)
        {
            return (ground[index]);
        }

        public GroundInfo GetByID(int ID)
        {
            for (int i = 0; i < this.ground.Length; i++)
            {
                if (this.ground[i].ID == ID)
                    return this.ground[i];
            }
            return null;
        }

        public GroundInfo GetByName(string name)
        {
            for (int i = 0; i < this.ground.Length; i++)
            {
                if (ground[i].Name.Equals(name))
                    return this.ground[i];
            }
            return null;
        }
    }
}
