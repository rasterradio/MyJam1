using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    controlPlayer myPlayer;
    public float xSpeed = 0f;
    public float ySpeed = 0f;

    void Awake()
    {
        myPlayer = GameObject.FindObjectOfType<controlPlayer>();
        xSpeed += (myPlayer._velocity.x * Time.deltaTime);
    }

    void Update()
    {
        Vector2 position = transform.position;
        position.x += xSpeed;
        position.y += ySpeed;
        transform.position = position;
        Invoke("DestroyBullet", 0.5f);
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D (Collider2D col) 
	{
        Debug.Log(col.name);
        Destroy(gameObject);
        if (col.tag == "Enemy")
        {
            Debug.Log("HIT ENEMY");
          //do stuff to enemy
        }
    }
}