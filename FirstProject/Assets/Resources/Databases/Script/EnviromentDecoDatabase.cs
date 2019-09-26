namespace UnityEngine
{
    public class EnviromentDecoDatabase : ScriptableObject
    {
        #region singleton
        private static EnviromentDecoDatabase m_Instance;
        public static EnviromentDecoDatabase Instance
        {
            get {
                if (m_Instance == null)
                    m_Instance = Resources.Load("Databases/EnviromentDecoDatabase") as EnviromentDecoDatabase;

                return m_Instance;
            }
        }
        #endregion

        public EnviromentDecoInfo[] enviromentDeco;


        public EnviromentDecoInfo Get(int index)
        {
            return (enviromentDeco[index]);
        }

        public EnviromentDecoInfo GetByID(int ID)
        {
            for (int i = 0; i < this.enviromentDeco.Length; i++)
            {
                if (this.enviromentDeco[i].ID == ID)
                    return this.enviromentDeco[i];
            }
            return null;
        }

        public EnviromentDecoInfo GetByName(string name)
        {
            for (int i = 0; i < this.enviromentDeco.Length; i++)
            {
                if (enviromentDeco[i].Name.Equals(name))
                    return this.enviromentDeco[i];
            }
            return null;
        }
    }
}