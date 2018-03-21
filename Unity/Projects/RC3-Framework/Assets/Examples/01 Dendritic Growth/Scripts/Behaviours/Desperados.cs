using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RC3.Unity.Examples.DendriticGrowth
{
    public class Desperados : MonoBehaviour
    {
        [SerializeField] private SharedGraph _grid; 
        private Graph _graph;
        private List<VertexObject> _vertices;  
        public GUISkin mySkin;
        private int score = 0;
        private int t1 = 0;
        private int t2 = 0;
        private int t3 = 0;
        private int t4 = 0;
        private int t5 = 0;
        private int t6 = 0;
        private int t7 = 0;
        private int t8 = 0;
        private int[] _tile = new int[8];// { 0, 0, 0, 0, 0, 0, 0, 0 };


        void RigidbodyList()
        {
            foreach (var v in _vertices)
            {
                if (v.RigidVertex.isKinematic == true)
                { score++; _tile[v.Index]++; }
            }
        }

        void CountTiles()
        {
            foreach(var v in _vertices)
            {
                if (v.Index == 1) t1++;
                if (v.Index == 2) t2++;
                if (v.Index == 3) t3++;
                if (v.Index == 4) t4++;
                if (v.Index == 5) t5++;
                if (v.Index == 6) t6++;
                if (v.Index == 7) t7++;
                if (v.Index == 8) t8++;
            }

            score = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8;
        }

        void CountTiles2()
        {
            foreach (var v in _vertices)
            { 
                 _tile[v.Index]++;
            }
        } 

        void Start()
        {
            _graph = _grid.Graph; //Noura
            _vertices = _grid.VertexObjects;
        }



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                //RigidbodyList();
                CountTiles();
            }

             
            // Quit application
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void OnGUI()
        {
            // Set a label for our game
            GUI.skin = mySkin;
            GUI.Label(new Rect(new Vector2(35, 370), new Vector2(350, 100)),"tiles:" + score.ToString());
            GUI.Label(new Rect(new Vector2(Screen.width -165, 140), new Vector2(250, 100)), "type 01:" + t1.ToString());
            GUI.Label(new Rect(new Vector2(Screen.width -165, 197), new Vector2(200, 100)), "type 02:" + t2.ToString());
            GUI.Label(new Rect(new Vector2(Screen.width -165, 258), new Vector2(250, 150)), "type 03:" + t3.ToString());
            GUI.Label(new Rect(new Vector2(Screen.width - 85, 110), new Vector2(200, 150)), "type 04:" + t4.ToString());
            GUI.Label(new Rect(new Vector2(Screen.width -85, 172), new Vector2(250, 150)), "type 05:" + t5.ToString());
            GUI.Label(new Rect(new Vector2(Screen.width - 85, 235), new Vector2(200, 200)), "type 06:" + t6.ToString());
      //      GUI.Label(new Rect(new Vector2(Screen.width -150, 200), new Vector2(250, 200)), "type 07:" + t7.ToString());
      //      GUI.Label(new Rect(new Vector2(Screen.width -250, 200), new Vector2(200, 200)), "type 08:" + t8.ToString());

            // Set the population count 
            /*
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Noura")
            {
                if (GameObject.Find("GameController").GetComponent<CubeGraphCreator>().GetComponent<VertexObject>().Index != null)
                { score = GameObject.Find("GameController").GetComponent<CubeGraphCreator>().GetComponent<VertexObject>().Index; }
            }
            if (scene.name == "CA_2d_history")
              {
                  score = GameObject.FindGameObjectsWithTag("caNode").Length;
              }
              GUI.Label(new Rect(new Vector2(Screen.width - 175, 100), new Vector2(300, 100)), "Population: " + score.ToString());
              if (scene.name == "CA_3d")
              {
                  score = GameObject.Find("caGrid").GetComponent<CA_3D>().GetAlive();
              }
              GUI.Label(new Rect(new Vector2(Screen.width - 175, 100), new Vector2(300, 100)), "Population: " + score.ToString());
      */
        }
    }
}