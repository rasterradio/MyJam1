using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour {
    public float moveSpeed = 150f;
    public float gravity = 600f;
    public float dashCharge = 100f;
	public int health = 120;
	public int maxHealth = 120;

	public bool locked = false;
    public bool active = true;
	public bool grounded = false;
	public bool firing = false;
	bool facingLeft = false;
	bool invuln = false;

    Vector2 aimDirection = new Vector2 (1,0);
    Vector2 velocity = new Vector2 (0,0);

    public double dx = 0f; //player coordinates
    public double dy = 0f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    AudioSource audioData;
    private SpriteRenderer spriteRenderer;

    protected Rigidbody2D rb2d;

    void awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioData = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        Debug.Log("TESTING");
    }

    void update()
    {
        Debug.Log("TEST");
        //if (active && !locked) { updateAimDirection()}
        dashCharge = dashCharge + 40; //dash recharge
        if (dashCharge > 100)
            dashCharge = 100;

        Vector2 desiredDirection = new Vector2(0,0);
        if (!locked && active)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                desiredDirection.x -=1;
                facingLeft = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                desiredDirection.x -=1;
                facingLeft = false;     
                Debug.Log("RIGHT");      
            }
            if (Input.GetKeyDown(KeyCode.Z) && active && !locked)
            {
                firing = true;
                //fire();
            }
            else
                firing = false;
        }

        if (!locked)
        {
            /*float movement = desiredDirection.x * (moveSpeed * Time.deltaTime);
            move(movement);
            float velocity = desiredDirection.x * moveSpeed;
            dy = dy + (gravity * Time.deltaTime);
            //move(new Vector2(0, (dy * Time.deltaTime)));*/

            velocity = new Vector2(velocity.x + (moveSpeed * desiredDirection.x), velocity.y);
            rb2d.AddForce(velocity);
        }
        else //smoke trail system
        {
            /*int x, y = position:unpack();
            for (int i = 1; 3; 1)
            {

            }*/
        }
    }

    void stopKick()
    {
        if (locked)
        {
            locked = false;
            gravity = 600f;
            //self.hitbox:rotate(math.rad(-90))
		    //self.hitbox:scale(0.5)
        }
    }

    void kickRecoil()
    {
        if (locked)
        {
		    dy = -200;
		    dx = dx * -0.2;
		    gravity = 600f;
        }
        //self.bounceSound:rewind()
	    //self.bounceSound:play()
    }

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

    void move(float v)
    {
        Vector2 position = new Vector2(0, 0);
        position.x = position.x + v;
        position.y = position.y + v;
    }

    void keyPressed()
    {
        if (Input.GetKeyDown(KeyCode.X) && grounded & active)
        {
            dy = -180f;
            grounded = false;
            //jumpSound.Play();
        }
        else if (Input.GetKeyDown(KeyCode.C) && active && !locked && dashCharge > 50)
        {
            locked = true;
            gravity = 0;
            //dashSound.Play();
            dashCharge = dashCharge - 50;
            if (facingLeft)
            {
                //currentAnim = dashLeftAnim;
                dx = -400;
            }
            else
            {
                //currentAnim = dashRightAnim;
                dx = 400;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                dy = -200;
                dx = dx * 0.5;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                dy = 200;
                dx = dx * 0.5;
            }
            else
            {
                dy = 0;
                invuln = true;
                //self.hitbox:rotate(math.rad(90))
		        //self.hitbox:scale(2)
                float invulnTimer = 0.1f;
                invulnTimer -= Time.deltaTime;
                float kickTimer = 0.5f;
                kickTimer -= Time.deltaTime;
                stopKick();
            }
        }
    }
    //still need player draw functions
}
