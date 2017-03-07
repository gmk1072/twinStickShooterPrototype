using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float speed = 5.0f;
    public float fireRate = 1.0f;

    public float maxHealth = 1.0f;

    private float health;
    private Rigidbody rigidBody;
    private float radius = 1.0f;
    private bool shoot;
    private Vector3 direction;
    private float timer;
    private float speedInit;
    private UIScript uiScript; // ref to UI stuff

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        uiScript = FindObjectOfType<UIScript>();
        radius = transform.localScale.x;
        health = maxHealth;
        speedInit = speed;
    }

    // Update is called once per frame
    void Update()
    {

        direction = new Vector3();
        Vector3 mousepos = Input.mousePosition;
        mousepos = Camera.main.ScreenToWorldPoint(mousepos);
        mousepos.y = transform.position.y;
        Vector3 shootdirection = Vector3.Normalize(mousepos - transform.position);
        if (Input.GetButton("shoot"))
        {
            shoot = true;
        }
        else
        {
            shoot = false;
        }
        timer += Time.deltaTime;
        if (shoot)
        {
            if (timer >= (1 / fireRate))
            {
                timer = 0;
                Quaternion quat = Quaternion.identity;
                quat.SetLookRotation(shootdirection, new Vector3(0, 1, 0));
                Instantiate(bulletPrefab, ((shootdirection * radius) + transform.position), quat);
            }

            speed = speedInit * .6f;
        }
        else
        {
            speed = speedInit;

        }

        if (Input.GetButton("up"))
        {
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        }
        if (Input.GetButton("right"))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetButton("left"))
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetButton("down"))
        {
            transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
        }


        if(health<=0)
        {
            Die();
        }
    }

    public void TakeDamage()
    {
        health--;
    }

    void Die()
    {
        // Set ui state.
        uiScript.SetState(UIState.DEAD);
        Destroy(this.gameObject);
    }
}
