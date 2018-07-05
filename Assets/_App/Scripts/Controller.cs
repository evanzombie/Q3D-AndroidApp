using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

  [System.Serializable]
  public struct MarkData { 
    public float time; 
    public AudioClip audio; 
  }

  public GameObject ux2d;
  public GameObject ux3d;

  public bool paused = false;
  public Animator animator;
  public Button restartButton;

  public MarkData[] marks;
  public int currentMark = -1;
  public float currentTime = 0.0f;

  private GameObject playUIGroup;
  private GameObject ux;
  private AudioSource audioSource;

  private AudioClip introAudio = null;
  private bool holdAnimForAudio = false;
  private bool looping = true;

  public void SkipForward() {
    StartMark(currentMark + 1);
  }

  public void SkipBackward() {
    PlayAnimationAtCurrentMark();
    StartMark(currentMark);
  }

  private void PlayAnimationAtCurrentMark() {
    holdAnimForAudio = false;
    currentTime = marks[currentMark].time;
    for (int i = 0; i < animator.layerCount; i++) {
      animator.PlayInFixedTime(animator.GetCurrentAnimatorStateInfo(i).fullPathHash, -1, currentTime);
    }
  }

  public void SetLooping(bool loop) {
    looping = loop;
  }

  public void Restart() {
    Debug.Log("Restart");
    audioSource.clip = introAudio;
    audioSource.Play();
    currentMark = -1;
    currentTime = 0;
    paused = false;
    for (int i = 0; i < animator.layerCount; i++) {
      animator.Play(animator.GetCurrentAnimatorStateInfo(i).fullPathHash, -1, 0.0f);
    }
    restartButton.gameObject.SetActive(false);
    playUIGroup.SetActive(true);
    UpdatePlayState();
  }

  public void StartMark(int i) {
    Debug.Log("Start Mark: " + i + " (Current Mark: " + currentMark + ")");
    if (audioSource.isPlaying)
      audioSource.Stop();

    if (i < 0 || i >= marks.Length) {
      // special case for rollovers
      Restart();
    } else {
      currentMark = i;
      if (marks[currentMark].audio != null) {
        Pause(); // don't play animation audio
        holdAnimForAudio = true;
        audioSource.clip = marks[currentMark].audio;
        audioSource.Play();
      } else {
        Resume();
      }
    }
  }

  public void EndOfSequence() {
    Pause();
    restartButton.gameObject.SetActive(true);
    playUIGroup.SetActive(false);

    if (looping) {
      Restart();
    }
  }
    
  public void Reposition() {
    Pause();
#if ARKIT
    BootstrapARKit bootstrap = FindObjectOfType<BootstrapARKit>();
    bootstrap.StartPlacement();
#endif

#if HOLOLENS
    BootstrapHoloLens bootstrap = FindObjectOfType<BootstrapHoloLens>();
    bootstrap.StartPlacement();
#endif
    }

    public void Resume() {
    Debug.Log("Resume Animation");
    paused = false;
    UpdatePlayState();
  }

  public void Pause() {
    Debug.Log("Pause Animation");
    paused = true;
    UpdatePlayState();
  }
    
  public void UpdatePlayState() {
    //playPauseButton.GetComponentInChildren<Text>().text = (paused? "Resume" : "Play");
    animator.speed = (paused? 0 : 1);
    //playUIGroup.SetActive(paused);
  }

	// Use this for initialization
	void Start () {

   
    #if HOLOLENS
    ux = ux3d;
    ux2d.SetActive(false);
    #else
    ux = ux2d;
    ux3d.SetActive(false);
    #endif

    ux.SetActive(true);
    playUIGroup = GameObject.Find("PlayUI");
    audioSource = ux.GetComponentInChildren<AudioSource>();
    introAudio = audioSource.clip; // cache the intro audio
    Restart();
    //playUIGroup.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

    if (holdAnimForAudio) {
      if (!audioSource.isPlaying) {
        audioSource.Stop();
        PlayAnimationAtCurrentMark();
        Resume();
      } else {
        return;
      }
    }

  

    if (paused) {
      if (looping && currentMark < 0 && !audioSource.isPlaying)
        SkipForward();
    } else {
      currentTime += Time.deltaTime;

      int nextMark = currentMark + 1;

      if (currentTime >= animator.GetCurrentAnimatorStateInfo(0).length) {
        // End of Sequence
        EndOfSequence();

      } else if (nextMark < marks.Length && currentTime > marks[nextMark].time) {
        Debug.Log("Mark Hit: " + nextMark);

        if (looping) {
          if (nextMark > 0) {
            SkipForward();
          } else { 
            Pause();
          }
        } else {
          // Next Step
          Pause();
        }
      }
    
    }
	}
}
