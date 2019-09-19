//-----------------------------------------------------------------------
// <copyright file="DetectedPlaneVisualizer.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.Common
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Visualizes a single DetectedPlane in the Unity scene.
    /// </summary>
    public class DetectedPlaneVisualizer : MonoBehaviour
    {
        private DetectedPlane m_DetectedPlane;

        // Keep previous frame's mesh polygon to avoid mesh update every frame.
        private List<Vector3> m_PreviousFrameMeshVertices = new List<Vector3>();
        private List<Vector3> m_MeshVertices = new List<Vector3>();
        private Vector3 m_PlaneCenter = new Vector3();

        private List<Color> m_MeshColors = new List<Color>();

        private List<int> m_MeshIndices = new List<int>();

        private Mesh m_Mesh;

        private MeshRenderer m_MeshRenderer;
        private int mapSize = 1;
        private float size = 1;

        public List<Vector3> MeshVertices { get => m_MeshVertices; set => m_MeshVertices = value; }

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            m_Mesh = GetComponent<MeshFilter>().mesh;
            m_MeshRenderer = GetComponent<UnityEngine.MeshRenderer>();
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {

            if (m_DetectedPlane == null)
            {
                return;
            }
            else if (m_DetectedPlane.SubsumedBy != null)
            {
                Destroy(gameObject);
                return;
            }
            else if (m_DetectedPlane.TrackingState != TrackingState.Tracking)
            {
                 m_MeshRenderer.enabled = false;
                 return;
            }

            m_MeshRenderer.enabled = true;

            _UpdateMeshIfNeeded();
        }

        /// <summary>
        /// Initializes the DetectedPlaneVisualizer with a DetectedPlane.
        /// </summary>
        /// <param name="plane">The plane to vizualize.</param>
        public void Initialize(DetectedPlane plane)
        {
            m_DetectedPlane = plane;
            m_MeshRenderer.material.SetColor("_GridColor", Color.white);
            m_MeshRenderer.material.SetFloat("_UvRotation", Random.Range(0.0f, 360.0f));

            Update();
        }

        /// <summary>
        /// Update mesh with a list of Vector3 and plane's center position.
        /// </summary>
        private void _UpdateMeshIfNeeded()
        {
            m_DetectedPlane.GetBoundaryPolygon(MeshVertices);

            if (_AreVerticesListsEqual(m_PreviousFrameMeshVertices, MeshVertices))
            {
                return;
            }


            m_PreviousFrameMeshVertices.Clear();
            m_PreviousFrameMeshVertices.AddRange(MeshVertices);

            m_PlaneCenter = m_DetectedPlane.CenterPose.position;

            Vector3 planeNormal = m_DetectedPlane.CenterPose.rotation * Vector3.up;

            m_MeshRenderer.material.SetVector("_PlaneNormal", planeNormal);

            int planePolygonCount = MeshVertices.Count;

            Debug.Log("aaaa : " + (MeshVertices[0] - MeshVertices[1]));
            Debug.Log("bbbb : " + (MeshVertices[1] - MeshVertices[2]));

            // The following code converts a polygon to a mesh with two polygons, inner polygon
            // renders with 100% opacity and fade out to outter polygon with opacity 0%, as shown
            // below.  The indices shown in the diagram are used in comments below.
            // _______________     0_______________1
            // |             |      |4___________5|
            // |             |      | |         | |
            // |             | =>   | |         | |
            // |             |      | |         | |
            // |             |      |7-----------6|
            // ---------------     3---------------2
            m_MeshColors.Clear();

            // Fill transparent color to vertices 0 to 3.
            for (int i = 0; i < planePolygonCount; ++i)
            {
                m_MeshColors.Add(Color.clear);
            }

            // Feather distance 0.2 meters.
            const float featherLength = 0.2f;

            // Feather scale over the distance between plane center and vertices.
            const float featherScale = 0.2f;

            // Add vertex 4 to 7.
            for (int i = 0; i < planePolygonCount; ++i)
            {
                Vector3 v = MeshVertices[i];

                // Vector from plane center to current point
                Vector3 d = v - m_PlaneCenter;

                float scale = 1.0f - Mathf.Min(featherLength / d.magnitude, featherScale);
                MeshVertices.Add((scale * d) + m_PlaneCenter);

                m_MeshColors.Add(Color.white);
            }

            m_MeshIndices.Clear();
            int firstOuterVertex = 0;
            int firstInnerVertex = planePolygonCount;

            // Generate triangle (4, 5, 6) and (4, 6, 7).
            for (int i = 0; i < planePolygonCount - 2; ++i)
            {
                m_MeshIndices.Add(firstInnerVertex);
                m_MeshIndices.Add(firstInnerVertex + i + 1);
                m_MeshIndices.Add(firstInnerVertex + i + 2);
            }

            // Generate triangle (0, 1, 4), (4, 1, 5), (5, 1, 2), (5, 2, 6), (6, 2, 3), (6, 3, 7)
            // (7, 3, 0), (7, 0, 4)
            for (int i = 0; i < planePolygonCount; ++i)
            {
                int outerVertex1 = firstOuterVertex + i;
                int outerVertex2 = firstOuterVertex + ((i + 1) % planePolygonCount);
                int innerVertex1 = firstInnerVertex + i;
                int innerVertex2 = firstInnerVertex + ((i + 1) % planePolygonCount);

                m_MeshIndices.Add(outerVertex1);
                m_MeshIndices.Add(outerVertex2);
                m_MeshIndices.Add(innerVertex1);

                m_MeshIndices.Add(innerVertex1);
                m_MeshIndices.Add(outerVertex2);
                m_MeshIndices.Add(innerVertex2);
            }

            m_Mesh.Clear();
            m_Mesh.SetVertices(MeshVertices);
            m_Mesh.SetTriangles(m_MeshIndices, 0);
            m_Mesh.SetColors(m_MeshColors);
        }

        private bool _AreVerticesListsEqual(List<Vector3> firstList, List<Vector3> secondList)
        {
            if (firstList.Count != secondList.Count)
            {
                return false;
            }

            for (int i = 0; i < firstList.Count; i++)
            {
                if (firstList[i] != secondList[i])
                {
                    return false;
                }
            }

            return true;
        }

     

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(m_PlaneCenter, 0.1f);

            //for (float i = 0.1f; i < 10; i+=0.1f)
            //{
            //    Gizmos.color = Color.blue;
            //    Gizmos.DrawSphere(m_PlaneCenter + Vector3.right * i, 0.01f);
            //}

            float minDistance = CustomMath.DistanceToPoint(m_PlaneCenter, m_PreviousFrameMeshVertices[0]);

            foreach (Vector3 point in m_PreviousFrameMeshVertices)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(point, 0.1f);

                float currentDistance = CustomMath.DistanceToPoint(m_PlaneCenter, point);
                if(minDistance > currentDistance)
                {
                    minDistance = currentDistance;
                }

                
            }
            Handles.color = Color.red;
            Handles.DrawWireDisc(m_PlaneCenter // position
                                          , new Vector3(0,1,0)                       // normal
                                          , minDistance);                              // radius

            Gizmos.color = Color.yellow;
            float squreSide = minDistance * 2 / Mathf.Sqrt(2);
            Gizmos.DrawWireCube(m_PlaneCenter, new Vector3(squreSide, 0, squreSide));

            Vector3 leftPoint = m_PlaneCenter - squreSide / 2 * Vector3.left;
            Vector3 rightPoint = m_PlaneCenter - squreSide / 2 * Vector3.right;
            Vector3 forwardPoint = m_PlaneCenter - squreSide / 2 * Vector3.forward;
            Vector3 backPoint = m_PlaneCenter - squreSide / 2 * Vector3.back;

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_PlaneCenter - leftPoint + backPoint, 0.1f);

            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(m_PlaneCenter - leftPoint + forwardPoint, 0.1f);

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(m_PlaneCenter - rightPoint + forwardPoint, 0.1f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(m_PlaneCenter - rightPoint + backPoint, 0.1f);


            Gizmos.DrawSphere(leftPoint - Vector3.right , 0.1f);

            for (float i = rightPoint.x; i < leftPoint.x; i += 0.1f)
            {
                Gizmos.DrawSphere(((i * Vector3.right) + (m_PlaneCenter.y * Vector3.up)), 0.01f);
            }

        }

        public Vector3 GetNearestPointOnGrid(Vector3 position)
        {
            position -= transform.position;

            float xCount = Mathf.RoundToInt(position.x / size);
            float yCount = Mathf.RoundToInt(position.y / size);
            float zCount = Mathf.RoundToInt(position.z / size);

            Vector3 result = new Vector3(
                (float)xCount * size - 0.5f,
                (float)yCount * size + 0.5f,
                (float)zCount * size - 0.5f);

            result += transform.position;

            return result;
        }
    }
}
