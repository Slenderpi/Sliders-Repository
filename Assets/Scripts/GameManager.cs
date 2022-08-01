using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public delegate void BindableEvent();
	public static event BindableEvent OnHasSwiped;
	public static event BindableEvent SetData;

	public readonly static float MIN_DRAG_DISTANCE = 10f;
	public static float sliderVelocity = 12f;

	public GameObject dIndicatorPrefab;
	public GameObject directionIndicator { get; private set; }

	public AudioClip swipeSound;
	public AudioClip collideSound;

	AudioSource asource;

	private void Awake() {
		if (instance) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	// Start is called before the first frame update
	void Start() {
		asource = GetComponent<AudioSource>();

		SceneManager.sceneLoaded += Bruh;

		LevelManager.instance.GenerateRound(10);
		StartRound();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartRound();
		}
	}

	void Bruh(Scene scene, LoadSceneMode mode) {
		print("RELOADING MAP");
		LevelManager.instance.RecreateSlidersFromMap();
		StartRound();
	}

	public void StartRound() {
		directionIndicator = Instantiate(dIndicatorPrefab);
		directionIndicator.SetActive(false);

		if (SetData != null)
			SetData();
	}

	public void RestartRound() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void HasSwiped() {
		if (OnHasSwiped != null)
			OnHasSwiped();
	}

	public void PlaySound(string type) {
		AudioClip sound = null;
		switch(type) {
			case "swipe":
				sound = swipeSound;
				break;
			case "collide":
				sound = collideSound;
				break;
			default:
				sound = collideSound;
				break;
		}
		asource.PlayOneShot(sound);
	}

}
