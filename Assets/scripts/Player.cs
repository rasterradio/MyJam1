using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed {

public class Player : Actors
{
    protected override void Start()
    {
        
    }

    void Update()
    {
        
    }
	
	public void test(string message){
		Debug.Log(message);
	}
	
	protected override void onHit<T>(T component){
		ballEnemy hitEnemy = component as ballEnemy;
	}
}
}
