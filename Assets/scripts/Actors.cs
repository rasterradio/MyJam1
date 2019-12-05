using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Completed {

public abstract class Actors : MonoBehaviour {
	private Rigidbody2D rb2D;
	private LayerMask collisionLayer;
	private BoxCollider2D boxCollider;
	private SpriteRenderer spriteRenderer;
	
	//screenshake
	//hit
	//death
	//shoot
	//HP
	//collision
	
	protected virtual void Start(){
		rb2D = GetComponent <Rigidbody2D>();
		boxCollider = GetComponent <BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	protected abstract void onHit<T>(T component)
		where T : Component; //child classes override component with hit target
		
	public void flashWhite(){
		StartCoroutine("showHitFlash");
    }

    public IEnumerator showHitFlash()
    {        
        spriteRenderer.material.shader = Shader.Find("PaintWhite");
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.material.shader = Shader.Find("Sprites/Default");
    }
}
}