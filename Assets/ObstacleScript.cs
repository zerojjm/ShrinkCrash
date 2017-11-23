using UnityEngine;
using System.Collections;
using System;

public class ObstacleScript : MonoBehaviour {
	
	private float obstacleMovement;

	public volatile bool hasNext = false;

	public int id;

	private float obsSpeed = 500f;

	public float yTarget;
	public bool doMove;
	private bool moveDirDown;

	// Use this for initialization
	void Start () {
	
		obstacleMovement = 0;
		doMove = false;
		if (transform.position.y < yTarget)
				moveDirDown = false;
		else
				moveDirDown = true;
	}

	void setYTarget(float newTarget)
	{
		yTarget = newTarget;

	}
	// Update is called once per frame
	void Update () {

		obstacleMovement = 0;
		if (CarScript.isDead)
			obstacleMovement = 0;

		if (doMove && moveDirDown && transform.position.y < yTarget + 0.1) 
		{
			doMove = false;
			float y_pos = gameObject.transform.position.y;
			float x_pos = gameObject.transform.position.x;
			float z_pos = gameObject.transform.position.z;
			gameObject.transform.position = new Vector3(x_pos,yTarget,z_pos);
		}
		if (doMove && !moveDirDown && transform.position.y > yTarget - 0.1) 
		{
			doMove = false;
			float y_pos = gameObject.transform.position.y;
			float x_pos = gameObject.transform.position.x;
			float z_pos = gameObject.transform.position.z;
			gameObject.transform.position = new Vector3(x_pos,yTarget,z_pos);
		}

		if (doMove) 
		{
			obstacleMovement = 300f;
			if(moveDirDown) obstacleMovement = -obstacleMovement;
		}
	}

	void FixedUpdate()
	{

		if (CarScript.isDead)
				rigidbody2D.velocity = Vector2.zero;
		else {
			Vector2 tmpMovement = new Vector2 (0, obstacleMovement * Time.deltaTime);
				rigidbody2D.velocity = tmpMovement;
		}
		//transform.Rotate(0, 0, rotation);
	}
}
