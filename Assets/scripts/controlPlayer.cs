using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class controlPlayer : MonoBehaviour {

	public float moveSpeed = 8f;
	public float gravity = -25f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public float dashCharge = 100f;
    public float dashSpeed = 8f;

	public bool active = true;
	public bool locked = false;
	public bool firing = false;
	bool facingLeft = false;
	bool invuln = false;
    //double dx;
    //double dy;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

	void Awake()
	{
		_controller = GetComponent<CharacterController2D>();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}

	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		dashCharge = dashCharge + 40; //dash recharge
        	if (dashCharge > 100)
            	dashCharge = 100;

		if( _controller.isGrounded )
			_velocity.y = 0;

        keyPress();
	}

    void stopKick()
    {
        if (locked)
        {
            locked = false;
            gravity = -25f;
            //self.hitbox:rotate(math.rad(-90))
            //self.hitbox:scale(0.5)
        }
    }

    void kickRecoil()
    {
        if (locked)
        {
            //dy = -200;
            //dx = dx * -0.2;
            //gravity = 600f;
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

    void keyPress()
    {
        moveSpeed = 8f;
        if (active && !locked)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                normalizedHorizontalSpeed = 1;
                if (transform.localScale.x < 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                normalizedHorizontalSpeed = 0;
            }
            if (Input.GetKey(KeyCode.X))
            {
                firing = true;
                //fire();
            }
            else
                firing = false;
            // we can only jump whilst grounded
            if (_controller.isGrounded && Input.GetKeyDown(KeyCode.Z))
            {
                _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            }
            else if (Input.GetKeyDown(KeyCode.C) && dashCharge > 50)
            {
                locked = true;
                moveSpeed = 20f;
                gravity = 0;
                //dashSound.Play();
                dashCharge = dashCharge - 50;
                //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                if (Input.GetKey(KeyCode.UpArrow))
                {

                }
                if (Input.GetKey(KeyCode.DownArrow))
                {

                }
                else
                {
                    //dy = 0;
                    invuln = true;
                    //self.hitbox:rotate(math.rad(90))
                    //self.hitbox:scale(2)
                    float invulnTimer = 0.1f;
                    invulnTimer -= Time.deltaTime;
                    invuln = false;
                    float kickTimer = 0.5f;
                    kickTimer -= Time.deltaTime;
                    stopKick();
                }
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                if (Camera.main.GetComponent<CamShake>() != null)
                    Camera.main.GetComponent<CamShake>().Shake(0.05f, 0.1f);
            }
        }



        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * moveSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        _velocity.y += gravity * Time.deltaTime;

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (_controller.isGrounded && Input.GetKey(KeyCode.DownArrow))
        {
            _velocity.y *= 3f;
            _controller.ignoreOneWayPlatformsThisFrame = true;
        }

        _controller.move(_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;

        if (locked)
        {
            //smoke trail system for dash
        }
    }

}
