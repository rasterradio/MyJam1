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
        transform.Rotate(Vector3.forward * -90); //rotation won't matter when bullets are circular
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

    void onTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }
}
