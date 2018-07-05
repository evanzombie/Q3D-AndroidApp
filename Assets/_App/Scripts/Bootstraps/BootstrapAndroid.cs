using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BootstrapAndroid : MonoBehaviour {


	// Plane Finder stuff
	public enum FocusState {
		Initializing,
		Finding,
		Found
	} 

	//for slide in menu animation
	public Animator TargetAnimator = null;

	public GameObject introText;
	public GameObject introTextPanel;
	public Button placementButton;
	public Button MenuBTN;
	public GameObject dropdownPanel;
	private bool isDropped = false; 

	public void what(){
		Debug.Log ("Asdf");
	}
	 
	public void MenuDropdown() {

		if (!isDropped) {
			isDropped = true;
			dropdownPanel.gameObject.SetActive (true);
			TargetAnimator.SetTrigger ("SlideIn");

		} else {
			isDropped = false;
			TargetAnimator.SetTrigger ("SlideOut");  
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
