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
    public float dashTime = 0.5f;
    float fireRate = 0.3f;
    float nextFire = 0f;
    public GameObject bulletPrefab;
    enum facing { Left, Right };
    string aimDirection;
    public Transform FirePoint;
    Vector3 dashDirection;
    public ParticleSystem dust;

    public bool locked = false;
    public bool firing = false;
    bool invuln = false;

    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip fireSound;
    public AudioClip hitSound;

    [SerializeField]
    GameObject afterImage;

    [HideInInspector]
    public float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private RaycastHit2D _lastControllerColliderHit;
    public Vector3 _velocity;
    public Sprite playerSprite;
    public Animator anim;
    SpriteRenderer playerSpriteRenderer;
    facing myFacing = facing.Right;


    private static controlPlayer instance;
    public static controlPlayer Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<controlPlayer>();
            return instance;
        }
    }

    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerSprite = playerSpriteRenderer.sprite;
        dust = GetComponent<ParticleSystem>();
        //dust.Stop();

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
        anim.SetFloat("speed", Mathf.Abs(_velocity.x));
        if (!_controller.isGrounded)
            anim.SetBool("jumping", true);
        else
            anim.SetBool("jumping", false);
        dashCharge = dashCharge + 40; //dash recharge
        if (dashCharge > 100)
            dashCharge = 100;

        if (_controller.isGrounded)
            _velocity.y = 0;

        keyPress();
        //if (!locked) //to use only on walking instead of also on dash
        applyMovement();
        if (locked)// && (_velocity.x != 0 || _controller.isGrounded))
        {
            //dust.Play();
            Instantiate(afterImage, transform.position, transform.rotation);//change afterimage life so they all get destroyed at same time
        }
        if (locked)
        {
            anim.SetBool("dashing", true);
            Instantiate(afterImage, transform.position, transform.rotation);//change afterimage life so they all get destroyed at same time

            if (dashTime > 0)
                dashTime -= Time.deltaTime;
            else
            {
                stopKick();
            }
        }
        if (!locked)
        {
            anim.SetBool("dashing", false);
            //dust.Pause();
        }
    }

    void keyPress()
    {
        if (!locked)//&& !firing)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                normalizedHorizontalSpeed = 1;
                myFacing = facing.Right;
                aimDirection = "right";

                if (transform.localScale.x < 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                normalizedHorizontalSpeed = -1;
                myFacing = facing.Left;
                aimDirection = "left";

                if (transform.localScale.x > 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                aimDirection = "up";
                normalizedHorizontalSpeed = 0;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                aimDirection = "down";
                normalizedHorizontalSpeed = 0;
            }
            else
            {
                normalizedHorizontalSpeed = 0;
                aimDirection = "";
            }
            if (locked)
                return;

            // we can only jump whilst grounded
            if (_controller.isGrounded && Input.GetKeyDown(KeyCode.Z))
            {
                AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position);
                _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            }
            else if (Input.GetKeyDown(KeyCode.C) && dashCharge > 50)
            {
                AudioSource.PlayClipAtPoint(dashSound, Camera.main.transform.position);
                if (Camera.main.GetComponent<CamShake>() != null)//when camera shakes, disable smoothCamera
                    Camera.main.GetComponent<CamShake>().Shake(0.05f, 0.1f);
                dash();
            }
            else if (Input.GetKeyDown(KeyCode.V)) //testing key
            {
                //if (Camera.main.GetComponent<CamShake>() != null)//when camera shakes, disable smoothCamera
                //Camera.main.GetComponent<CamShake>().Shake(0.05f, 0.1f);
                HurtPlayer();
            }
            //if (!locked) //for if we don't want player to move while shooting
            if (Input.GetKey(KeyCode.X) && Time.time > nextFire)
            {
                AudioSource.PlayClipAtPoint(fireSound, Camera.main.transform.position);
                nextFire = Time.time + fireRate;
                firing = true;
                shoot();
            }
            else
            {
                firing = false;
            }
        }
    }

    void applyMovement()
    {
        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?

        if (!locked)
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * moveSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        _velocity.y += gravity * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);
    }

    void shoot()
    {
        GameObject go = (GameObject)Instantiate(bulletPrefab, FirePoint.position, Quaternion.identity);
        if (aimDirection == "up")
        {
            go.GetComponent<bullet>().ySpeed += 0.1f;
        }
        else if (aimDirection == "down")
        {
            go.GetComponent<bullet>().ySpeed -= 0.1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            go.GetComponent<bullet>().xSpeed -= 0.1f;
            if (aimDirection == "up")
                go.GetComponent<bullet>().ySpeed += 0.1f;
            else if (aimDirection == "down")
                go.GetComponent<bullet>().ySpeed -= 0.1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            go.GetComponent<bullet>().xSpeed += 0.1f;
            if (aimDirection == "up")
                go.GetComponent<bullet>().ySpeed += 0.1f;
            else if (aimDirection == "down")
                go.GetComponent<bullet>().ySpeed -= 0.1f;
        }
        else
        {
            if (myFacing == facing.Left)
                go.GetComponent<bullet>().xSpeed -= 0.1f;
            if (myFacing == facing.Right)
                go.GetComponent<bullet>().xSpeed += 0.1f;
        }
    }

    void dash()
    {
        locked = true;
        gravity = 0;
        _velocity.x = 0;
        _velocity.y = 0;
        //start a timer on keypress, update it on update
        dashTime = 0.5f;
        dashCharge -= 50;
        if (myFacing == facing.Right)
            _velocity.x = dashSpeed;
        if (myFacing == facing.Left)
            _velocity.x = -dashSpeed;

        if (aimDirection == "up")
        {
            _velocity.y += dashSpeed;
        }
        else if (aimDirection == "down")
        {
            _velocity.y -= dashSpeed;
        }
        else
        {
            _velocity.y = 0;
        }
        applyMovement();


        /*invuln = true;
        //self.hitbox:rotate(math.rad(90))
        //self.hitbox:scale(2)
        float invulnTimer = 0.1f;
        invulnTimer -= Time.deltaTime;
        invuln = false;
        float kickTimer = 5f;
        kickTimer -= Time.deltaTime;*/
        //Debug.Log(kickTimer);
        //stopKick();

    }

    void stopKick()
    {
        if (locked)
        {
            locked = false;
            gravity = -25f;
            //self.hitbox:rotate(math.rad(-90))
            //self.hitbox:scale(0.5)
            //kickRecoil();
        }
    }

    void kickRecoil()
    {
        if (locked)
        {
            //dy = -200;
            _velocity.x = _velocity.x * -0.2f;
            gravity = -25f;
        }
        //bounceSound:play()
    }

    public void HurtPlayer()
    {
        StartCoroutine("ShowHitFlash");
    }

    IEnumerator ShowHitFlash()
    {        
        playerSpriteRenderer.material.shader = Shader.Find("PaintWhite");
        yield return new WaitForSeconds(0.25f);
        playerSpriteRenderer.material.shader = Shader.Find("Sprites/Default");
     }

}
