using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class afterImage : MonoBehaviour {

	SpriteRenderer sprite;
	float timer = 0.2f;

	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();

		transform.position = controlPlayer.Instance.transform.position;
		transform.localScale = controlPlayer.Instance.transform.localScale;

		sprite.sprite = controlPlayer.Instance.playerSprite;
		sprite.color = new Vector4(50, 50, 50, 0.2f);
	}

	void Update()
	{
		timer -= Time.deltaTime;

		if (timer <= 0)
			Destroy (gameObject);
	}
}
