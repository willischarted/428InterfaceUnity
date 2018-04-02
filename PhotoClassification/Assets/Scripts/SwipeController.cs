using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler {

	private Vector3 basePostion;
	public GameObject go;
	PhotoLoader p;
	public int swipeSpeed;

	bool swipeRegisteredUp;
	bool swipeRegisteredLeft;
	bool swipeRegisteredRight;


	// Use this for initialization
	void Start () {
		p = go.GetComponent<PhotoLoader>();
		swipeRegisteredUp = false;
		swipeRegisteredLeft = false;
		swipeRegisteredRight = false;

	}
		
	// Update is called once per frame
	void Update () {
		//Debug.Log(Input.mousePosition.);
		//Debug.Log(go.transform.position);

	}


	// Collision detection for swiping
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log(other.gameObject.name);

		if (other.gameObject.name == "LeftArea") {
			setFalse();
			swipeRegisteredLeft = true;
			//gameObject.GetComponent<Rigidbody2D>().AddForce(transform.);
		}
		else if (other.gameObject.name == "TopArea") {
			setFalse();
			swipeRegisteredUp = true;
			//gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 1);
		}
		else if (other.gameObject.name == "RightArea") {
			setFalse();
			swipeRegisteredRight = true;
			//gameObject.GetComponent<Rigidbody2D>().AddForce(10,0);
		}


	}



	public void OnBeginDrag (PointerEventData eventData)
	{	


		this.GetComponent<Animator>().enabled = false;
		basePostion = this.transform.position;

	}


	public void OnDrag(PointerEventData data)
	{

		this.transform.position = Input.mousePosition;	

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (swipeRegisteredRight == false && swipeRegisteredLeft == false 
			&& swipeRegisteredUp == false){

			GetComponent<Animator>().enabled = true;
			transform.position = basePostion;
		}
		else
			StartCoroutine(moveAndReset());

	}

	public IEnumerator moveAndReset() {
		if (swipeRegisteredLeft == true) {
			GetComponent<Rigidbody2D>().AddForce(-transform.right*swipeSpeed);
			p.moveToFolder("Left");
		}
		if (swipeRegisteredUp == true) {
			GetComponent<Rigidbody2D>().AddForce(transform.up * swipeSpeed);
			p.moveToFolder("Up");
		}
		if (swipeRegisteredRight == true) {
			GetComponent<Rigidbody2D>().AddForce(transform.right * swipeSpeed);
			p.moveToFolder("Right");
		}

		//do other stuff here 
		//wait until its off screen
		yield return new WaitForSeconds(0.5f);
		//set back to false
		setFalse();
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().angularVelocity = 0;

		GetComponent<Animator>().enabled = true;
		if (p.count >= p.images.Count)
			p.finishClassification();
		else
			p.loadNextPicture();

		p.playResetAnimation();
	}


	void setFalse() {
		swipeRegisteredRight = false;
		swipeRegisteredLeft = false;
		swipeRegisteredUp = false;
	}



}
