using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour {

	public Sprite mainSprite;
	public Sprite hoverSprite;



	// Use this for initialization
	void Start () {
		
	}


	public void setCurrentImageHover() {
		Image image = gameObject.GetComponent<Image>();
		image.sprite = hoverSprite;
	}

	public void returnCurrentImageDefault(){
		Image image = gameObject.GetComponent<Image>();
		image.sprite = mainSprite;
	}
		

}
