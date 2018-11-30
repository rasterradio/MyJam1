using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class afterImage : MonoBehaviour {

	//SpriteRenderer sprite;
	float timer = 0.2f;
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

            Color color = trailPartRenderer.color;
            color.a -= 0.6f; // replace 0.5f with needed alpha decrement
            //color.
            //color.g -= 0.5f;
            //color.r -= 0.5f;
            trailPartRenderer.color = color;
            Destroy(trailPart, 0.5f); // replace 0.5f with needed lifeTime

        }

        //sprite = GetComponent<SpriteRenderer>();

        transform.position = controlPlayer.Instance.transform.position;
		transform.localScale = controlPlayer.Instance.transform.localScale;

		//sprite.sprite = controlPlayer.Instance.playerSprite;
		//sprite.color = new Vector4(50, 50, 50, 0.2f);
	}

	void Update()
	{
		timer -= Time.deltaTime;

		if (timer <= 0)
			Destroy (gameObject);
	}
}
