using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public delegate void BindableEvent();
	public static event BindableEvent OnHasSwiped;
	public static event BindableEvent SetData;

	public readonly static float MIN_DRAG_DISTANCE = 10f;
	public static float sliderVelocity = 8f;

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

		LevelManager.instance.GenerateRound(2);
		StartRound();
	}

	// Update is called once per frame
	void Update() {

	}

	public void StartRound() {
		//canSwipe = true;
		directionIndicator = Instantiate(dIndicatorPrefab);
		directionIndicator.SetActive(false);

		if (SetData != null)
			SetData();
		//Slider[] sliders = Resources.FindObjectsOfTypeAll<Slider>();
		//foreach (Slider s in sliders) {
		//	s.gameObject.SetActive(true);
		//}
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
