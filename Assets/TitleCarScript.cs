using UnityEngine;
using System.Collections;

public class TitleCarScript : MonoBehaviour {

	private Bounds stage_bounds;

	private float speed;
	// Use this for initialization
	void Start () {
	
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = Camera.main.orthographicSize * 2;
		stage_bounds = new Bounds(
			Camera.main.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

		speed = 150;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (transform.position.y > stage_bounds.max.y-1.4f) 
		{
			speed = -150;
		}
		if (transform.position.y < stage_bounds.min.y+1.4f) 
		{
			speed = 150;
		}
	}

	void FixedUpdate()
	{

			Vector2 tmpMovement = new Vector2 (0, speed*Time.deltaTime);
			rigidbody2D.velocity = tmpMovement;
		
	}
}
