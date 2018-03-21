using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RC3.Unity.Examples.DendriticGrowth
{
    public class GUI_Controller : MonoBehaviour
    {


        public GUISkin mySkin;
        public int score = 0;



        private void Update()
        {



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
            GUI.Label(new Rect(new Vector2(50, 100), new Vector2(300, 100)), "The Game of Life!");

            // Set the population count 
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Noura")
            {

                score = GameObject.Find("GameController").GetComponent<CubeGraphCreator>().GetComponent<VertexObject>().Index;
            }
            /* if (scene.name == "CA_2d_history")
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