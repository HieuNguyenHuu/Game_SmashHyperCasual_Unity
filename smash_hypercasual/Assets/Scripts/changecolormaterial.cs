using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class changecolormaterial : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float metalic;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", metalic);
    }
}
