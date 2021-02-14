using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class gameplay : MonoBehaviour
{
    GameObject obj_character;
    Transform ts_character;
    Vector3 v3_initcharacter;
    public float f_swdistthreshold , f_swtimethreshold ;
    public float f_force;

    GameObject obj_basea;
    GameObject obj_pointa;
    GameObject textgameover;

    GameObject wall;

    GameObject buttonnext;

    bool b_swleft, b_swright, b_swup, b_swdown;

    Vector2 v2_swbeginp, v2_swendp;
    float f_swbegintime, f_swendtime, f_swtime, f_swdist;
    double d_swangle;

    Transform[] ts_basea;
    Transform[] ts_pointa;

    int i_tspointa;

    Transform bases;
    int stepl = 1, stepr = 1, stepu= 1, stepd = 1;

    Vector3 d;
    int i_animationstate;

    bool die = false;

    bool reload = false;

    bool b_check1 = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObject;
        if (gameObject = GameObject.Find("character"))
            obj_character = gameObject;
        ts_character = obj_character.GetComponent<Transform>();
        v3_initcharacter = ts_character.position;

        if (gameObject = GameObject.Find("basea"))
            obj_basea = gameObject;

        if (gameObject = GameObject.Find("Pointa"))
            obj_pointa = gameObject;

        if (gameObject = GameObject.Find("Textgameover"))
            textgameover = gameObject;

        if (gameObject = GameObject.Find("Quadwall"))
            wall = gameObject;


        if (gameObject = GameObject.Find("Buttonnext"))
            buttonnext = gameObject;

        ts_basea = obj_basea.GetComponentsInChildren<Transform>().Where((val, idx) => idx != 0).ToArray();
        ts_pointa = obj_pointa.GetComponentsInChildren<Transform>().Where((val, idx) => idx != 0).ToArray();

       
        obj_character.GetComponentInChildren<Text>().color = obj_character.GetComponent<MeshRenderer>().material.color;

        i_tspointa = ts_pointa.Length;

        b_check1 = false;

        baseupdate();
    }

    // Update is called once per frame
    void Update()
    {
        swupdate();
        wallanimationupdate();
        pointupdate();
        dieupdate();
        winupdate();
    }
    void winupdate()
	{
        if (obj_character != null)
        {
            if (i_tspointa == 0 && !b_check1)
            {
                buttonnext.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                if (!GameObject.Find("TextStep").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") && GameObject.Find("TextStep").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    GameObject.Find("TextStep").GetComponentInChildren<Text>().enabled = false;
                }
                GameObject.Find("TextStep").GetComponentInChildren<Text>().enabled = false;
                //GameObject.Find("TextStep").GetComponentInChildren<Text>().text = "";
                GameObject.Find("TextGreat").GetComponent<Text>().text = "GREAT !";
                if (!GameObject.Find("TextGreat").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") && GameObject.Find("TextGreat").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    GameObject.Find("TextGreat").GetComponent<Animator>().Play("idle");
                }
                GameObject.Find("TextGreat").GetComponent<Animator>().Play("greattexta");
                
                b_check1 = true;
            }
        }
    }
	void dieupdate()
	{
        if (obj_character != null)
        {
            /*if (i_tspointa == 0)
            {
                buttonnext.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            else */
            if (int.Parse(obj_character.GetComponentInChildren<Text>().text) == 0 && i_tspointa != 0)
            {
                obj_character.GetComponent<Animator>().Play("diecharactera");
                if (!obj_character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") && obj_character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    wall.GetComponent<Animator>().Play("idle");
                    Destroy(obj_character);
                    obj_character.GetComponent<Explosion>().explode();
                    //obj_character.GetComponent<Animator>().Play("idle");
                }
                //text.active = true;
                textgameover.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
	}
    void pointupdate()
	{
        foreach(Transform e in ts_pointa)
        if (e != null)
        {
            if (e.GetComponent<collierpoint>().b_col)
            {
                i_tspointa--;
                Destroy(e.gameObject);
            }
        }
    }
    bool b_check = false;
    void wallanimationupdate() 
    {
        if (obj_character != null && !die)
        {
            if (obj_character.GetComponent<collider>().b_col)
            {
                b_check = false;
                switch (i_animationstate)
                {
                    case 0:
                        wall.GetComponent<Animator>().Play("upwalla");
                        break;
                    case 1:
                        wall.GetComponent<Animator>().Play("leftwalla");
                        break;
                    case 2:
                        wall.GetComponent<Animator>().Play("rightwalla");
                        break;
                    case 3:
                        wall.GetComponent<Animator>().Play("downwalla");
                        break;
                }

                ts_character.position = Vector3.MoveTowards(ts_character.position, d + new Vector3(0, 2, 0), 5);
                //ts_character.position = new Vector3(15, 1, 0);
                baseupdate();
                obj_character.GetComponent<collider>().b_col = false;
            }
            if (!wall.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") && wall.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                wall.GetComponent<Animator>().Play("idle");
            }

            if (!obj_character.GetComponentInChildren<Text>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") && obj_character.GetComponentInChildren<Text>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                obj_character.GetComponentInChildren<Text>().GetComponent<Animator>().Play("idle");

            }
            if (ts_character.position == d + new Vector3(0, 2, 0) && !b_check)
            {
                obj_character.GetComponentInChildren<Text>().GetComponent<Animator>().Play("texta");
                if (int.Parse(obj_character.GetComponentInChildren<Text>().text) > 0) 
                    obj_character.GetComponentInChildren<Text>().text = (int.Parse(obj_character.GetComponentInChildren<Text>().text) - 1).ToString();
                b_check = true;
            }
        }
    }
	void swupdate()
	{
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            f_swbegintime = Time.time;
            v2_swbeginp = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            f_swendtime = Time.time;
            v2_swendp = Input.mousePosition;

            f_swtime = f_swbegintime - f_swendtime;
            f_swdist = (v2_swendp - v2_swbeginp).magnitude;

            d_swangle = Mathf.Atan2(v2_swendp.y - v2_swbeginp.y, v2_swendp.x - v2_swbeginp.x);

            if (f_swtime < f_swtimethreshold && f_swdist > f_swdistthreshold)
            {
                swcontrol();
            }

        }
    }

    void swcontrol()
	{
        if (obj_character != null && !die)
        {
            double d_swangledreege = d_swangle * 180 / Math.PI;
            if (d_swangledreege >= 20 && d_swangledreege <= 70)
            {
                reload = true;
                obj_character.GetComponentInChildren<Text>().GetComponent<Animator>().Play("invtexta");
                foreach (Transform e in ts_basea)
                {
                    if (e.position == bases.position + new Vector3(7.5f * (stepu - 1), 0, 0))
                    {
                        d = e.position;
                        e.GetComponent<BoxCollider>().size = new Vector3(0.1f, 10, 1);
                        e.GetComponent<BoxCollider>().center = new Vector3(0.5f, 0, 0);
                    }
                }
                i_animationstate = 0;
                //Debug.Log(bases.position + new Vector3(7.5f * (stepu-1), 2, 0));
                //ts_character.position = Vector3.MoveTowards(ts_character.position, new Vector3(300,2,0), f_force * Time.deltaTime);
                obj_character.GetComponent<Rigidbody>().AddForce((7.5f * (stepu - 1)) * f_force, 0, 0);
                //obj_character.GetComponent<collider>().b_col = false;
            }
            if (d_swangledreege >= 115 && d_swangledreege <= 160)
            {
                reload = true;
                obj_character.GetComponentInChildren<Text>().GetComponent<Animator>().Play("invtexta");
                foreach (Transform e in ts_basea)
                {
                    if (e.position == bases.position + new Vector3(0, 0, 7.5f * (stepl - 1)))
                    {
                        d = e.position;
                        e.GetComponent<BoxCollider>().size = new Vector3(1, 10, 0.1f);
                        e.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0.5f);
                    }
                }
                i_animationstate = 1;
                obj_character.GetComponent<Rigidbody>().AddForce(0, 0, (7.5f * (stepl - 1)) * f_force);
            }
            if (d_swangledreege >= -70 && d_swangledreege <= -20)
            {
                reload = true;
                obj_character.GetComponentInChildren<Text>().GetComponent<Animator>().Play("invtexta");
                foreach (Transform e in ts_basea)
                {
                    if (e.position == bases.position - new Vector3(0, 0, 7.5f * (stepr - 1)))
                    {
                        d = e.position;
                        e.GetComponent<BoxCollider>().size = new Vector3(1, 10, 0.1f);
                        e.GetComponent<BoxCollider>().center = new Vector3(0, 0, -0.5f);
                    }
                }
                i_animationstate = 2;
                obj_character.GetComponent<Rigidbody>().AddForce(0, 0, -(7.5f * (stepr - 1)) * f_force);
            }
            if (d_swangledreege >= -160 && d_swangledreege <= -115)
            {
                reload = true;
                obj_character.GetComponentInChildren<Text>().GetComponent<Animator>().Play("invtexta");
                foreach (Transform e in ts_basea)
                {
                    if (e.position == bases.position - new Vector3(7.5f * (stepd - 1), 0, 0))
                    {
                        d = e.position;
                        e.GetComponent<BoxCollider>().size = new Vector3(0.1f, 10, 1);
                        e.GetComponent<BoxCollider>().center = new Vector3(-0.5f, 0, 0);
                    }
                }
                i_animationstate = 3;  
                obj_character.GetComponent<Rigidbody>().AddForce(-(7.5f * (stepd - 1)) * f_force, 0, 0);
            }
        }
    }

    void baseupdate()
	{
        foreach (Transform e in ts_basea)
        {
            if (ts_character.position == e.position + new Vector3(0, 2, 0))
            {
                bases = e;
                stepr = 1;
                stepl = 1;
                stepu = 1;
                stepd = 1;
            }
        }
        for (int i = 0; i < ts_basea.Length; i++)
        {
            if (ts_basea[i].position != bases.transform.position)
            {
                //Debug.Log(bases.position.x - (stepd * 7.5) + " " + e.name + " " + e.position.x + " " + e.position.y + " " + e.position.z);
                //Debug.Log(bases.position.x + (stepu * 7.5) + " " + e.position.x + " " + stepu);
                if (((bases.position.z - (stepr * 7.5)) == ts_basea[i].position.z) && (bases.position.x == ts_basea[i].position.x))
                {
                    stepr++;
                    i = 0;
                }
                if ((bases.position.z + (stepl * 7.5) == ts_basea[i].position.z) && (bases.position.x == ts_basea[i].position.x))
                {
                    stepl++;
                    i = 0;
                }
                if (((bases.position.x - (stepd * 7.5)) == ts_basea[i].position.x) && (bases.position.z == ts_basea[i].position.z))
                {
                    stepd++;
                    i = 0;
                }
                if ((bases.position.x + (stepu * 7.5) == ts_basea[i].position.x) && (bases.position.z == ts_basea[i].position.z))
                {
                    stepu++;
                    i = 0;
                }
            }
        }
        //Debug.Log("l "+stepl + " r " + stepr + " u " + stepu + " d " + stepd);
	}

    public void reloadfunction()
    {
        if(reload)
            SceneManager.LoadScene("Level4");
    }
}
