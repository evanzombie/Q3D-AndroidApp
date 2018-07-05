using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartApp : MonoBehaviour {

	public void LoadScene()
	{
		SceneManager.LoadScene (1); 
		SceneManager.LoadScene (2, LoadSceneMode.Additive);
	} 
 
}
 