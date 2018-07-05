#if HOLOLENS
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using System.Linq;



public class BootstrapHoloLens : MonoBehaviour {

    public string sceneName = "Main";
    public PlacementPrompt placementPrompt;
    public GameObject spatialData;
    public GameObject statusPrompt;

    private UnityEngine.XR.WSA.Input.GestureRecognizer gestures = null;
    private bool placementSelected = false;
    private Camera camera;
    private Vector3 statusVelocity;
    private Controller stepController;
    public static Vector3 GazeOffset = new Vector3(0.0f, -0.1f, 0.0f);

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void Start() {
        camera = Camera.main;

        // Hook up air tap
        gestures = new UnityEngine.XR.WSA.Input.GestureRecognizer();
        gestures.TappedEvent += OnTap;
        gestures.StartCapturingGestures();

        // Hook up voice
        keywords.Add("repeat", () =>
        {
            if (stepController)
                stepController.SkipBackward();
        });

        keywords.Add("next", () => {
            if (stepController)
                stepController.SkipForward();
        });

        keywords.Add("start over", () => {
            if (stepController)
                stepController.Restart();
        });

        keywords.Add("reposition", () => {
            if (stepController)
                stepController.Reposition();
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeyword;
        keywordRecognizer.Start();
    }

    void OnDestroy() {
        gestures.StopCapturingGestures();
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
        root.transform.position = placementPrompt.transform.position;
        root.transform.rotation = placementPrompt.transform.rotation;
        root.AddComponent<UnityEngine.XR.WSA.WorldAnchor>();

        stepController = root.GetComponentInChildren<Controller>();
    }

    public void StartPlacement() {

        if (!placementSelected) {
            placementSelected = true;
            LoadContentScene();
        }
        else {
            placementSelected = false;
            statusPrompt.gameObject.SetActive(true);
            placementPrompt.gameObject.SetActive(false);
            Scene s = SceneManager.GetSceneByName(sceneName);
            if (s.isLoaded) {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }


    void Update() {
        if (!placementSelected) {
            if (spatialData.transform.childCount > 0) {
                if (statusPrompt.activeInHierarchy)
                    statusPrompt.SetActive(false);
                PlacementUpdate();
            } else {
                Vector3 target = camera.transform.position + GazeOffset + (camera.transform.forward * 2.0f);
                statusPrompt.transform.position = Vector3.SmoothDamp(statusPrompt.transform.position, target, ref statusVelocity, 0.2f);
            }
        }
    }

    void PlacementUpdate() {

        if (!placementPrompt.gameObject.activeInHierarchy)
            placementPrompt.gameObject.SetActive(true);

        // Raycast against all game objects that are on either the
        // spatial surface or UI layers.
        int layerMask = 1 << LayerMask.NameToLayer("HoloLensSurface");
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position + GazeOffset, Camera.main.transform.forward, 3.0f, layerMask);

        if (hits.Length > 0) {
            int closestHit = 0;
            int h = 0;
            float closestDist = float.MaxValue;
            foreach (RaycastHit hit in hits) {
                Vector3 delta = hit.point - camera.transform.position;
                if (delta.sqrMagnitude < closestDist) {
                    closestDist = delta.sqrMagnitude;
                    closestHit = h;
                }
                h++;
            }
            placementPrompt.SetHit(hits[closestHit].point);
        }
    }

    private void OnKeyword(PhraseRecognizedEventArgs args) {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction)) {
            keywordAction.Invoke();
        }
    }

    void OnTap(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray headRay) {
       if (!placementSelected) {
            placementSelected = true;
            placementPrompt.gameObject.SetActive(false);
            LoadContentScene();
       } else {
            stepController.SkipForward();
       }
    }

}

#endif