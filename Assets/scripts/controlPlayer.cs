using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class controlPlayer : MonoBehaviour
{

    public float moveSpeed = 8f;
    public float gravity = -25f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;
    public float dashCharge = 100f;
    public float dashSpeed = 8f;
    public float dashTime = 3f;
    //public float currDashTime;

    public bool active = true;
    public bool locked = false;
    public bool firing = false;
    bool invuln = false;

    [SerializeField]
    GameObject afterImage;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private RaycastHit2D _lastControllerColliderHit;
    public Vector3 _velocity;
    public Sprite playerSprite;
    SpriteRenderer playerSpriteRenderer;

    private static controlPlayer instance;
    public static controlPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<controlPlayer>();
            }
            return instance;
        }
    }



    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerSprite = playerSpriteRenderer.sprite;

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
    }

    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }


    void onTriggerExitEvent(Collider2D col)
    {
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion

    void Update()
    {

        //dashCharge = dashCharge + 40; //dash recharge
        if (dashCharge > 100)
            dashCharge = 100;

        if (_controller.isGrounded)
        _velocity.y = 0;

        keyPress();
        if (locked && (_velocity.x != 0 || _controller.isGrounded))
        {
            GameObject playerGhost = Instantiate(afterImage, transform.position, transform.rotation);
        }
    }

    void keyPress()
    {
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
                dash();
            else if (Input.GetKeyDown(KeyCode.V))
            {
                if (Camera.main.GetComponent<CamShake>() != null)//when camera shakes, disable smoothCamera
                    Camera.main.GetComponent<CamShake>().Shake(0.05f, 0.1f);
            }
        }

        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * moveSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        _velocity.y += gravity * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);

        if (locked)
        {
            //smoke trail system for dash
        }
    }

    void dash()
    {
        locked = true;
        gravity = 0;
        //dashSound.Play();
        dashCharge = dashCharge - 50;
        //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //still need to apply velocity
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //diagonal dashing, add y factor
            //dx = dx * 0.5

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {

        }
        else
        {
            _velocity.y = 0;
        }
        invuln = true;
        //self.hitbox:rotate(math.rad(90))
        //self.hitbox:scale(2)
        float invulnTimer = 0.1f;
        invulnTimer -= Time.deltaTime;
        invuln = false;
        float kickTimer = 5f;
        kickTimer -= Time.deltaTime;
        //Debug.Log(kickTimer);
        stopKick();

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
            gravity = -25f;
        }
        //self.bounceSound:rewind()
        //self.bounceSound:play()
    }

}
