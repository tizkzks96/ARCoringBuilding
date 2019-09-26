using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridNode
{
    public enum Direction
    {
        LEFT = 0,
        RIGHT = 1,
        TOP = 2,
        BOTTOM = 3,
    }

    public class GridNode : MonoBehaviour
    {

        private GameObject data;
        private Vector2 position;
        private GridNode leftNode;
        private GridNode rightNode;
        private GridNode topNode;
        private GridNode bottonNode;
        Direction m_direction;

        public Vector2 Position { get => position; set => position = value; }
        public GridNode LeftNode { get => leftNode; set => leftNode = value; }
        public GridNode RightNode { get => rightNode; set => rightNode = value; }
        public GridNode TopNode { get => topNode; set => topNode = value; }
        public GridNode BottonNode { get => bottonNode; set => bottonNode = value; }
        public GameObject Data { get => data; set => data = value; }

        public GridNode(Vector2 position, GameObject data = null, GridNode leftNode = null, GridNode rightNode = null, GridNode topNode = null, GridNode bottonNode = null)
        {
            Position = position;
            LeftNode = leftNode;
            RightNode = rightNode;
            TopNode = topNode;
            BottonNode = bottonNode;
            Data = data;
        }



    }

    public class Grid : GridNode
    {
        public Grid(Vector2 position, GameObject data = null, GridNode leftNode = null, GridNode rightNode = null, GridNode topNode = null, GridNode bottonNode = null) :
            base(position, data, leftNode, rightNode, topNode, bottonNode)
        {
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Insert(GridNode n, Direction direction, GameObject new_data)
        {
            switch (direction)
            {
                case Direction.LEFT:
                    if (n.LeftNode == null)
                    {
                        GridNode newNode = new GridNode(new Vector2(n.Position.x - 1, n.Position.y), new_data, null, n);
                    }
                    else return false;
                    break;
                case Direction.RIGHT:
                    if (n.RightNode == null)
                    {
                        GridNode newNode = new GridNode(new Vector2(n.Position.x + 1, n.Position.y), new_data, n);
                    }
                    else return false;
                    break;
                case Direction.TOP:
                    if (n.TopNode == null)
                    {
                        GridNode newNode = new GridNode(new Vector2(n.Position.x, n.Position.y + 1), new_data, null, null, null, n);
                    }
                    else return false;
                    break;
                case Direction.BOTTOM:
                    if (n.BottonNode == null)
                    {
                        GridNode newNode = new GridNode(new Vector2(n.Position.x, n.Position.y - 1), new_data, null, null, n, null);
                    }
                    else return false;
                    break;
                default:
                    return false;
            }
            return true;
        }

        public bool Insert(Vector2 position, Direction direction, GameObject new_data)
        {
            while (true)
            {
                
                break;
            }
            return true;
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
