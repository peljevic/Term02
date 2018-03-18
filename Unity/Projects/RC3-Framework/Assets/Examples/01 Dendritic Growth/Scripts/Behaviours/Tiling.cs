using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using SpatialSlur.SlurData;

namespace RC3.Unity.Examples.DendriticGrowth
{
    public class Tiling : MonoBehaviour
    {
        [SerializeField] private SharedSelection _sources;
        [SerializeField] private SharedGraph _grid;
        [SerializeField] private Transform[] _targets;
        [SerializeField] private int _countX;
        [SerializeField] private int _countY;
        [SerializeField] private int _countZ;

        private Graph _graph;
        private List<VertexObject> _vertices;
        //private PriorityQueue<float, int> _pqueue;
       // private Queue<int> _queue;
        private bool _pause;

        private int _counter = 1;

        void Start()
        {
            _graph = _grid.Graph;
            _vertices = _grid.VertexObjects;
           // _queue = new PriorityQueue<float, int>();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
                Tiles2D();
        }


        private void Tiles2D() 
        {
            #region ignore
            //for (int k=0; k<_countZ; k++)
            //{
            //    for (int j = 0; j<_countY; j++)
            //    {
            //        for (int i=0; i<_countX; i++)
            //        {
            // if (i == 0&& j==0 && k==0)
            // {
            #endregion ignore

            for (int i = 1; i <_countX*_countY*_countZ; i++)
            {
                var vFirst = _vertices[0];
                vFirst.Status = (VertexStatus)Random.Range(1, 6);
                

                if (i<_countX)
                {
                    var v = _vertices[i - 1];
                    var vi = _vertices[i];
                    vi.Status = RightNeighbour(v.Status);
                }

                if (i==_countX)
                {
                    var vi = _vertices[i];
                    vi.Status = TopNeighbour(vFirst.Status);
                }


                if (i > _countX && i<_countX*_countZ)
                {
                    var v0 = _vertices[i - _countX];
                    var v = _vertices[i - 1];
                    var vi = _vertices[i];

                    vi.Status = TopNeighbours(v0.Status, v.Status);
                }

                if(i== _countX * _countZ)
                {

                }



            }
           
        }



    private int TopRandom(int down, int previous)
        {
            var dV = TopOptions(down);
            var rV = RightOptions(previous);
            int intersect = 0;
            IEnumerable<int> both = dV.Intersect(rV);

            foreach (int id in both)
            {
                intersect = id;
                Debug.Log(id);
            }
            
            return intersect;
        }

    private VertexStatus TopNeighbours(VertexStatus down, VertexStatus previous)
        {
            var index0 = (int)down;
            var index1 = (int)previous;

            return (VertexStatus)TopRandom(index0, index1);
        }

        #region returning arrays
        private int[] TopOptions(int down)
        {
            var validChoices = new int[2];

            if (down == 0) validChoices = new int[2] { 0, 4 };

            if (down == 1) validChoices = new int[2] { 1, 5 };

            if (down == 2) validChoices = new int[2] { 2, 6 };

            if (down == 3) validChoices = new int[2] { 4, 0 };

            if (down == 4) validChoices = new int[2] { 3, 0 };

            if (down == 5) validChoices = new int[2] { 2, 6 };

            if (down == 6) validChoices = new int[2] { 1, 5 };

            return validChoices;
        }

        private int[] RightOptions(int previous)
        {
            var validChoices = new int[2];

            if (previous == 0) validChoices = new int[2] { 2, 2 };//return 2;

            if (previous == 1) validChoices = new int[2] { 0, 2 };

            if (previous == 2) validChoices = new int[2] { 1, 1 };//return 1;

            if (previous == 3) validChoices = new int[2] { 3, 5 };

            if (previous == 4) validChoices = new int[2] { 4, 6 };

            if (previous == 5) validChoices = new int[2] { 4, 6 };

            if (previous == 6) validChoices = new int[2] { 3, 5 };

            return validChoices;
        }

        #endregion returning arrays

        #region returning randoms
    private int TopOption(int down)
    {
        var validChoices = new int[2];

        if (down == 0) validChoices = new int[2] { 0, 4 };

        if (down == 1) validChoices = new int[2] {1,5 };

        if (down == 2) validChoices = new int[2] { 2, 6 };

        if (down == 3) validChoices = new int[2] { 4, 0 };

        if (down == 4) validChoices = new int[2] { 3, 0};
                
        if (down == 5) validChoices = new int[2] { 2, 6 };

        if (down == 6) validChoices = new int[2] { 1, 5 };

            return validChoices[Random.Range(0, validChoices.Length)];
    }

        private VertexStatus TopNeighbour(VertexStatus down)
        {
            var index = (int)down;
          
           return (VertexStatus)TopOption(index);
        }
        

        private int RightOption (int previous)
    {
         var validChoices = new int[2];

         if (previous == 0) return 2;//validChoices = new int[2] { 2, 2 };//return 2;

         if (previous == 1) return 2;//validChoices = new int[2] { 0, 2 };
          
         if (previous == 2) return 1;//validChoices = new int[2] { 1, 1 };//return 1;

         if (previous == 3) return 3;//validChoices = new int[2] { 3, 5 };

         if (previous == 4) return 4;//validChoices = new int[2] { 4, 6 };
                  
         if (previous == 5) return 6;//validChoices = new int[2] { 4, 6 };

         if (previous == 6) return 5; //validChoices = new int[2] { 3, 5 };

         return validChoices[Random.Range(0, validChoices.Length)];
    }


    private VertexStatus RightNeighbour(VertexStatus left)
        {
            var index = (int)left;

            return (VertexStatus)RightOption(index);
        }

        #endregion returning randoms


    }
}
