using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SpatialSlur.SlurData;

namespace RC3.Unity.Examples.DendriticGrowth
{
    public class GrowthNew : MonoBehaviour {

    [SerializeField] private SharedSelection _sources;
    [SerializeField] private SharedGraph _grid;
    [SerializeField] private Transform[] _targets;
    [SerializeField] private int _countX;

    private Graph _graph;
    private List<VertexObject> _vertices;
    private PriorityQueue<float, int> _pqueue;
    private Queue<int> _queue;
    private bool _pause;

    private int _counter = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        private float GetKey(int vertex, int target)
    {
        var p = _vertices[vertex].transform.position;
        var t = _targets[target].transform.position;
        float sum = 0.0f;
        float d = 1;

        if ((p - t).magnitude <= 1)
        {
            t = new Vector3(UnityEngine.Random.Range(12f, -12f), UnityEngine.Random.Range(12f, -12f), UnityEngine.Random.Range(12f, -12f));
        }


        var d0 = (p - t).magnitude;
        d = Mathf.Abs(d0);

        sum += 1.0f / d;
        //foreach (var target in _targets)
        //{
        //    var d = target.position - p;
        //    sum += 1.0f / d.sqrMagnitude;
        //}

        return -sum;
    }


    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        _graph = _grid.Graph;
        _vertices = _grid.VertexObjects;
        _pqueue = new PriorityQueue<float, int>();
        _queue = new Queue<int>();
    }


    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ResetGrowth2();

        if (Input.GetKeyDown(KeyCode.P))
            _pause = !_pause;

        if (Input.GetKeyDown(KeyCode.C))
            ClearSources();

            if (Input.GetKeyDown(KeyCode.A))
                Again();


            if (!_pause)
                UpdateLatest();
                    // UpdateGrowth3();
    }


        private void Again()
        {
            foreach (int v in _sources.Indices)
            {
                var cv = _vertices[GetRight(v)];
                cv.Status = VertexStatus.Source;
            
            }
        }

        /// <summary>
        /// 
        /// </summary>
     private void ResetGrowth2()
     {
            _queue.Clear();

            // reset visited vertices
            foreach (var v in _vertices)
            {
                if (v.Status == VertexStatus.Visited)
                    v.Status = VertexStatus.Default;
            }

            // enqueue sources
            foreach (int v in _sources.Indices)
            {
                // for (int t = 0; t < _targets.Length; t++)

                //if ((_vertices[v].transform.position - _targets[0].transform.position).magnitude <= 1)
                //{
                //    _targets[0].transform.position = new Vector3(UnityEngine.Random.Range(12f, -12f), UnityEngine.Random.Range(12f, -12f), UnityEngine.Random.Range(12f, -12f));
                //}

                _queue.Enqueue(v);

            }
      }
    


        /// <summary>
        /// 
        /// </summary>
        private void ResetGrowth()
    {
        _pqueue = new PriorityQueue<float, int>();

        // reset visited vertices
        foreach (var v in _vertices)
        {
            if (v.Status == VertexStatus.Visited)
                v.Status = VertexStatus.Default;
        }

        // enqueue sources
        foreach (int v in _sources.Indices)
        {
            // for (int t = 0; t < _targets.Length; t++)

            if ((_vertices[v].transform.position - _targets[0].transform.position).magnitude <= 1)
            {
                _targets[0].transform.position = new Vector3(UnityEngine.Random.Range(12f, -12f), 
                    UnityEngine.Random.Range(12f, -12f), UnityEngine.Random.Range(12f, -12f));
            }

            _pqueue.Insert(GetKey(v, 0), v);

        }
    }


        private void UpdateLatest()
        {
            if (_queue.Count == 0)
                return;

            var v0 = _queue.Dequeue();

            foreach (var vi in _graph.GetVertexNeighbors(_queue.Dequeue()))
            {
                var v = _vertices[vi];
                if (v.Status != VertexStatus.Default) continue;

                //if (CountVisitedNeighbours(vi) == 1)
                //{
                var v2 = _vertices[GetRight(v0)];
                if (v2.Status != VertexStatus.Default) continue;
                v2.Status = VertexStatus.Visited;
                    _queue.Enqueue(vi);
                //}
            }

        }

        private int GetRight(int i)
        {
            var vI = _graph.GetVertexNeighbors(i);
            

            var v1 = vI.ElementAt(2);

            return v1;
        }


        private void UpdateGrowth3()
        {
            _counter++;

            if (_queue.Count == 0)
                return;

            var v0 = _queue.Dequeue();
            var cv = _graph.GetVertexNeighbors(v0);
            var v1 = _counter % 2 == 0 ? cv.ElementAt(2) : cv.ElementAt(1);
            var v = _vertices[v1];

            if (v.Status != VertexStatus.Default) return;

            if (CountVisitedNeighbours(v1) ==1 )
            {
                v.Status = VertexStatus.Source;
                _queue.Enqueue(v1);

            }
            //UpdateGrowth3();
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearSources()
    {
        foreach (int v in _sources.Indices)
            _vertices[v].Status = VertexStatus.Default;

        _sources.Indices.Clear();
    }


    /// <summary>
    /// 
    /// </summary>
    private void UpdateGrowth()
    {
        if (_pqueue.Count == 0)
            return;

        float key;
        int vertex;
        _pqueue.RemoveMin(out key, out vertex);

        foreach (var vi in _graph.GetVertexNeighbors(vertex))
        {
            var cv = _graph.GetVertexNeighbors(vi);


            var vobj = _vertices[vi];

            if (CountVisitedNeighbours(vi) < 2)
            {
                vobj.Status = VertexStatus.Visited;
                if ((vobj.transform.position - _targets[0].transform.position).magnitude <= 2)
                {
                    var pos = _targets[0].transform.position;

                    if (pos.x + 5 < _countX)
                    { _targets[0].transform.position = new Vector3(pos.x + 5, pos.y, pos.z - 1); }

                    else
                    { _targets[0].transform.position = new Vector3(pos.x - 7, pos.y, pos.z + 3); }
                    //new Vector3(UnityEngine.Random.Range(0, 20),UnityEngine.Random.Range(0, 20),0);
                    // _queue.Insert(GetKey(vi, 1), vi);
                }

                _pqueue.Insert(GetKey(vi, 0), vi);
            }

            //_queue.Insert(GetKey(vi), vi);
        }
    }



    /// <summary>
    /// Returns the number of visited or source neighbours
    /// </summary>
    private int CountVisitedNeighbours(int vertex)
    {
        int count = 0;

        foreach (var vi in _graph.GetVertexNeighbors(vertex))
        {
            if (_vertices[vi].Status != VertexStatus.Default)
                count++;
        }

        return count;
    }
}
}
