using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFlash : MonoBehaviour
{
	SpriteRenderer playerSpriteRenderer;
	
    void Start()
    {
		playerSpriteRenderer = GetComponent<SpriteRenderer>();      
    }
	
	public void flash()
    {
        StartCoroutine("showHitFlash");
    }

    IEnumerator showHitFlash()
    {        
        playerSpriteRenderer.material.shader = Shader.Find("PaintWhite");
        yield return new WaitForSeconds(0.15f);
        playerSpriteRenderer.material.shader = Shader.Find("Sprites/Default");
    }
}
