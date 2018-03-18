using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using SpatialSlur.SlurData;

namespace RC3.Unity.Examples.DendriticGrowth
{
    public class CorrectedTiles : MonoBehaviour
    {

        [SerializeField] private SharedSelection _sources;
        [SerializeField] private SharedGraph _grid;
        [SerializeField] private Transform[] _targets;
        [SerializeField] private int _random;
        [SerializeField] private int _elementAt;


        private int _debugCount=0;
        private Graph _graph;
        private List<VertexObject> _vertices;
        private List<Rigidbody> _rigidVertices;
        private PriorityQueue<float, int> _queue;
        private Queue<int> _queuE;
        private bool _pause;
        private int _counter = 1;

        void Start()
        {
            _graph = _grid.Graph;
            _vertices = _grid.VertexObjects;
            _queue = new PriorityQueue<float, int>();
            _queuE = new Queue<int>();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
                DebugNeighbours();


            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                _vertices[_random].Status = VertexStatus.Tile3;
                Set3(_random);
            }

            if (Input.GetKeyDown(KeyCode.Keypad4))
                _vertices[_random].Status = VertexStatus.Tile4; Set4(_random);
            
            if (Input.GetKeyDown(KeyCode.Keypad8))
                _vertices[_random].Status = VertexStatus.Tile8; Set8(_random);
        }


        void DebugNeighbours()
        {
            foreach (int v in _sources.Indices)
            {
                var n =  _graph.GetVertexNeighbor(v, _debugCount);
              //  GetTile(_vertices[n - 1].Index, _debugCount);
                _vertices[n].Status = VertexStatus.Visited;
                int count = _graph.GetVertexNeighbors(v).Count<int>();
                if (_debugCount < count) _debugCount++;
                else { _debugCount = 0; }
                Debug.Log(_debugCount);
            }
        }




        private void GetNextTile(int index, int neighbour)
        {
            if (index == 3 || index == 4 || index == 8) AdvanceTiles(index, neighbour); // sredi ovo da ima smisla
            else if (neighbour == 3) GetRightNeighbour(index);
            else if (neighbour == 4) GetTopNeighbour(index);
        }

        private void AdvanceTiles(int index, int vertex)
        {
            if (index == 3) { Set3(vertex); }
            if (index == 4) { Set4(vertex); }
            if (index == 8) { Set8(vertex); }
        }

        private void Set3(int vertex)
        {
            var newStatus = (int)Random.Range(6, 8);
             
            {
                var nC = _graph.GetVertexNeighbors(vertex).Count<int>();
                Debug.Log(nC);
                var n0=_graph.GetVertexNeighbor(vertex, 4);
                var n1 = _graph.GetVertexNeighbor(n0, 4);
                if (newStatus == 6)
                {
                    _vertices[n1].Index = 6; 
                    _vertices[n1].Visited = true;

                }                
                else 
                {
                    var n2 = _graph.GetVertexNeighbor(n1,3);
                    _vertices[n1].Index = 7; _vertices[n1].Visited = true;
                }
            }
        }

        private void Set4(int vertex)
        {
            var newStatus = (int)Random.Range(6, 8);

            {
                var n0 = _graph.GetVertexNeighbor(vertex, 5);
                var n1 = _graph.GetVertexNeighbor(n0, 4);
                if (newStatus == 7)
                {
                    _vertices[n1].Index = 7;
                    _vertices[n1].Visited = true;

                }
                else
                {
                    var n2 = _graph.GetVertexNeighbor(n1, 0);
                    _vertices[n2].Index = 6; _vertices[n2].Visited = true;
                }
            }
        }

        private void Set8(int vertex)
        {
            var n0 = _graph.GetVertexNeighbor(vertex, 5);
            var n1 = _graph.GetVertexNeighbor(n0, 3);
            _vertices[n1].Index = 5;
            _vertices[n1].Visited = true;
        }


        // make more strict rules for the tiles
        private int GetFrontRandom(int back, int previous)
        {
            var dV = GetFrontOptions(back);
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

        private int GetFrontNeighbours(int back, int previous)
        {
            var index0 = back;
            var index1 = previous;

            return GetFrontRandom(index0, index1);
        }


        private int[] GetFrontOptions(int back)
        {
            var validChoices = new int[2];

            if (back == 0) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            if (back == 1) validChoices = new int[5] { 0, 2, 3, 5, 8 };

            if (back == 2) validChoices = new int[5] { 0, 1, 4, 6, 4 };

            if (back == 3) validChoices = new int[5] { 0, 0, 0, 3, 0 }; //2, 3, 5, 8 };

            if (back == 4) validChoices = new int[4] { 0, 0, 0, 4 }; //3, 5, 7 };

            if (back == 5) validChoices = new int[3] { 0, 2, 6 };

            if (back == 6) validChoices = new int[5] { 0, 2, 3, 3, 8 };

            if (back == 7) validChoices = new int[5] { 0, 1, 4, 6, 6 };

            if (back == 8) validChoices = new int[2] { 0, 0 };


            return validChoices;
        }

        private int[] GetRightOptions(int previous)
        {
            var validChoices = new int[2];

            if (previous == 0) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };//return 2;

            if (previous == 1) validChoices = new int[3] { 0, 1, 4 };

            if (previous == 2) validChoices = new int[3] { 0, 2, 5 };

            if (previous == 3) validChoices = new int[1] { 0 };//validChoices = new int[2] { 3, 5 };

            if (previous == 4) validChoices = new int[2] { 0, 3 };//validChoices = new int[2] { 4, 6 };

            if (previous == 5) validChoices = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }; //new int[1] { 0 };//validChoices = new int[2] { 4, 6 };

            if (previous == 6) validChoices = new int[5] { 0, 1, 6, 4, 6 };

            if (previous == 7) validChoices = new int[3] { 0, 2, 7 };

            if (previous == 8) validChoices = new int[4] { 0, 2, 7, 3 };

            return validChoices;
        }


        #region returning randoms

        private int GetTopNeighbour(int down)
        {
            var index = GetFrontOptions(down);
            var status = index[Random.Range(0, index.Length)];

            return status;
        }

        private int GetRightNeighbour(int left)
        {
            var index = GetRightOptions(left);
            var status = index[Random.Range(0, index.Length)];

            return status;
        }

        #endregion returning randoms
       
        // grow from one to another place using clear tile directionality

        private void GoRight()
        {
            
        }

        //create empty areas


        //increase density at zones?

    }
}
