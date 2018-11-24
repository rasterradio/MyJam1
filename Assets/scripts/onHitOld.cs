using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onHitOld : MonoBehaviour {

	// Use this for initialization
	void Start () {

		
    /*void onHitBy() //projectileHit
    {
		int oldHealth = health;
		int breakpoint = 0;
		while (oldHealth > 20)
		{
			breakpoint = breakpoint + 20;
			oldHealth = oldHealth - 20;
		}

		//health = health - projectileDamage;

        if (health < 0)
        {
            health = 0;
            locked = true;
            if (facingLeft)
            {
                currAnim = hurtLeftAnim;
                dx = 100;
            }
            else
            {
                currAnim = hurtRightAnim;
                dx = -100;
            }
            audio.stop();
            deadsound.play();
            gameState = death();
        }
		else if (health < breakpoint) //stagger
		{
            float invulnTimer = 0.5f;
	        float lockedTimer = 0.5f;
			health = breakpoint;
			locked = true;
			invuln = true;
			if (facingLeft)
            {
                currAnim = hurtLeftAnim;
                dx = 100;
            }
            else
            {
                currAnim = hurtRightAnim;
                dx = -100;
            }
			invulnTimer -= Time.deltaTime;
			invuln = false;
			lockedTimer -= Time.deltaTime;
			locked = false;
		}
    }*/

    /*void shoot()
    {
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        //audioData = GetComponent<AudioSource>();
        audioData.Play(0);

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.forward * 6;
        Destroy(bullet, 2.0f);
	}*/
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
