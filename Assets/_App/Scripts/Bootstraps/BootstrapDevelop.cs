using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BootstrapDevelop : MonoBehaviour {

    private string sceneName = "Main";

	void Start () {
        LoadContentScene();
    }


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
        root.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
