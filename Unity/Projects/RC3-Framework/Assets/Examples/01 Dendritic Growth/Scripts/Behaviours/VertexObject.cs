using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Notes
 */
 
namespace RC3.Unity.Examples.DendriticGrowth
{
    /// <summary>
    /// 
    /// </summary>
    public class VertexObject : RC3.Unity.VertexObject, ISelectionHandler
    {
        [SerializeField] private SharedSelection _sources;
        [SerializeField] private SharedMeshes _meshes;
        [SerializeField] private SharedMaterials _materials;
        [SerializeField] private SharedFloats _scales;
        [SerializeField] private Rigidbody _rigidbody;


        private VertexStatus _status;
        private MeshFilter _filter;
        private MeshRenderer _renderer;
        [SerializeField] private List<VertexObject> _vList;
        [SerializeField] private bool _visited = false;
        private int _index;

        private int _s01;
        private int _s02;
        private int _s03;
        private int _s04;
        private int _s05;
        private int _s06;



        #region RenderVisitedOnly
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnSetIndex();
            }
        }

        public Rigidbody RigidVertex
        {
            get { return _rigidbody; }

        }


        private void OnSetIndex()
        {
            int index = _index;

            if (_index != 0 || _index > 8)
            {
                _rigidbody.isKinematic = true;
               // _visited = true;
                //_vList.Add(this);
            }
           // _filter.sharedMesh = _meshes[index];
        
            if(_index>0 && _index< 9)
            {
                Visited = false;
            }

            Status = (VertexStatus)Index;
            /*
            if (index == 1) _s01++;
            if (index == 2) _s02++;
            if (index == 3) _s03++;
            if (index == 4) _s04++;
            if (index == 5) _s05++;
            if (index == 6) _s06++;
*/
            // if (index == 0) Visited = false;
        }

        public bool Visited
        {
            get { return _visited; }
            set
            {  _visited = value ;
               // _rigidbody.isKinematic = _visited;
                //  OnSetBool();

            }
        }

  

        private void OnSetBool()
        {
            bool isVisited = _visited;
            if(isVisited==true)
            {
                Status = (VertexStatus)Index;
                //_renderer.sharedMaterial = _materials[Index];
                //var t = _scales[Index];
                //transform.localScale = new Vector3(t, t, t);
            }
            else
            {
                Status = VertexStatus.Default;
               // Visited = false;
                //_renderer.sharedMaterial = _materials[0];
                //var t = _scales[0];
                //transform.localScale = new Vector3(t, t, t);

            }

        }
        #endregion RenderVisitedOnly


        #region VertexStatus
        public VertexStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnSetStatus();
            }
        }

        private void OnSetStatus()
        {
            int index = (int)_status;

           _filter.sharedMesh = _meshes[index];
            _renderer.sharedMaterial = _materials[index];

            var t = _scales[index];
            transform.localScale = new Vector3(t, t, t);

           
        }
        #endregion VertexStatus


        private void Start()
        {
            _filter = GetComponent<MeshFilter>();
            _renderer = GetComponent<MeshRenderer>();
            Status = VertexStatus.Default; // default vertex state
            Index = 0;
        }


        #region Explicit interface implementations

        /// <summary>
        /// 
        /// </summary>
        bool ISelectionHandler.IsSelected
        {
            get { return _status == VertexStatus.Source; }
        }


        /// <summary>
        /// 
        /// </summary>
        void ISelectionHandler.OnDeselected()
        {
            _sources.Indices.Remove(Vertex);
            Status = VertexStatus.Default;
        }


        /// <summary>
        /// 
        /// </summary>
        void ISelectionHandler.OnSelected()
        {
            _sources.Indices.Add(Vertex);
            Status = VertexStatus.Source;
        }

        #endregion
    }
}