using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour {

	Camera camera;
	GameObject directionIndicator;

	Rigidbody rb;

	float velocity = 0;

	Vector2 mouseDownPos;
	Vector2 mouseUpPos;

	bool canSwipe = true;
	bool isSliding = false;

	GameObject twinSlider;

	// Start is called before the first frame update
	void Start() {
		camera = Camera.main;
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update() {

	}

	private void OnEnable() {
		GameManager.OnHasSwiped += OnHasSwiped;
		GameManager.SetData += OnSetData;
	}

	private void OnDisable() {
		GameManager.OnHasSwiped -= OnHasSwiped;
		GameManager.SetData -= OnSetData;
	}

	private void OnMouseDown() {
		if (!canSwipe) return;
		mouseDownPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	}

	private void OnMouseUp() {
		if (!canSwipe) return;
		mouseUpPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		directionIndicator.SetActive(false);
		OnSwipe();
	}

	private void OnMouseDrag() {
		if (!canSwipe) return;
		Vector2 direction = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mouseDownPos;
		if (direction.magnitude <= GameManager.MIN_DRAG_DISTANCE) {
			directionIndicator.SetActive(false);
			return;
		} else {
			directionIndicator.SetActive(true);
			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
				directionIndicator.transform.position = transform.position + new Vector3(direction.x, 0, 0).normalized * 1f;
				directionIndicator.transform.eulerAngles = new Vector3(0, 0, 90);
			} else {
				directionIndicator.transform.position = transform.position + new Vector3(0, 0, direction.y).normalized * 1f;
				directionIndicator.transform.eulerAngles = new Vector3(0, 90, 90);

			}
		}
	}

	//private void OnTriggerStay(Collider other) {
	//	if (isSliding && other.CompareTag("Slider")) {
	//		// The sliding slider will handle collision logic
	//		GameObject sg1 = other.gameObject;
	//		Slider ss1 = sg1.GetComponent<Slider>();
	//		if (ss1.isSliding)
	//			return;
	//		GameObject sg2 = Instantiate(gameObject, sg1.transform.position, sg1.transform.rotation);
	//		Slider ss2 = sg2.GetComponent<Slider>();

	//		ss1.SendSlider(new Vector3(rb.velocity.z, 0, rb.velocity.x));
	//		ss2.SendSlider(new Vector3(-rb.velocity.z, 0, -rb.velocity.x));

	//		Destroy(gameObject);
	//	}
	//}

	private void OnTriggerEnter(Collider other) {
		if (isSliding && other.CompareTag("Slider")) {
			// The sliding slider will handle collision logic
			GameObject sg1 = other.gameObject;
			Slider ss1 = sg1.GetComponent<Slider>();
			if (ss1.isSliding)
				return;
			GameObject sg2 = Instantiate(gameObject, sg1.transform.position, sg1.transform.rotation);
			Slider ss2 = sg2.GetComponent<Slider>();

			ss1.SendSlider(new Vector3(rb.velocity.z, 0, rb.velocity.x));
			ss2.SendSlider(new Vector3(-rb.velocity.z, 0, -rb.velocity.x));

			GameManager.instance.PlaySound("collide");

			Destroy(gameObject);
		}
	}

	void OnSwipe() {
		Vector2 direction = mouseUpPos - mouseDownPos;
		if (direction.magnitude <= GameManager.MIN_DRAG_DISTANCE)
			return;
		GameManager.HasSwiped();
		GameManager.instance.PlaySound("swipe");
		isSliding = true;
		rb.velocity = (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? new Vector3(direction.x, 0, 0) : new Vector3(0, 0, direction.y)).normalized * velocity;
	}

	void OnSetData() {
		directionIndicator = GameManager.instance.directionIndicator;
		velocity = GameManager.sliderVelocity;
	}

	void OnHasSwiped() {
		canSwipe = false;
	}

	void SendSlider(Vector3 velocity) {
		canSwipe = false;
		isSliding = true;
		GetComponent<Rigidbody>().velocity = velocity;
	}

}
