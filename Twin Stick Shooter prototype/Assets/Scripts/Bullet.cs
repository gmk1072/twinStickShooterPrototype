using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float shootforce = 0.0f;
    private Rigidbody rigidBody;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position+=(transform.forward * shootforce * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag != "Bullet") && (other.gameObject.tag != "Player")) 
        {
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag =="Enemy")
        {
            other.gameObject.GetComponent<CustomEnemy>().TakeDamage();
        }
    }
}
