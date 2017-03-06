using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float speed = 5.0f;

    private Rigidbody rigidBody;
    private float radius = 1.0f;
    public float firerate = 15.0f;
    private bool shoot;
    private Vector3 direction;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        radius = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector3();
        Vector3 mousepos = Input.mousePosition;
        mousepos = Camera.main.ScreenToWorldPoint(mousepos);
        mousepos.y = transform.position.y;
        Vector3 shootdirection = Vector3.Normalize(mousepos - transform.position);
        if(Input.GetButton("shoot"))
        {
            shoot = true;
        }
        else
        {
            shoot = false;
        }
        if (shoot)
        {
            Instantiate(bulletPrefab, ((shootdirection * radius) + transform.position), new Quaternion(shootdirection.x,shootdirection.y,shootdirection.z,Quaternion.identity.w));
        }
        Debug.Log(shoot);
        if(Input.GetButton("up"))
        {
            transform.position += new Vector3(0, 0, speed);
        }
        if (Input.GetButton("right"))
        {
            transform.position += new Vector3(speed, 0, 0);
        }
        if (Input.GetButton("left"))
        {
            transform.position += new Vector3(-speed, 0, 0);
        }
        if (Input.GetButton("down"))
        {
            transform.position += new Vector3(0, 0, -speed);
        }
    }
}
