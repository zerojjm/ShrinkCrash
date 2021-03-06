﻿using UnityEngine;
using System.Collections;

public class LevelViewScript : MonoBehaviour {

	public GUIStyle lblStyle; 
	public Texture btnCar1Texture;
	public Texture btnCar2Texture;

	public Texture btnLeftTexture;
	public Texture btnRightTexture;

	public Vector2 scrollPosition = Vector2.zero;

	private Rect levelWindowRect;

	private int highScore1;
	private int highScore2;
	private int highScore3;
	private int highScore4;
	private int highScore5;

	public GUISkin mainSkin;
	public GUISkin cleanSkin;

	public static int currentPage = 1;

	float scrollXValue;
	bool scrollStarted;

	Touch touch;

	// Use this for initialization
	void Start () {
		lblStyle.fontSize = 24;
		lblStyle.alignment = TextAnchor.MiddleCenter;
		HighScoreScript hsScript = (HighScoreScript) gameObject.GetComponent(typeof(HighScoreScript));
		//hsScript.Save(); // This will reset highscores
		hsScript.Load();

		highScore1 = hsScript.highScores.highscore1;
		highScore2 = hsScript.highScores.highscore2;
		highScore3 = hsScript.highScores.highscore3;
		highScore4 = hsScript.highScores.highscore4;
		highScore5 = hsScript.highScores.highscore5;

		scrollStarted = false;
		scrollXValue = 0;
		AdHandlerScript.showBanner();
	}


	void OnGUI ()
	{
		GUI.skin = mainSkin;
		GUI.skin.window.normal.background = cleanSkin.window.normal.background;

		float winYSpace = Screen.height / 10;
		float winXSpace = Screen.width / 10;

		levelWindowRect = new Rect (winXSpace, 1.4f*winYSpace, Screen.width -2*winXSpace, Screen.height-2.4f*winYSpace);

		levelWindowRect = GUI.Window (0, levelWindowRect, DoLevelViewWindow, "");
	}

	void DoLevelViewWindow(int windowID) {

		float btnHeight = levelWindowRect.height / 10;
		float btnWidth = levelWindowRect.width / 4;

		GUI.skin = mainSkin;
		GUI.skin.button.fontSize = 22 * Screen.height / 800;
		lblStyle.fontSize = 26 * Screen.height / 800;

		float lvlBtnHeight = 1.7f * btnHeight;
		float lvlBtnMargin = 0.3f * btnHeight;

		//if (currentPage == 1 || currentPage == 3) 
		//{
			GUI.Label (new Rect (0 , levelWindowRect.height / 14, levelWindowRect.width - btnHeight/8, btnHeight), "SELECT LEVEL   ", lblStyle);

			GUI.skin = cleanSkin;
			
			/*if (GUI.Button (new Rect (btnHeight/2, levelWindowRect.height / 14, 
			                          btnHeight, btnHeight), btnLeftTexture)) {
				
				doPageChange();
			}
			if (GUI.Button (new Rect (levelWindowRect.width- btnHeight - btnHeight/2 , levelWindowRect.height / 14, 
			                          btnHeight, btnHeight), btnRightTexture)) {
				doPageChange();
			}*/
			GUI.skin = mainSkin;
			
			//if(currentPage == 1)
			//{
				if (GUI.Button (new Rect (Screen.width / 8, levelWindowRect.height / 14 + btnHeight, 2 * levelWindowRect.width / 3, lvlBtnHeight), " 1. BEGINNER \n   HIGHSCORE: " + highScore1.ToString ())) {
						SelectedLevelScript.selectedLevel = 1;

					Application.LoadLevel ("GameScene");
				}

				if (GUI.Button (new Rect (Screen.width / 8, levelWindowRect.height / 14 + btnHeight + lvlBtnHeight + lvlBtnMargin, 2 * levelWindowRect.width / 3, lvlBtnHeight), " 2. INTERMEDIATE \n   HIGHSCORE: " + highScore2.ToString ())) {
						SelectedLevelScript.selectedLevel = 2;
					Application.LoadLevel ("GameScene");
				}

				if (GUI.Button (new Rect (Screen.width / 8, levelWindowRect.height / 14 + btnHeight + 2 * lvlBtnHeight + 2 * lvlBtnMargin, 2 * levelWindowRect.width / 3, lvlBtnHeight), " 3. HARD \n   HIGHSCORE: " + highScore3.ToString ())) {
						SelectedLevelScript.selectedLevel = 3;
					Application.LoadLevel ("GameScene");
				}

			//}
			/*if(currentPage == 3)
			{


				if (GUI.Button (new Rect (Screen.width / 8, levelWindowRect.height / 14 + btnHeight, 2 * levelWindowRect.width / 3, lvlBtnHeight), " 4. HARDER \n   HIGHSCORE: " + highScore4.ToString ())) {
					SelectedLevelScript.selectedLevel = 4;
					
					currentPage = 2;
				}
				
				if (GUI.Button (new Rect (Screen.width / 8, levelWindowRect.height / 14 + btnHeight + lvlBtnHeight + lvlBtnMargin, 2 * levelWindowRect.width / 3, lvlBtnHeight), " 5. EXTREME \n   HIGHSCORE: " + highScore5.ToString ())) {
					SelectedLevelScript.selectedLevel = 5;
					currentPage = 2;
				}
			}	*/

			if (GUI.Button (new Rect (Screen.width / 8, Screen.height - Screen.height / 2.8f, 1.2f*btnWidth, btnHeight), "  BACK"))
						Application.LoadLevel ("TitleScene");
		//} 
		/*else if (currentPage == 2) 
		{
			GUI.Label (new Rect (0, levelWindowRect.height / 14, levelWindowRect.width- btnHeight/8, btnHeight), "SELECT CAR   ", lblStyle);

			GUI.skin = cleanSkin;

			if (GUI.Button (new Rect (Screen.width / 8, levelWindowRect.height / 14 + 2*btnHeight, 2 * levelWindowRect.width / 3, lvlBtnHeight), btnCar1Texture)) {
				SelectedLevelScript.selectedCar = 1;
				
				Application.LoadLevel ("GameScene");
			}
			
			if (GUI.Button (new Rect (Screen.width / 8, levelWindowRect.height / 14 + 2*btnHeight + lvlBtnHeight + 2*lvlBtnMargin, 2 * levelWindowRect.width / 3, lvlBtnHeight), btnCar2Texture)) {
				SelectedLevelScript.selectedCar = 2;
				
				Application.LoadLevel ("GameScene");
			}

			GUI.skin = mainSkin;
			GUI.skin.button.fontSize = 22 * Screen.height / 800;

			if (GUI.Button (new Rect (Screen.width / 8, Screen.height - Screen.height / 2.8f, 1.2f*btnWidth, btnHeight), "  BACK"))
			{
				if(SelectedLevelScript.selectedLevel>3)
					currentPage = 3;
				else
					currentPage = 1;
			}
		}*/
	}

	void Update()
	{
		/*if(Input.touchCount > 0)
		{
			touch = Input.touches[0];
			if (touch.phase == TouchPhase.Moved)
			{
				scrollXValue += touch.deltaPosition.x;
			}

			if(scrollXValue >levelWindowRect.width/4.0f)
			{
				scrollXValue = 0;
				doPageChange();

			}
		}*/
	}
	private void doPageChange()
	{
		if(currentPage == 1)
			currentPage = 3;
		else if(currentPage == 3)
			currentPage = 1;
	}
}


