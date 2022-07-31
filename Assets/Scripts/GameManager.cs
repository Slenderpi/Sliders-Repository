using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public delegate void BindableEvent();
	public static event BindableEvent OnHasSwiped;
	public static event BindableEvent SetData;

	public readonly static float MIN_DRAG_DISTANCE = 10f;
	public static float sliderVelocity = 12f;

	public GameObject dIndicatorPrefab;
	public GameObject directionIndicator { get; private set; }

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {
		//canSwipe = false;

		// Temporary
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

}
