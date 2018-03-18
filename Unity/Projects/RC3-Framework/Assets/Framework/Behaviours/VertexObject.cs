using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Notes
 */
 
namespace RC3.Unity
{
    /// <summary>
    /// 
    /// </summary>
    public class VertexObject : MonoBehaviour
    {
        [SerializeField] private int _vertex;
        //[SerializeField] private int _left;
        //[SerializeField] private int _right;
        //[SerializeField] private int _back;
        //[SerializeField] private int _front;
        //[SerializeField] private int _up;
        //[SerializeField] private int _down;


       //public

        /// <summary>
        /// Returns the vertex associated with this object.
        /// </summary>
        public int Vertex
        {
            get { return _vertex; }
            set { _vertex = value; }
        }

        //public int VertexBack
        //{
        //    get { return (_vertex - 1); }
        //    set { _back = value - 1; }
        //}

        //public int VertexFront
        //{
        //    get { return (_vertex + 1); }
        //    set { _front = value + 1; }
        //}


    }
}