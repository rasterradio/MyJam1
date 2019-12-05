using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class flash : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	
    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();      
    }
	
	public void flashing()
    {
        StartCoroutine("showHitFlash");
    }

    public IEnumerator showHitFlash()
    {        
        spriteRenderer.material.shader = Shader.Find("PaintWhite");
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.material.shader = Shader.Find("Sprites/Default");
    }
}
