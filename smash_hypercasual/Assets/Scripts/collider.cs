using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider : MonoBehaviour
{
    public bool b_col;
    // Start is called before the first frame update
    void Start()
    {
        b_col = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
        {
            b_col = true;
            //Debug.Log(b_col);
            // this rigidbody hit the player
        }
    }
}
