using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour {

	public Button LaunchBtn;
	public Canvas TutorialCanvas; 


	// Use this for initialization
	void Start () {
 
		if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
		{
			Debug.Log("First Time Opening");

			//Set first time opening to false
			PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);

			//Set Launch button to open tutorial page 
			LaunchBtn.onClick.AddListener (LoadTutorial);
		}
		else
		{
			Debug.Log("NOT First Time Opening");

			//Load Game
			LaunchBtn.onClick.AddListener (LoadGame);
		}
	}

	void LoadTutorial(){
		TutorialCanvas.gameObject.SetActive (true);
	}

	void LoadGame(){
		SceneManager.LoadScene (1); 
		SceneManager.LoadScene (2, LoadSceneMode.Additive);
	}
}
