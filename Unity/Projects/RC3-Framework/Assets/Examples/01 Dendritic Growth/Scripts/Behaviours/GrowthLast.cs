using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using SpatialSlur.SlurData;

namespace RC3.Unity.Examples.DendriticGrowth
{
        public class GrowthLast : MonoBehaviour
    {
        [SerializeField] private SharedSelection _sources;
        [SerializeField] private SharedGraph _grid; //Noura
        [SerializeField] private Transform[] _targets;
        [SerializeField] private int _elementAt;

        private List<Rigidbody> _rigidVertices; //Noura
        private Graph _graph;
        private List<VertexObject> _vertices;
        private PriorityQueue<float, int> _queue;
        private int _debugCount = 0;
        private int _counter = 1;

        private int _s01;
        private int _s02;
        private int _s03;
        private int _s04;
        private int _s05;
        private int _s06;


        void Start()
        {
            _graph = _grid.Graph; //Noura
            _vertices = _grid.VertexObjects; //Noura
            _queue = new PriorityQueue<float, int>();

        }

        /// <summary>
        /// For Noura, storing rigidbodies
        /// </summary>
        void RigidbodyList()
        {
            foreach (var v in _vertices)
            {
                if (v.RigidVertex.isKinematic == true)
                    _rigidVertices.Add(v.RigidVertex);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                ExportPrepare();

            if (Input.GetKeyDown(KeyCode.R))
                ResetVisited();

            if (Input.GetKeyDown(KeyCode.Space))
                ResetGrowth();

            if (Input.GetKeyDown(KeyCode.P))
                UpdateGrowth2();

            if (Input.GetKeyDown(KeyCode.D))
                DebugNeighbours();

        }

        void DebugNeighbours()
        {
            foreach (int v in _sources.Indices)
            {
                var n = _graph.GetVertexNeighbor(v, _debugCount);
                //  GetTile(_vertices[n - 1].Index, _debugCount);
                _vertices[n].Status = VertexStatus.Visited;
                int count = _graph.GetVertexNeighbors(v).Count<int>();
                if (_debugCount < count) _debugCount++;
                else { _debugCount = 0; }
                Debug.Log(_debugCount);
            }
        }

        private void ExportPrepare()
        {
            foreach (var v in _vertices)
            {
                if (v.Visited == false) Destroy(v.gameObject);
                // else _rigidVertices.Add(v.RigidVertex);
            }
        }

        #region DirectedGrowthManager

        private int CountVisitedNeighbours(int vertex)
        {
            int count = 0;

            foreach (var vi in _graph.GetVertexNeighbors(vertex))
            {
                var v = _vertices[vi];
                if (_vertices[vi].Visited == true)   //if (v.Status != VertexStatus.Default)
                    count++;
            }
            return count;
        }




        //Get the right neighbor with status 
        private int GetNeighbour(int vertex)
        {
            var v = _vertices[vertex];
            int status = v.Index;
            List<int> neigh = new List<int> { 0, 2, 5 };

            if (status==0)
            {
                int neigh1 = neigh[Random.Range(0, neigh.Count)];
                var nV = _graph.GetVertexNeighbor(vertex, neigh1);
                if (neigh1 == 0) _vertices[nV].Index = GetLeft();
                if (neigh1 == 2) _vertices[nV].Index = GetDown();
                if (neigh1 == 5) _vertices[nV].Index = GetFront();
                return neigh1;
            }

            if (status == 1)
            {
                var nV = _graph.GetVertexNeighbor(vertex, 5);
                _vertices[nV].Index = GetFront();
                _vertices[nV].Visited = true;
                return 5;
            }

            if (status == 7)
            {
                var nV = _graph.GetVertexNeighbor(vertex, 0);
                _vertices[nV].Index = GetLeft();
                _vertices[nV].Visited = true;
                return 0;
            }
           
            if (status == 3)
            {
                var nV = _graph.GetVertexNeighbor(vertex, 1);
                _vertices[nV].Index = GetDown(); 
                _vertices[nV].Visited = true;
                return 1;
            }
        
            if (status == 9)
            {
                var nV = _graph.GetVertexNeighbor(vertex, 1);
                _vertices[nV].Index = GetDown(); 
                _vertices[nV].Visited = true;
                return 1;
            }

            if (status == 2)
            {
                var nV = _graph.GetVertexNeighbor(vertex, 3);
                _vertices[nV].Index = GetFront();
                _vertices[nV].Visited = true;
                return 5;
            }

            if (status == 4)
            {
                var nV = _graph.GetVertexNeighbor(vertex, 3);
                _vertices[nV].Index = GetFront();
                _vertices[nV].Visited = true;
                return 5;
            }

            if (status == 5)
            {
                var nV = _graph.GetVertexNeighbor(vertex, 3);
                _vertices[nV].Index = GetFront();
                _vertices[nV].Index = GetLeft();
                _vertices[nV].Visited = true;
                return 0;
            }


            if (status == 8)
            {
                var nV = _graph.GetVertexNeighbor(vertex, 3);
                _vertices[nV].Index = GetDown();
                _vertices[nV].Visited = true;
                return 1;
            }

            else return 4;

        }


        private int GetRight(int status)
        {
            List<int> newStatus = new List<int> { 0 };

            if (status == 9) newStatus = new List<int> { 2 };


            return newStatus[Random.Range(0, newStatus.Count)];
        }

        private int GetFront()
        {
           // List<int> newStatus = new List<int>{ 0 };

          //  if(status==1 || status==9) newStatus = new List<int> { 1, 4,5,6 };

            List<int> newStatus = new List<int> { 1, 4, 5, 6 };

                return newStatus[Random.Range(0, newStatus.Count)];
        }

        private int GetDown()
        {
            List<int> newStatus = new List<int> { 1, 7, 5 };

          //  if (status == 3) newStatus = new List<int> { 7,5 };
           // if (status == 9) newStatus = new List<int> { 1,7, 5 };


            return newStatus[Random.Range(0, newStatus.Count)];
        }


        private int GetLeft()
        {
            List<int> newStatus = new List<int> {2,6 };


            return newStatus[Random.Range(0, newStatus.Count)];
        }

        private void UpdateGrowth2()
        {
            if (_queue.Count == 0)
                return;

            float key;
            int vertex;
            _queue.RemoveMin(out key, out vertex);
            int nCount = 0;

            foreach (var vi in _graph.GetVertexNeighbors(vertex))
            {
                var vobj = _vertices[vi];

                var vIndex = _vertices[vertex].Index;
              //  Debug.Log(vi);
                if (vobj.Visited != true)
                {
                    GetNeighbour(vi);
                    vobj.Visited = true;
                    _queue.Insert(GetKey(vi), vi);
                }
               // nCount++;
               // if (nCount == 6) nCount = 0;

            }
        }


        ///TODO
        /// 1 Tiles more considerate about their neighbours
        /// 2 Add counters for gui

        private void UpdateGrowth()
        {
            if (_queue.Count == 0)
                return;

            float key;
            int vertex;
            _queue.RemoveMin(out key, out vertex);
            int nCount = 0;

            foreach (var vi in _graph.GetVertexNeighbors(vertex))
            {
                var vobj = _vertices[vi];

                var vIndex = _vertices[vertex].Index;
                Debug.Log(vi);
                if (vobj.Visited != true)
                {
                    vobj.Index = ReturnNeighbour(vIndex, nCount);
                    //Debug.Log("index is " + vobj.Index);
                    vobj.Visited = true;
                    _queue.Insert(GetKey(vi), vi);
                }
                nCount++;
                if (nCount == 6) nCount = 0;

            }
        }

        private int ReturnNeighbour(int vertex, int neighbour)
        {
            int meshIndex = 0;

            if (neighbour == 0) meshIndex = GetLeftNeighbour(vertex);
            if (neighbour == 1) meshIndex = GetDownNeighbour(vertex);
            if (neighbour == 2) meshIndex = GetBackNeighbour(vertex);
            if (neighbour == 3) meshIndex = GetRightNeighbour(vertex);
            if (neighbour == 4) meshIndex = GetTopNeighbour(vertex);
            if (neighbour == 5) meshIndex = GetFrontNeighbour(vertex);

            return meshIndex;
        }

        private float GetKey(int vertex)
        {
            var p = _vertices[vertex].transform.position;
            float sum = 0.0f;

            foreach (var target in _targets)
            {
                var d = target.position - p;
                sum += 1.0f / d.sqrMagnitude;
            }

            return -sum;
        }


        private void ResetGrowth()
        {
            _queue = new PriorityQueue<float, int>();

            // reset visited vertices
            foreach (var v in _vertices)
            {
                if (v.Visited == true)
                    v.Visited = false;
            }

            // enqueue sources
            foreach (int v in _sources.Indices)
            {
                // for (int t = 0; t < _targets.Length; t++)

                //this should be moved to GetKey if you want the target to move
                if ((_vertices[v].transform.position - _targets[0].transform.position).magnitude <= 3)
                {
                    _targets[0].transform.position = new Vector3(UnityEngine.Random.Range(12f, -12f),
                                                                UnityEngine.Random.Range(12f, -12f),
                                                                UnityEngine.Random.Range(12f, -12f));
                }

                _queue.Insert(GetKey(v), v);

            }
        }
        #endregion DirectedGrowthManager



        private void ResetVisited()
        {
            foreach (var vertex in _vertices)
            {
                vertex.Visited = false;
            }
        }


        private int GetIntersectRandom(int down, int previous)
        {
            var dV = GetFrontOptions(down);
            var rV = GetRightOptions(previous);
            int intersect = 0;

            IEnumerable<int> both = dV.Intersect(rV);

            foreach (int id in both)
            {
                intersect = id;
                // Debug.Log(id);
            }

            return intersect;
        }

        private int GetIntersectNeighbours(int down, int previous)
        {
            var index0 = down;
            var index1 = previous;

            return GetIntersectRandom(index0, index1);
        }
        /// <summary>
        /// All the possible meshes to connect to the previous one
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        #region returning arrays

        private int[] GetFrontOptions(int vertex)
        {
            var validChoices = new int[2];

            if (vertex == 0) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (vertex == 1) validChoices = new int[5] { 0, 2, 3, 5, 8 };

            if (vertex == 2) validChoices = new int[5] { 0, 1, 4, 6, 4 };

            if (vertex == 3) validChoices = new int[5] { 0, 0, 0, 3, 0 }; //2, 3, 5, 8 };

            if (vertex == 4) validChoices = new int[4] { 0, 0, 0, 4 }; //3, 5, 7 };

            if (vertex == 5) validChoices = new int[3] { 0, 2, 6 };

            if (vertex == 6) validChoices = new int[5] { 0, 2, 3, 3, 8 };

            if (vertex == 7) validChoices = new int[5] { 0, 1, 4, 6, 6 };

            if (vertex == 8) validChoices = new int[2] { 0, 0 };


            return validChoices;
        }

        private int[] GetDownOptions(int vertex)
        {
            var validChoices = new int[2];

            if (vertex == 0) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (vertex == 1) validChoices = new int[3] { 0, 2, 5 };

            if (vertex == 2) validChoices = new int[5] { 0, 1, 4, 6, 8 };

            if (vertex == 3) validChoices = new int[5] { 0, 0, 5, 5, 0 };

            if (vertex == 4) validChoices = new int[4] { 0, 0, 0, 5 };

            if (vertex == 5) validChoices = new int[3] { 0, 2, 5 };

            if (vertex == 6) validChoices = new int[3] { 0, 2, 5 };

            if (vertex == 7) validChoices = new int[5] { 0, 1, 4, 6, 8 };

            if (vertex == 8) validChoices = new int[3] { 0, 2, 5 };


            return validChoices;
        }

        private int[] GetTopOptions(int vertex)
        {
            var validChoices = new int[2];

            if (vertex == 0) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (vertex == 1) validChoices = new int[4] { 0, 2, 5, 7 };

            if (vertex == 2) validChoices = new int[6] { 0, 1, 3, 8, 6, 4 };

            if (vertex == 3) validChoices = new int[4] { 0, 0, 6, 0 };

            if (vertex == 4) validChoices = new int[4] { 0, 0, 5, 7 };

            if (vertex == 5) validChoices = new int[1] { 0 };

            if (vertex == 6) validChoices = new int[3] { 0, 3, 4 };

            if (vertex == 7) validChoices = new int[3] { 0, 0, 4 };

            if (vertex == 8) validChoices = new int[1] { 0 };


            return validChoices;
        }

        private int[] GetBackOptions(int vertex)
        {
            var validChoices = new int[2];

            if (vertex == 0) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (vertex == 1) validChoices = new int[3] { 0, 4, 1 };

            if (vertex == 2) validChoices = new int[3] { 0, 5, 2 };

            if (vertex == 3) validChoices = new int[5] { 0, 6, 0, 3, 0 };

            if (vertex == 4) validChoices = new int[4] { 0, 0, 7, 4 };

            if (vertex == 5) validChoices = new int[2] { 0, 7 };

            if (vertex == 6) validChoices = new int[3] { 0, 4, 1 };

            if (vertex == 7) validChoices = new int[3] { 0, 2, 5 };

            if (vertex == 8) validChoices = new int[3] { 1, 0, 4 };


            return validChoices;
        }

        private int[] GetRightOptions(int vertex)
        {
            var validChoices = new int[2];

            if (vertex == 0) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (vertex == 1) validChoices = new int[3] { 0, 1, 4 };

            if (vertex == 2) validChoices = new int[3] { 0, 2, 5 };

            if (vertex == 3) validChoices = new int[1] { 0 };

            if (vertex == 4) validChoices = new int[2] { 0, 3 };

            if (vertex == 5) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (vertex == 6) validChoices = new int[5] { 0, 1, 6, 4, 6 };

            if (vertex == 7) validChoices = new int[3] { 0, 2, 7 };

            if (vertex == 8) validChoices = new int[4] { 0, 2, 7, 3 };

            return validChoices;
        }

        private int[] GetLeftOptions(int vertex)
        {
            var validChoices = new int[2];

            if (vertex == 0) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (vertex == 1) validChoices = new int[3] { 0, 2, 5 };

            if (vertex == 2) validChoices = new int[6] { 0, 1, 4, 6, 8, 8 };

            if (vertex == 3) validChoices = new int[4] { 0, 4, 1, 8 };

            if (vertex == 4) validChoices = new int[3] { 0, 2, 7 };

            if (vertex == 5) validChoices = new int[4] { 0, 1, 6, 8 };

            if (vertex == 6) validChoices = new int[3] { 0, 7, 2 };

            if (vertex == 7) validChoices = new int[3] { 0, 2, 7 };

            if (vertex == 8) validChoices = new int[3] { 0, 2, 7 };

            return validChoices;
        }

        #endregion returning arrays

        /// <summary>
        /// Takes the array and give you one random back
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        #region returning one random mesh

        private int GetLeftNeighbour(int vertex)
        {
            var index = GetLeftOptions(vertex);
            var status = index[Random.Range(0, index.Length)];

            return status;
        }

        private int GetFrontNeighbour(int vertex)
        {
            var index = GetFrontOptions(vertex);
            var status = index[Random.Range(0, index.Length)];

            return status;
        }

        private int GetBackNeighbour(int vertex)
        {
            var index = GetBackOptions(vertex);
            var status = index[Random.Range(0, index.Length)];

            return status;
        }

        private int GetDownNeighbour(int vertex)
        {
            var index = GetDownOptions(vertex);
            var status = index[Random.Range(0, index.Length)];

            return status;
        }

        private int GetTopNeighbour(int vertex)
        {
            var index = GetTopOptions(vertex);
            var status = index[Random.Range(0, index.Length)];

            return status;
        }

        private int GetRightNeighbour(int vertex)
        {
            var index = GetRightOptions(vertex);
            var status = index[Random.Range(0, index.Length)];

            return status;
        }

        #endregion returning one random mesh


    }
}
