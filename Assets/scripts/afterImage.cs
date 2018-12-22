using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class afterImage : MonoBehaviour {
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
            Destroy(trailPart, 0.5f); // replace 0.5f with needed lifeTime

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
