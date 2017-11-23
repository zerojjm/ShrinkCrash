using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CarScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		inputX = 0;
		targetLane = 2;
		prevX = -10000;
		isDead = false;
		carDefaultYPos = 0.0f;
		onlyCarMoves = true;
		speed = 0;
		max_speed = 200;
		score = 0;
		stopped = true;
		crashAnimationTimer = 0;
		finished = false;
		moveDir = 0;
		yPos = 0;
		handleButtonPress = false;
		xSpeed = 0;
	}

	public float prevX;
	
	static public bool isDead;
	static public bool stopped;
	static public bool finished;

	static public bool onlyCarMoves;

	static public int targetLane;
	static public int moveDir;
	static public float yPos;
	static public float yHeight;

	static public bool handleButtonPress;

	static public int score;

	private float inputX;
	// 2 - Store the movement
	static public Vector2 movement;

	public List<float> rotationList; // For drifting
	float delayTimer;

	static public float speed;
	static private float max_speed;

	public static float getMaxSpeed() {return max_speed;}

	private float carDefaultYPos;

	private float crashAnimationTimer;

	private float xSpeed;

	public static void setMaxSpeed(float newMax)
	{
		max_speed = newMax;
	}
	// Update is called once per frame
	void Update () {

		/*if (rigidbody2D.position.x < BackgroundScript.stage_bounds.min.x || rigidbody2D.position.x > BackgroundScript.stage_bounds.max.x
						|| rigidbody2D.position.y < BackgroundScript.stage_bounds.min.y || rigidbody2D.position.y > BackgroundScript.stage_bounds.max.y)
			handleCrash ();*/

		if (isDead || stopped)
		{
			crashAnimation();
			movement = new Vector2(0,0);
			return;
		}

		if (SelectedLevelScript.selectedLevel == 1) 
		{
			max_speed = 350 + CarScript.score;
		}
		else if (SelectedLevelScript.selectedLevel == 2) 
		{
			max_speed = 500 + 2*CarScript.score;
		}
		else if (SelectedLevelScript.selectedLevel == 3) 
		{
			max_speed = 700 + 2*CarScript.score;
		}

		if (speed < max_speed)
			accelerate ();

		xSpeed = 0;

		float speedWDir = speed;

		if (moveDir == 0)
			speedWDir = -speedWDir;

		movement = new Vector2 (
			xSpeed, speedWDir);
	}

	void crashAnimation()
	{
		if (!isDead)
			return;
		crashAnimationTimer += Time.deltaTime;
		//float scaling = 0.9 * Time.deltaTime;

		transform.localScale = new Vector3(0.94f*transform.localScale.x,0.94f*transform.localScale.y,transform.localScale.z);
		//transform.Rotate(0, 0, 0.8f*scaling);

		if (crashAnimationTimer > 0.5) 
		{
			finished = true;
			Destroy(gameObject, 0.2f);
		}
	}
	void FixedUpdate()
	{
		if (isDead)
				rigidbody2D.velocity = Vector2.zero;
		else {

			Vector2 tmpMovement = new Vector2 (0, movement.y * Time.deltaTime);
			rigidbody2D.velocity = tmpMovement;
			yPos = transform.position.y;
			yHeight = renderer.bounds.size.y;
			//transform.Rotate(0, 0, rotation);
		}

	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		handleCrash();
	}

	private void accelerate()
	{
		speed += 1000 * Time.deltaTime;
		if (speed > max_speed)
						speed = max_speed;
	}

	private void handleCrash()
	{
		isDead = true;

		/*SpriteRenderer carSprite = GetComponent<SpriteRenderer>();
		if(carSprite != null)
		{
			carSprite.sprite = Resources.Load<UnityEngine.Sprite>("dasher-red");
		}*/
	}
}
