using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridNode
{
    public class GridNode : MonoBehaviour
    {
        private Vector2? cursor;
        private GridNode leftNode;
        private GridNode rightNode;
        private GridNode topNode;
        private GridNode bottonNode;

        public GridNode(Vector2? cursor = null, GridNode leftNode = null, GridNode rightNode = null, GridNode topNode = null, GridNode bottonNode = null)
        {
            if (cursor != null)
                Cursor = cursor.Value;
            else
                Cursor = new Vector2(0, 0);
            LeftNode = leftNode;
            RightNode = rightNode;
            TopNode = topNode;
            BottonNode = bottonNode;
        }

        public Vector2 Cursor { get => cursor.Value; set => cursor = value; }
        public GridNode LeftNode { get => leftNode; set => leftNode = value; }
        public GridNode RightNode { get => rightNode; set => rightNode = value; }
        public GridNode TopNode { get => topNode; set => topNode = value; }
        public GridNode BottonNode { get => bottonNode; set => bottonNode = value; }

        public void AddNode(GridNode n)
        {
            GridNode gridNode = new GridNode(new Vector2(0,0));
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
