﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour {

    public Camera mainCam;
    public Vector3 startPos;
    float shakeAmount = 0f;
    float camTimer = 0.5f;
    SmoothFollow camFollow;

    private void Awake()
    {
        camFollow = GetComponent<SmoothFollow>();
        startPos = transform.position;
        if (mainCam == null)
            mainCam = Camera.main;
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void DoShake()
    {
        //camFollow.enabled = false;
       
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;
        }
        

    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        mainCam.transform.localPosition = startPos;
        /*if (camTimer > 0)
        {
            camTimer -= Time.deltaTime;
        }
        else
            camFollow.enabled = true;*/

    }
}
