using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEnemy : MonoBehaviour
{

    public float speed = 5.0f;
    public float maxHealth = 10.0f;
    private Rigidbody rBody;
    private Vector3 direction;
    private float health;
    private GameObject player;
    private float scale;
    private UIScript uiScript; // ref to UI stuff

    // Use this for initialization
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        uiScript = FindObjectOfType<UIScript>();
        direction = Vector3.Normalize(player.gameObject.transform.position - transform.position);
        scale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        rBody.AddForce(direction);
        rBody.velocity = Vector3.ClampMagnitude(rBody.velocity, speed);

        transform.localScale =new Vector3( scale*.6f * (health /maxHealth) + (scale * .4f), scale * .6f * (health / maxHealth) + (scale * .4f), scale*.6f * (health / maxHealth) + (scale* .4f));

        if(health<maxHealth)
        {
            health += (float)(maxHealth/5 * Time.deltaTime);
        }
        else
        {
            health = maxHealth;
        }
        if(health <= 0)
        {
            Die();
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CustomController>().TakeDamage();
        }
        if (player && other.gameObject.tag != "Bullet")
        {
            direction = Vector3.Normalize(player.gameObject.transform.position - transform.position);
        }
    }

    public void TakeDamage()
    {
        health--;
    }

    void Die()
    {
        uiScript.DecrementEnemies();
        Destroy(this.gameObject);
    }
}
