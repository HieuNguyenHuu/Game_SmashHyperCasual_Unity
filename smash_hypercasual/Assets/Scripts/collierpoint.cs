using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collierpoint : MonoBehaviour
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
	private void OnTriggerEnter(Collider col)
	{
        if (col.gameObject.name == "character")
        {
            b_col = true;
            gameObject.GetComponent<Explosion>().explode();
            //Debug.Log(b_col);
            // this rigidbody hit the player
        }
    }
	
}
