using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballEnemy : MonoBehaviour {

	SpriteRenderer ballSpriteRenderer;

	public AudioClip ballHitSound;

	int hp = 1;

	void Awake(){
		ballSpriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start () {
		
	}
	
	void Update () {
		
	}

	public void hitEnemy()
    {
        StartCoroutine("showHitFlash");
		AudioSource.PlayClipAtPoint(ballHitSound, Camera.main.transform.position);
    }

    IEnumerator showHitFlash()
    {        
        ballSpriteRenderer.material.shader = Shader.Find("PaintWhite");
        yield return new WaitForSeconds(0.15f);
        ballSpriteRenderer.material.shader = Shader.Find("Sprites/Default");
    }
}
