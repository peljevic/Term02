using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using SpatialSlur.SlurData;

namespace RC3.Unity.Examples.DendriticGrowth
{
    public class TilesOurs : MonoBehaviour
    {
        [SerializeField] private SharedSelection _sources;
        [SerializeField] private SharedGraph _grid; //Noura
        [SerializeField] private Transform[] _targets;
        [SerializeField] private int _countX;
        [SerializeField] private int _countY;
        [SerializeField] private int _countZ;
        [SerializeField] private int _random3;
        [SerializeField] private int _random4;
        [SerializeField] private int _random5;
        [SerializeField] private int _elementAt;
        private List<Rigidbody> _rigidVertices; //Noura


        private Graph _graph;
        private List<VertexObject> _vertices;


        private List<VertexObject> _fixed;
        private PriorityQueue<float, int> _queue;
        private Queue<int> _queuE;
        private bool _pause;
        private int sendTo;

        private int _counter = 1;

        void Start()
        {
            _graph = _grid.Graph; //Noura
            _vertices = _grid.VertexObjects; //Noura
            _queue = new PriorityQueue<float, int>();
            _queuE = new Queue<int>();
         
        }
        
        /// <summary>
        /// For Noura, storing rigidbodies
        /// </summary>
        void RigidbodyList ()
        {
            foreach(var v in _vertices)
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
                UpdateGrowth();  //_pause = !_pause;

            if (Input.GetKeyDown(KeyCode.T))
                Tiling2D();

            if (!_pause)
              

            #region DebugKeys
            if (Input.GetKeyDown(KeyCode.F))
                SetFirst();
            if (Input.GetKeyDown(KeyCode.Q))
                SetRandom();
            if (Input.GetKeyDown(KeyCode.Keypad3))
            { Set3(); }

            if (Input.GetKeyDown(KeyCode.Keypad4))
                Set4();
            if (Input.GetKeyDown(KeyCode.Keypad8))
                Set8();
            #endregion DebugKeys
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
                //if (_vertices[vi].Visited == true)
                if ( v.Status != VertexStatus.Default)
                    count++;
            }
            return count;
        }

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
               // var index =  ReturnNeighbour(vertex, nCount);
                var vobj = _vertices[vi];
                var vIndex = _vertices[vertex].Index;
                //Debug.Log(vi);
                vobj.Index = ReturnNeighbour(vIndex, nCount);
                //Debug.Log("index is " + vobj.Index);
                vobj.Visited = true;
                _queue.Insert(GetKey(vi), vi);
                if (nCount == 6) nCount = 0;
            }
        }

        private int ReturnNeighbour(int vertex, int neighbour)
        {
            int index = 0;

            if (neighbour == 0) index = GetLeftNeighbour(vertex);
            if (neighbour == 1) index = GetDownNeighbour(vertex);
            if (neighbour == 2) index = GetBackNeighbour(vertex);
            if (neighbour == 3) index = GetRightNeighbour(vertex);
            if (neighbour == 4) index = GetTopNeighbour(vertex);
            if (neighbour == 5) index = GetFrontNeighbour(vertex);
         
            return index;
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
                if (v.Visited == false)
                    v.Visited = true;
            }

            // enqueue sources
            foreach (int v in _sources.Indices)
            {
                // for (int t = 0; t < _targets.Length; t++)

                if ((_vertices[v].transform.position - _targets[0].transform.position).magnitude <=3)
                {
                    _targets[0].transform.position = new Vector3(UnityEngine.Random.Range(12f, -12f),
                                                                UnityEngine.Random.Range(12f, -12f),
                                                                UnityEngine.Random.Range(12f, -12f));
                }

                _queue.Insert(GetKey(v), v);

            }
        }
        #endregion DirectedGrowthManager


        #region testButtons
        private void SetFirst()
        {
            int number = Random.Range(1, 9);
            _vertices[0].Status = (VertexStatus)number;
            SetTilesInAdvance(0, number);
        }
        
        private void SetRandom()
        {
            int number = Random.Range(1, 9);
            _vertices[22].Status = (VertexStatus)number;
           // _filter.sharedMesh = _meshes[number];
            SetTilesInAdvance(22, number);
        }

        private void Set3()
        {
            _vertices[_random3].Index = 3;
            _vertices[_random3].Visited = true;
            SetTilesInAdvance(_random3, 3);
            foreach (var v in _graph.GetVertexNeighbors(_random3))
            { _queuE.Enqueue(v); }
            Debug.Log(_queuE.Count);
            var first = _queuE.ElementAt(_elementAt);           
                //_graph.GetVertexNeighbor(_random3, _elementAt);
            Debug.Log(first);
            _vertices[first].Index = Random.Range(6, 8);// After3(_random3);
            _vertices[first].Visited = true;
        }

        private void Set4()
        {
            _vertices[_random4].Index = 4;
            _vertices[_random4].Visited = false;
            SetTilesInAdvance(_random4,4);
        }

        private void Set8()
        {
            _vertices[_random5].Index = 8;
            _vertices[_random5].Visited = true;
            SetTilesInAdvance(_random5, 8);
        }
        #endregion testButtons

        private void ResetVisited()
        {
            foreach(var vertex in _vertices)
            {
                vertex.Visited = false;
            }
        }

        private void Tiling2D()
        {
            for (int i = _countX*_countY; i < _countX * _countY *( _countZ-1); i++)
            {
                var v = _vertices[i - 1];
                var vi = _vertices[i];
                if (vi.Visited == false)
                {
                    if (i < _countX)
                    {
                        vi.Index = GetRightNeighbour(v.Index);
                     //   vi.Visited = true;
                    }

                    if (i == _countX)
                    {
                        vi.Index = GetFrontNeighbour(v.Index);
                      //  vi.Visited = true;
                    }


                    if (i > _countX && i < _countX * _countY*(_countZ-1))
                    {
                        var v0 = _vertices[i - _countX];
                        vi.Index = GetIntersectNeighbours(v0.Index, v.Index);
                      //  vi.Visited = true;
                    }

                    if (i == _countX * _countZ)
                    {

                    }

               vi.Visited = true;
                    //i= 
                    SetTilesInAdvance(i, vi.Index);
                 //   Debug.Log(i);
                   
                }

            }

        }

        private void After8(int vertex)
        {
            sendTo = vertex + _countX * _countY + 1;
            var v = _vertices[sendTo];
            v.Index = 5;
           // v.Visited = true;
        }

        private int After3(int vertex)
        {
            var newStatus = (int)Random.Range(6, 8);

            if (vertex % _countX != 0)
            {  
                if (newStatus == 7)
                {
                    sendTo = vertex - (_countX * (_countY - 1)) - 1;
                    //  Debug.Log("entered");
                    var v = _vertices[sendTo];
                    v.Index = 7;
                    v.Visited = true;
                }
                else
                {
                    sendTo = vertex - (_countX * (_countY - 1));
                    // Debug.Log("entered else");
                    var v = _vertices[sendTo];
                    v.Index = 6;
                    v.Visited = true;
                }
            }
            return newStatus;
        }

        private void After4(int vertex)
        {
            var newStatus = (int)Random.Range(6, 8);

            if (newStatus == 6)
            {
                sendTo = (vertex - (_countX * (_countY - 1)) + 1);
                // Debug.Log(sendTo);
                var v = _vertices[sendTo];
                v.Index = newStatus;
               v.Visited = true;                
            }
            else
            {  // Debug.Log(4 + " to " + 7);
                sendTo = vertex - (_countX * (_countY - 1));
                var v = _vertices[sendTo];
                v.Index = 7;
               v.Visited = true;
            }
        }

        #region returning arrays

        private int SetTilesInAdvance(int vertex, int status)
        {
            sendTo = vertex;
            
            if (status == 8)// && vertex % _countY != (_countY - 1) && vertex % _countX != 0)
            {
                After8(vertex);
            }
            if (status == 3 && vertex > _countX)
            {
                After3(vertex);
            }

            if (status == 4)
            {
                After4(vertex);
            }
            return sendTo;
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

        private int[] GetFrontOptions(int vertex)
        {
            var validChoices = new int[2];

            if (vertex == 0) validChoices = new int[9] { 0, 1,2, 3, 4, 5,6,7,8 };

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

            if (vertex == 1) validChoices = new int[3] { 0, 2,  5 };

            if (vertex == 2) validChoices = new int[5] { 0, 1, 4, 6, 8 };

            if (vertex == 3) validChoices = new int[5] { 0, 0, 5, 5, 0 }; //2, 3, 5, 8 };

            if (vertex == 4) validChoices = new int[4] { 0, 0, 0, 5 }; //3, 5, 7 };

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

            if (vertex == 1) validChoices = new int[4] { 0, 2, 5,7 };

            if (vertex == 2) validChoices = new int[6] { 0, 1, 3,8,6,4};

            if (vertex == 3) validChoices = new int[4] { 0, 0, 6,  0 }; //2, 3, 5, 8 };

            if (vertex == 4) validChoices = new int[4] { 0, 0, 5, 7 }; //3, 5, 7 };

            if (vertex == 5) validChoices = new int[1] { 0 };

            if (vertex == 6) validChoices = new int[3] { 0, 3, 4 };

            if (vertex == 7) validChoices = new int[3] { 0, 0,4 };

            if (vertex == 8) validChoices = new int[1] { 0 };


            return validChoices;
        }

        private int[] GetBackOptions(int vertex)
        {
            var validChoices = new int[2];

            if (vertex == 0) validChoices = new int[9] { 0, 1,2, 3, 4, 5,6,7,8 };

            if (vertex == 1) validChoices = new int[3] { 0, 4,1};

            if (vertex == 2) validChoices = new int[3] { 0, 5, 2};

            if (vertex == 3) validChoices = new int[5] { 0, 6, 0, 3, 0 }; //2, 3, 5, 8 };

            if (vertex == 4) validChoices = new int[4] { 0, 0,7, 4 }; //3, 5, 7 };

            if (vertex == 5) validChoices = new int[2] { 0,7 };

            if (vertex == 6) validChoices = new int[3] { 0, 4,1 };

            if (vertex == 7) validChoices = new int[3] { 0, 2,5};

            if (vertex == 8) validChoices = new int[3] { 1, 0,4 };


            return validChoices;
        }

        private int[] GetRightOptions(int vertex)
        {
            var validChoices = new int[2];

            if (vertex == 0) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };//return 2;

            if (vertex == 1) validChoices = new int[3] { 0, 1, 4 };

            if (vertex == 2) validChoices = new int[3] { 0, 2, 5 };

            if (vertex == 3) validChoices = new int[1] { 0 };//validChoices = new int[2] { 3, 5 };

            if (vertex == 4) validChoices = new int[2] { 0 ,3};//validChoices = new int[2] { 4, 6 };

            if (vertex == 5) validChoices =new int [9] { 0, 1,2, 3, 4, 5,6,7,8 }; //new int[1] { 0 };//validChoices = new int[2] { 4, 6 };

            if (vertex == 6) validChoices = new int[5] { 0, 1,6, 4 ,6};

            if (vertex == 7) validChoices = new int[3] { 0, 2, 7 };

            if (vertex == 8) validChoices = new int[4] { 0,2, 7,3 };

            return validChoices;
        }

        private int[] GetLeftOptions(int vertex)
        {
            var validChoices = new int[2];

            if (vertex == 0) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };//return 2;

            if (vertex == 1) validChoices = new int[3] { 0, 2,5 };

            if (vertex == 2) validChoices = new int[6] { 0, 1,4,6,8,8};

            if (vertex == 3) validChoices = new int[4] { 0,4,1,8 };//validChoices = new int[2] { 3, 5 };

            if (vertex == 4) validChoices = new int[3] { 0, 2,7 };//validChoices = new int[2] { 4, 6 };

            if (vertex == 5) validChoices = new int[4] { 0, 1,  6, 8 }; //new int[1] { 0 };//validChoices = new int[2] { 4, 6 };

            if (vertex == 6) validChoices = new int[3] { 0, 7,2};

            if (vertex == 7) validChoices = new int[3] { 0, 2, 7 };

            if (vertex == 8) validChoices = new int[3] { 0, 2, 7 };

            return validChoices;
        }

        #endregion returning arrays

        #region returning randoms



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

        #endregion returning randoms




    }
}
