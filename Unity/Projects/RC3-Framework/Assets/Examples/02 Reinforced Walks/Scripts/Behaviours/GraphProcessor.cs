using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SpatialSlur.SlurCore;

/*
 * Notes
 */

namespace RC3.Unity.Examples.ReinforcedWalks
{
    /// <summary>
    /// Manages the growth process
    /// </summary>
    public partial class GraphProcessor : MonoBehaviour
    {
        [SerializeField] private SharedSelection _sources;
        [SerializeField] private VertexObject _targetPrefab;
        [SerializeField] private Transform _target;
        [SerializeField] private SharedEdgeGraph _sharedGraph;
        [SerializeField] private EdgeWeightMapper _weightMapper;
        [SerializeField] private float _reinforceRate = 0.001f;

        [SerializeField] private int target = Mathf.Abs(5);

        private EdgeGraph _graph;
        private List<VertexObject> _vertices;
        private List<EdgeObject> _edges;


        private int[] _depths;
        private float[] _distances;
        private float[] _weights;
        private float _minWeight = 0.0f;


        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            _graph = _sharedGraph.Graph;
            _vertices = _sharedGraph.VertexObjects;
            _edges = _sharedGraph.EdgeObjects;

            _distances = new float[_graph.VertexCount];
            _weights = new float[_graph.EdgeCount];

           //  target =(int)_target.position.magnitude;

            ResetWeights();
        }


        /// <summary>
        /// Initializes the weight of each edge
        /// </summary>
        private void ResetWeights()
        {
            
            //for(int i = 0; i < _graph.EdgeCount; i++)
            //{
            //    var v1 = _graph.GetStartVertex(i);
            //    var v0 = _graph.GetEndVertex(i);

            //    var p0 = _vertices[v0].transform.localPosition;
            //    var p1 = _vertices[v1].transform.localPosition;

            //    _weights[i] = Vector3.Distance(p0, p1);
            //}
            

            _weights.Set(1.0f);
            UpdateGraphElements();
        }


        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                ClearSources();

            if (Input.GetKeyDown(KeyCode.W))
                ResetWeights();

            if (Input.GetKeyDown(KeyCode.R))
                ReinforceEdges();

            if (Input.GetKeyDown(KeyCode.E))
                Reinforce2Edges();

            //if (Input.GetKeyDown(KeyCode.B))
            //    ReinforceEdgesBetween();

            if (Input.GetKeyDown(KeyCode.M))
                ReinforceEdgesMax();

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
        private void ReinforceEdgesMax()
        {
            GraphUtil.GetVertexDistances(_graph, _weights, _sources.Indices, _distances);

            // descend distance gradient from each vertex
            for (int i = 0; i < _graph.VertexCount; i++)
            {
                // reduce weight of each travelled edge
                foreach (var e in GraphUtil.WalkToMax(_graph, _distances, i))
                    //_weights[e] = 0.1f;
                    _weights[e] = Mathf.Max(_weights[e] - _reinforceRate, _minWeight);
            }

            UpdateGraphElements();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Reinforce2Edges()
        {
            GraphUtil.GetVertexDistances(_graph, _weights, _sources.Indices, _distances);

            // descend distance gradient from each vertex
            for (int i = 1; i < _graph.VertexCount; i++)
            {
                // reduce weight of each travelled edge
                foreach (var e in GraphUtil.WalkToMin(_graph, _distances, i))
                {
                    if (_distances[target] - _distances[i-1] > _distances[target] - _distances[i])
                    {
                       // _distances[0] = _distances[i];
                        _weights[i] = Mathf.Max(_weights[i] - _reinforceRate, _minWeight);
                    }
                  
                }
                //_weights[e] = 0.1f;
              
                  
            }

            UpdateGraphElements();
        }




        /// <summary>
        /// 
        /// </summary>
        private void ReinforceEdges()
        {
            GraphUtil.GetVertexDistances(_graph, _weights, _sources.Indices, _distances);

            // descend distance gradient from each vertex
            for(int i = 0; i < _graph.VertexCount; i++)
            {
                // reduce weight of each travelled edge
                foreach (var e in GraphUtil.WalkToMin(_graph, _distances, i))
                   //_weights[e] = 0.1f;
                         _weights[e] = Mathf.Max(_weights[e] - _reinforceRate, _minWeight);
            }

            UpdateGraphElements();
        }

        private void ReinforceEdgesBetween()
        {
            GraphUtil.GetVertexDistances(_graph, _weights, _sources.Indices, _distances);
            for (int i = 0; i < _graph.VertexCount; i++)
            {
                GraphUtil.WalkToMin(_graph, _distances, i);
            }
            float dMax = -1;
            List<float> max = new List<float>();
            var j = 0;
                //=new float [_graph.EdgeCount];
            // descend distance gradient from each vertex
            for (int i = 0; i < _distances.Length; i++)
            {
                Debug.Log("Ismth");
                if (_distances[i]> dMax)
                {
                    dMax = _distances[i];
                    max[j] = _distances[i];
                  _weights[i] = Mathf.Max(_weights[i] - _reinforceRate, _minWeight);
                    //  _weights[i] = Vector3.Distance(_vertices[10].transform.localPosition, _vertices[i].transform.localPosition);
                    //_vertices[i].
                    Debug.Log("smth");
                    j++;
                }
                //_weights[i] = Mathf.Max(_weights[i] - _reinforceRate, _minWeight);
            }

            Debug.Log(max.Count);

            UpdateGraphElements();
        }





        /// <summary>
        /// Updates graph elements with current edge weights
        /// </summary>
        private void UpdateGraphElements()
        {
            const float vertexScale = 1.5f;

            // set edge scale with current weights
            for (int i = 0; i < _graph.EdgeCount; i++)
                _edges[i].Scale = _weightMapper.ToScale(_weights[i]);

            // set vertex scale to the maximum of incident edge scales
            for (int i = 0; i < _graph.VertexCount; i++)
                _vertices[i].Scale = _graph.GetIncidentEdges(i).Max(e => _edges[e].Scale) * vertexScale;
        }
    }
}
