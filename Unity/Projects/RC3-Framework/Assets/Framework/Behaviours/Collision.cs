using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private void OnCollisionEnter(UnityEngine.Collision col)
    {
       if(col.gameObject.name == "Target1")
             Destroy(col.gameObject);
    }
}
