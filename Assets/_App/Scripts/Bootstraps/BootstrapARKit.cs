#if ARKIT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BootstrapARKit : MonoBehaviour {

  // Plane Finder stuff
	public enum FocusState {
		Initializing,
		Finding,
		Found
	} 
  
  //for slide in menu animation
	public Animator TargetAnimator = null;  
  //for editor version
  public float maxRayDistance = 30.0f;
  public LayerMask collisionLayerMask;
  public float findingSquareDist = 0.5f;
  public GameObject introText;
  public GameObject introTextPanel;
  public Button placementButton;
  public Button MenuBTN;
  public GameObject dropdownPanel;
  private bool isDropped = false; 
	public Button assetOneBtn;
	public Button assetTwoBtn;
	private GameObject myAsset2;   
	protected int currentAssetNumber = 1;

  private UnityARAnchorManager unityARAnchorManager;

  //Bootstrapper Stuff
  public PlacementPrompt placementPrompt;
  public string sceneName = "Main";
  private bool placementSelected = false;
  private Vector3 statusVelocity;
  public static Vector3 GazeOffset = new Vector3(0.0f, -0.1f, 0.0f);

  private Toggle loopToggle;
 
  void LoadContentScene() {
    // Load Main scene
    Scene s = SceneManager.GetSceneByName(sceneName);
		 
    if (s.isLoaded) {
      OnSceneLoaded(s, LoadSceneMode.Additive);
    }
    else {
      SceneManager.sceneLoaded += OnSceneLoaded;
      SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
  } 

  void OnSceneLoaded(Scene s, LoadSceneMode m) {
    // Move the MainRoot to the placement prompt position
        GameObject root = GameObject.Find("MainRoot");  
  
		var allObjects = Resources.FindObjectsOfTypeAll<MainAssetSwitch>(); 
		var targetobject = allObjects[0];  
 
		if (currentAssetNumber == 1) {
			targetobject.asset1.SetActive (true);
			targetobject.asset2.SetActive (false);	
		} else {
			targetobject.asset1.SetActive (false);
			targetobject.asset2.SetActive (true);
		}

    if (root) {
			if (currentAssetNumber == 1) {
				targetobject.asset1.SetActive (true);
				targetobject.asset2.SetActive (false);	
			} else {
				targetobject.asset1.SetActive (false);
				targetobject.asset2.SetActive (true);
			}

      root.SetActive(true); 
      root.transform.position = placementPrompt.transform.position;
      root.transform.rotation = placementPrompt.transform.rotation;
      Controller ctrl = root.GetComponentInChildren<Controller>(); 

      ctrl.SetLooping(loopToggle.isOn);
    }
    placementPrompt.gameObject.SetActive(false);
    SceneManager.sceneLoaded -= OnSceneLoaded;
  } 

  public void StartPlacement() {

		var allObjects = Resources.FindObjectsOfTypeAll<MainAssetSwitch>();
		Debug.Log (allObjects.Length);
		if (allObjects.Length > 0) {
			Debug.Log (allObjects [0]);
			var targetobject = allObjects [0];  

			if (!placementSelected) {
				placementSelected = true;
				loopToggle.gameObject.SetActive (false);
				placementButton.GetComponentInChildren<Text> ().text = "Reposition";
				introText.gameObject.SetActive (false);
				introTextPanel.gameObject.SetActive (false);  

				targetobject.enginMover.SetActive (true);
				targetobject.speaker.SetActive (true); 
				introText.GetComponentInChildren<Text> ().text = "The current asset is active. Please retry when demo is not live.";
				assetOneBtn.onClick.RemoveAllListeners();
				assetTwoBtn.onClick.RemoveAllListeners();

				LoadContentScene ();
			} else {
				placementSelected = false;
				loopToggle.gameObject.SetActive (true);
				placementButton.GetComponentInChildren<Text> ().text = "Select Placement";
				introText.gameObject.SetActive (true);
				introTextPanel.gameObject.SetActive (true); 
				placementPrompt.gameObject.SetActive (false);

				targetobject.enginMover.SetActive (false);
				targetobject.asset1.SetActive (false);
				targetobject.speaker.SetActive (false);
				introText.GetComponentInChildren<Text> ().text = "Slowly point the device to a horizontal surface for best viewing results.";
				 
				assetOneBtn.onClick.AddListener (GetAssetOne);
				assetTwoBtn.onClick.AddListener (GetAssetTwo); 

//				Scene s = SceneManager.GetSceneByName (sceneName);
//				if (s.isLoaded) {
//					SceneManager.UnloadSceneAsync (sceneName);
//				}
			}
		}
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

	public void SwitchAssets(){

//		Scene s = SceneManager.GetSceneByName (sceneName); 
//
//		GameObject asset1 = GameObject.Find("engine_all_00");
//		GameObject asset2 = GameObject.Find("Model"); 
		var allObjects = Resources.FindObjectsOfTypeAll<MainAssetSwitch>();
		if (allObjects.Length > 0) {
			var targetobject = allObjects [0];   
			var go = EventSystem.current.currentSelectedGameObject;
			if (go != null){
				Debug.Log ("Clicked on : " + go.name);
				if (targetobject.speaker.activeSelf) { 

					if (!assetOneBtn.IsInteractable()) {
						Debug.Log ("asset 1 disabled");
						assetTwoBtn.interactable = true; 
						assetOneBtn.interactable = false;  
					} else {
						Debug.Log ("asset 2 disabled");
						assetOneBtn.interactable = true;
						assetTwoBtn.interactable = false;  
					}

 					introText.gameObject.SetActive(true);
					introTextPanel.gameObject.SetActive(true); 	 
				} else { 
					if (go.name == "Asset 1") { 
						targetobject.asset1.gameObject.SetActive (true);
						targetobject.asset2.gameObject.SetActive (false); 

						assetOneBtn.interactable = true;
						assetTwoBtn.interactable = false;

						currentAssetNumber = 1;

					} else if (go.name == "Asset 2") { 
						targetobject.asset1.gameObject.SetActive (false);
						targetobject.asset2.gameObject.SetActive (true);

						assetOneBtn.interactable = false;
						assetTwoBtn.interactable = true;

						currentAssetNumber = 2;
					} 
 
				}

			}else {
				Debug.Log ("currentSelectedGameObject is null");
			} 
		}
	}

 
	// Use this for initialization
	void Start () {
		 
     loopToggle = gameObject.GetComponentInChildren<Toggle>();
     unityARAnchorManager = new UnityARAnchorManager();
//      StartPlacement();
		   
		//Set Asset 1 button to be pressed as the default
		assetOneBtn.interactable = false;
		assetOneBtn.onClick.AddListener (GetAssetOne);
		assetTwoBtn.onClick.AddListener (GetAssetTwo); 
		//Stop loading asset 1 at the beginning. 
		var allObjects = Resources.FindObjectsOfTypeAll<MainAssetSwitch>(); 
		var targetobject = allObjects[0];  
		targetobject.enginMover.SetActive(false);  
	}

	void GetAssetOne()
	{	  
		assetOneBtn.interactable = false;
		assetTwoBtn.interactable = true;
	} 
	void GetAssetTwo()
	{	 
		assetOneBtn.interactable = true;
		assetTwoBtn.interactable = false;
	} 

  void OnDestroy()
  {
    unityARAnchorManager.Destroy();
  }


  bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
  {
    List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
    if (hitResults.Count > 0) {
      foreach (var hitResult in hitResults) {
        
        Vector3 pos = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
        placementPrompt.SetHit(pos);
        placementPrompt.gameObject.SetActive(true);
        introText.gameObject.SetActive(false);
				introTextPanel.gameObject.SetActive(false);
		 MenuBTN.gameObject.SetActive (true);
        return true;
      }
    }
    return false;
  }

	// Update is called once per frame
	void Update () {

    if (placementSelected)
      return;

    //use center of screen for focusing
    Vector3 center = new Vector3(Screen.width/2, Screen.height/2, findingSquareDist);


    var screenPosition = Camera.main.ScreenToViewportPoint(center);
    ARPoint point = new ARPoint {
      x = screenPosition.x,
      y = screenPosition.y
    };

    // prioritize reults types
    ARHitTestResultType[] resultTypes = {
      ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
      // if you want to use infinite planes use this:
      //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
      //ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
      //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
    }; 

    foreach (ARHitTestResultType resultType in resultTypes)
    {
      if (HitTestWithResultType (point, resultType))
      {
        return;
      }
    }
     

    Ray ray = Camera.main.ScreenPointToRay (center);
    RaycastHit hit;

    //we'll try to hit one of the plane collider gameobjects that were generated by the plugin
    //effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
    if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayerMask)) {
      //we're going to get the position from the contact point
      placementPrompt.SetHit(hit.point);
      return;
    }
      

	}


}
#endif
