using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed {

public class ballEnemy : Actors {

	SpriteRenderer ballSpriteRenderer;

	public AudioClip ballHitSound;
	//enemyFlasher = GameObject.FindObjectOfType(TypeOf(flashSprite) as flashSprite;
	//flashSprite enemyFlasher = GameObject.AddComponent.(typeof(flashSprite)) as flashSprite;

	//int hp = 1;

	void Awake(){
		ballSpriteRenderer = GetComponent<SpriteRenderer>();
		//flashSprite enemyFlasher = gameObject.AddComponent.<flashSprite>();// as flashSprite;
		//enemyFlasher = GameObject.FindObjectOfType(typeof(flashSprite)) as flashSprite;
		//myObject.GetComponent<flashSprite>().flash();
	}
	
	protected override void Start(){
		base.Start();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.V)) //testing key
            {
				hitEnemy();
            }
		
	}

	public void hitEnemy()
    {
        //StartCoroutine("showHitFlash");
		//enemyFlasher.flash();
		//base.flashWhite();
		flashWhite();
		AudioSource.PlayClipAtPoint(ballHitSound, Camera.main.transform.position); //camera is moving left/right with player, messing up audio
    }

    /*IEnumerator showHitFlash()
    {        
        ballSpriteRenderer.material.shader = Shader.Find("PaintWhite");
        yield return new WaitForSeconds(0.15f);
        ballSpriteRenderer.material.shader = Shader.Find("Sprites/Default");
    }*/
	
	protected override void onHit<T>(T component){
		Player hitPlayer = component as Player;
		
		hitPlayer.test("Inheritance!");
	}
}
}
