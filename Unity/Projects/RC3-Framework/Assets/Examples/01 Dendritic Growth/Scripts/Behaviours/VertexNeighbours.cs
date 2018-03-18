//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace RC3.Unity.Examples.DendriticGrowth
//{
//    public class VertexNeighbours : RC3.Unity.VertexObject, ISelectionHandler
//    {


//        #region Explicit interface implementations

//        /// <summary>
//        /// 
//        /// </summary>
//        bool ISelectionHandler.IsSelected
//        {
//            get { return _status == VertexStatus.Source; }
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        void ISelectionHandler.OnDeselected()
//        {
//            _sources.Indices.Remove(Vertex);
//            Status = VertexStatus.Default;
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        void ISelectionHandler.OnSelected()
//        {
//            _sources.Indices.Add(Vertex);
//            Status = VertexStatus.Source;
//        }

//        #endregion
//    }

//}

