using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class afterImage : MonoBehaviour {
	float timer = 0.1f; //trail length
    public SpriteRenderer[] sprites;

    void Start()
	{
        foreach (SpriteRenderer sprite in sprites)
        {
            GameObject trailPart = new GameObject();
            SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
            trailPartRenderer.sprite = sprite.sprite;
            trailPartRenderer.sortingLayerID = sprite.sortingLayerID;
            trailPart.transform.position = sprite.transform.position;
            trailPart.transform.localScale = sprite.transform.lossyScale;
        }
        transform.position = controlPlayer.Instance.transform.position;
		transform.localScale = controlPlayer.Instance.transform.localScale;
	}

	void Update()
	{
		timer -= Time.deltaTime;
        
		if (timer <= 0)
			Destroy (gameObject);
	}
}
