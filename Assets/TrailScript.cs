using UnityEngine;
using System.Collections;

public class TrailScript : MonoBehaviour {

	private LineRenderer lineRenderer;

	GameObject cube; // Use this for initialization 
	// Use this for initialization
	void Start () {
		lineRenderer = gameObject.GetComponent<LineRenderer>(); 
		//the cube that is in the scene has beeen assigned the tag as a player. 
		cube = GameObject.Find ("Player");
		//It has two indexes by default. The initial point denotes the Index 0, and the 
		//final point(of the line renderer) denotes the index 1 

		//Set the width of the line renderer. 
		lineRenderer.SetWidth(cube.renderer.bounds.size.x, 3*cube.renderer.bounds.size.x/4); 
	}
	
	// Update is called once per frame
	void Update () {
		//the end position of the line will follow the player where ever it goes. 
		//This is the effect that I am talking about.
		lineRenderer = gameObject.GetComponent<LineRenderer>(); 
		float xSpeed = CarScript.movement [0]/600;
		float ySpeed = CarScript.movement [1]/200;

		Vector3 trailPos = new Vector3(cube.transform.position.x- xSpeed, 
		                               cube.transform.position.y - ySpeed, 
		                               cube.transform.position.z+1);

		lineRenderer.SetPosition(0, cube.transform.position); 
		lineRenderer.SetPosition(1, trailPos); 
	}
}
