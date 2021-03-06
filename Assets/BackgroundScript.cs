﻿using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {

	public Texture btnLeftTexture;

	public Texture btnRestartTexture;
	public Texture btnBackTexture;
	
	public GameObject upObstacle;
	public GameObject downObstacle;


	static public Bounds stage_bounds;

	static public int obstacleIndex;
	static public int iteration;
	static public int obstacleCount;
	static public int goCount = 0;

	private Rect gameOverWindowRect;

	private float startTimer;

	private GameObject numberObject;
	int showingNumber = 4;


	public GUIStyle lblStyle; 
	public GUIStyle lblStyleBig; 

	public GUIStyle btnTurnStyle; 

	public GUISkin mainSkin;
	public GUISkin keySkin;

	private bool videoAdToBeShown;
	private bool interstitialToBeShown;

	private bool easyPort;

	private int prevObstacle;

	// Use this for initialization
	void Start () {
	
		/*GameObject carObject = GameObject.Find ("Player");
		if (carObject != null) 
		{
			SpriteRenderer carSprite = carObject.GetComponent<SpriteRenderer>();
			if(carSprite != null)
			{
				if(SelectedLevelScript.selectedCar == 1)
					carSprite.sprite = Resources.Load<UnityEngine.Sprite>("car2");
				else if(SelectedLevelScript.selectedCar == 2)
					carSprite.sprite = Resources.Load<UnityEngine.Sprite>("rallycar");
			}
		}*/
		HighScoreScript hsScript = (HighScoreScript) gameObject.GetComponent(typeof(HighScoreScript));
		//hsScript.Save(); // This will reset highscores
		hsScript.Load();
		obstacleIndex = 0;
		obstacleCount = 0;
		iteration = 0;
		startTimer = 0;
		videoAdToBeShown = false;
		interstitialToBeShown = false;
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = Camera.main.orthographicSize * 2;
		stage_bounds = new Bounds(
			Camera.main.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));


		CreateObstacles ();

		showingNumber = 4;

		easyPort = true;
		prevObstacle = 0;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (CarScript.stopped)
			checkStartup ();

	}

	void checkStartup()
	{
		if (!CarScript.stopped)
			return;

		if (showingNumber > 3) 
		{
			Destroy(numberObject, 0.1f);
			numberObject = (GameObject) GameObject.Instantiate(Resources.Load("Number3", typeof(GameObject)));
			showingNumber = 3;
		}
		if (startTimer > 1 && showingNumber == 3) 
		{
			Destroy(numberObject, 0.1f);
			numberObject = (GameObject) GameObject.Instantiate(Resources.Load("Number2", typeof(GameObject)));
			showingNumber = 2;
		}
		if (startTimer > 2 && showingNumber == 2) 
		{
			Destroy(numberObject, 0.1f);
			numberObject = (GameObject) GameObject.Instantiate(Resources.Load("Number1", typeof(GameObject)));
			showingNumber = 1;
		}
		if (startTimer > 2.8f && showingNumber == 1) 
		{
			Destroy(numberObject, 0.1f);
			numberObject = (GameObject) GameObject.Instantiate(Resources.Load("Number0", typeof(GameObject)));
			showingNumber = 0;
			Destroy(numberObject, 0.8f);
			AdHandlerScript.hideBanner();
		}
		numberObject.transform.position = new Vector3(0,0,-3);
		startTimer += Time.deltaTime;

		float scaling = 5 * Time.deltaTime;
		numberObject.transform.localScale = new Vector3(numberObject.transform.localScale.x + scaling,numberObject.transform.localScale.y+scaling,numberObject.transform.localScale.z);
		if (startTimer > 3) 
		{
			AdHandlerScript.hideBanner();

			CarScript.stopped = false;
			goCount++;
			if (goCount == 5 || goCount == 30) 
			{
				videoAdToBeShown = true;
			}
			if (goCount == 10 || goCount == 50) 
			{
				interstitialToBeShown = true;
			}
			if(numberObject)
				Destroy(numberObject, 0.8f);
		}
	}
	void OnGUI () {

		GUI.skin = mainSkin;
		GUI.skin.window.normal.background = keySkin.window.normal.background;

		if (CarScript.finished) {
			float winYSpace = Screen.height/10;
			float winXSpace = Screen.width/10;

			Rect gameOverWindowRect = new Rect(winXSpace, 1.4f*winYSpace, Screen.width -2*winXSpace, Screen.height-2.4f*winYSpace);

			gameOverWindowRect = GUI.Window(0, gameOverWindowRect, DoGameOverWindow, "");

		} else {
			GUI.skin = keySkin;

			float btnHeight = Screen.height / 4;
			if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), btnLeftTexture)) {
				if(CarScript.moveDir == 0) CarScript.moveDir = 1;
				else if (CarScript.moveDir == 1) CarScript.moveDir = 0;
				CarScript.speed = 0;

				if(!CarScript.isDead)
				{
					CarScript.score = CarScript.score +1;
					moveObstacle();
				}

			} 

			lblStyle.fontSize = 24 * Screen.height / 800;

			GUI.Label (new Rect (3*Screen.width / 100, 3*Screen.width / 100, Screen.width / 2, btnHeight / 8), "SCORE: " + CarScript.score.ToString (),lblStyle);
		}
	}

	void DoGameOverWindow(int windowID) {

		if (!AdHandlerScript.bannerVisible)
		{
			AdHandlerScript.showBanner();
		}

		GUI.skin = mainSkin;
		GUI.skin.button.fontSize = 22 * Screen.height / 800;

		float btnHeight = Screen.height / 16;
		float btnWidth = Screen.width / 4;
		float lblStartY = btnHeight;
		float btnStartY = Screen.width / 2 + 1.6f*btnHeight;

		lblStyleBig.fontSize = 34 * Screen.height / 800;
		lblStyle.fontSize = 26 * Screen.height / 800;

		GUI.Label(new Rect(Screen.width / 2 -1.3f*btnWidth, Screen.height / 10, 2*btnWidth, btnHeight), "GAME OVER!", lblStyleBig);

		GUI.Label(new Rect(Screen.width / 2 -1.0f*btnWidth, Screen.height / 10+1.5f*btnHeight, 2*btnWidth, btnHeight), "SCORE: "+ CarScript.score.ToString(), lblStyle);

		HighScoreScript hsScript = (HighScoreScript) gameObject.GetComponent(typeof(HighScoreScript));

		int hsScore = 0;
		if (SelectedLevelScript.selectedLevel == 1) 
		{
			hsScore = hsScript.highScores.highscore1;
		}
		else if (SelectedLevelScript.selectedLevel == 2) 
		{
			hsScore = hsScript.highScores.highscore2;
		}
		else if (SelectedLevelScript.selectedLevel == 3) 
		{
			hsScore = hsScript.highScores.highscore3;
		}
		else if (SelectedLevelScript.selectedLevel == 4) 
		{
			hsScore = hsScript.highScores.highscore4;
		}
		else if (SelectedLevelScript.selectedLevel == 5) 
		{
			hsScore = hsScript.highScores.highscore5;
		}
		int carScore = CarScript.score;

		if (hsScore <= carScore) 
		{
			GUI.Label (new Rect (Screen.width / 2 - 1.5f * btnWidth, Screen.height / 10 + 3f * btnHeight, btnWidth*5, btnHeight), "THAT'S A HIGHSCORE!", lblStyle);

			if (SelectedLevelScript.selectedLevel == 1) 
			{
				hsScript.highScores.highscore1 = carScore;
			}
			else if (SelectedLevelScript.selectedLevel == 2) 
			{
				hsScript.highScores.highscore2 = carScore;
			}
			else if (SelectedLevelScript.selectedLevel == 3) 
			{
				hsScript.highScores.highscore3 = carScore;
			}
			else if (SelectedLevelScript.selectedLevel == 4) 
			{
				hsScript.highScores.highscore4 = carScore;
			}
			else if (SelectedLevelScript.selectedLevel == 5) 
			{
				hsScript.highScores.highscore5 = carScore;
			}
			hsScript.Save();
		}

		if (GUI.Button(new Rect(Screen.width / 2 -btnWidth, btnStartY, btnWidth, 1.2f*btnHeight), " RESTART"))
			Application.LoadLevel("GameScene");

		if (GUI.Button(new Rect(Screen.width / 2 -btnWidth, btnStartY+2*btnHeight, btnWidth, 1.2f*btnHeight), "  BACK"))
			Application.LoadLevel("LevelScene");

		if (videoAdToBeShown)
		{
			AdHandlerScript.showVideoAd();
			videoAdToBeShown = false;
		}
		if (interstitialToBeShown)
		{
			AdHandlerScript.showInterstitial();
			interstitialToBeShown = false;
		}
	}

	public void CreateObstacles()
	{

		//Create obstacle(s)

		upObstacle = (GameObject) GameObject.Instantiate(Resources.Load("Obstacle_green", typeof(GameObject)));
		downObstacle = (GameObject) GameObject.Instantiate(Resources.Load("Obstacle_red", typeof(GameObject)));


		float targetSize = stage_bounds.size.x; //Screen width/3
		
		float currentSize = upObstacle.renderer.bounds.size.x; 
		Vector3 scale = upObstacle.transform.localScale;
		scale.x = targetSize * scale.x / currentSize;
		scale.y = scale.y / 2.0f;
		upObstacle.transform.localScale = scale;

		float y_pos = BackgroundScript.stage_bounds.max.y;
		float x_pos = 0;
		float z_pos = upObstacle.transform.position.z;
		
		upObstacle.transform.position = new Vector3(x_pos,y_pos,z_pos);

		currentSize = downObstacle.renderer.bounds.size.x; 
		scale = downObstacle.transform.localScale;
		scale.x = targetSize * scale.x / currentSize;
		scale.y = scale.y / 2.0f;
		downObstacle.transform.localScale = scale;

		y_pos = BackgroundScript.stage_bounds.min.y;

		downObstacle.transform.position = new Vector3(x_pos,y_pos,z_pos);


	}

	public void moveObstacle()
	{
		if (CarScript.moveDir == 0) 
		{
			float y_pos = upObstacle.transform.position.y;
			float x_pos = upObstacle.transform.position.x;
			float z_pos = upObstacle.transform.position.z;

			float y_player = CarScript.yPos;

			y_pos -= upObstacle.renderer.bounds.size.y/2.0f;
			y_player += CarScript.yHeight/2.0f;
			//upObstacle.transform.position = new Vector3(x_pos,(y_pos+y_player)/2.0f + upObstacle.renderer.bounds.size.y/2.0f,z_pos);

			ObstacleScript obsScript = (ObstacleScript) upObstacle.GetComponent(typeof(ObstacleScript));
			if (obsScript) 
			{
				obsScript.yTarget = (y_pos+y_player)/2.0f + upObstacle.renderer.bounds.size.y/2.0f;
				//obsScript.yTarget = y_player + upObstacle.renderer.bounds.size.y/2.0f;
				obsScript.doMove = true;
			}

		} 
		else if (CarScript.moveDir == 1) 
		{
			float y_pos = downObstacle.transform.position.y;
			float x_pos = downObstacle.transform.position.x;
			float z_pos = downObstacle.transform.position.z;
			
			float y_player = CarScript.yPos;
			y_pos += downObstacle.renderer.bounds.size.y/2.0f;
			y_player -= CarScript.yHeight/2.0f;
			//downObstacle.transform.position = new Vector3(x_pos,(y_pos+y_player)/2.0f - downObstacle.renderer.bounds.size.y/2.0f,z_pos);

			ObstacleScript obsScript = (ObstacleScript) downObstacle.GetComponent(typeof(ObstacleScript));
			if (obsScript) 
			{
				obsScript.yTarget = (y_pos+y_player)/2.0f - downObstacle.renderer.bounds.size.y/2.0f;
				//obsScript.yTarget = y_player - downObstacle.renderer.bounds.size.y/2.0f;
				obsScript.doMove = true;
			}
		}
	}
}
