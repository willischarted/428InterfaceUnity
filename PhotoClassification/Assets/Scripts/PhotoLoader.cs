using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEngine.UI;
using System.IO;



public class PhotoLoader : MonoBehaviour {

	// List of images from folder
	public List<Texture2D> images;

	// Acquired files and directories
	// public visibility for testing only!!
	public string[] files;
	public string[] directories;
	string pathPreFix;

	public Text category1;
	public Text category2;
	public Text category3;
	public Text timer;
//	public string username;
	public string path;

	// UI specific stuffz
	public Image focusImage;
	private Animator anim;

	public int count;
	public Sprite finalImage;

	float elapsedTime;
	bool isFinished;


	// Use this for initialization
	void Start () {
		elapsedTime = 0f;
		isFinished = false;

		count = 0;
		images = new List<Texture2D>();

		// Set up animator
		anim = focusImage.GetComponent<Animator>();
		if (anim == null)
			Debug.Log("Could not find the Animator component");

		//Change this to change pictures folder
		//path is dynamic and set by the user.
		//old code below
		/*string path =    @"/Users/"
			+ username  
			//HisProdigalSon	
			+ "/Desktop/pics";
			*/

		pathPreFix = @"file://";

		files = System.IO.Directory.GetFiles(path, "*.jpg");

		// Set up the Directories of classification
		directories = System.IO.Directory.GetDirectories(path);
		category1.text = Path.GetFileName(directories[0]);
		category2.text = Path.GetFileName(directories[1]);
		category3.text = Path.GetFileName(directories[2]);

		// Get all pictures into texture list
		StartCoroutine(LoadImages());

		//load first image on the focus area: Load texture, create sprite, attatch sprite
		loadNextPicture();
	}


	void Update () {
		//print(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
		if(!isFinished)
			elapsedTime += Time.deltaTime;
		//Debug.Log(elapsedTime);
	}

	private IEnumerator LoadImages(){
		// Loop though all files we acquired in folder
		foreach(string textureString in files){
			
			string pathTemp = pathPreFix + textureString;
			//Debug.Log(pathTemp);
			// Deal with urls, get the texture from them
			WWW www = new WWW(pathTemp);
			images.Add(www.texture);

		}
		yield return null;
	}


	public void playMovementAnimation(string triggername) {
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Position")) {
			anim.SetTrigger(triggername);
			Debug.Log("setting animation Right");
			StartCoroutine(preLoad());
		}
		else
			Debug.Log("Not in right state to set");
		
	}

	public IEnumerator preLoad() {
		// wait while we are still in base 
		while (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Position"))
			yield return new WaitForSeconds(0.01f);
		// wait until we have finished offscreen animation
		while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
			yield return new WaitForSeconds(0.01f);
		if (count >= images.Count) 
			finishClassification();
		else
			loadNextPicture();
		playResetAnimation();
	}

	public void playResetAnimation() {

		anim.SetTrigger("Reset Position Trigger");

		Debug.Log("setting animation Reset");
	}

	public void finishClassification() {
		focusImage.sprite = finalImage;
		if (!isFinished) {
			timer.text = elapsedTime.ToString();
			isFinished = true;
		}
	}

	public void moveToFolder(string target){
		// Dont wanna sort accidently or before image loads
		if (count < images.Count){
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Position")) {
				if (target == "Left") {
					File.Move(files[count], directories[0] + "/" + Path.GetFileName(files[count]));
				}
				else if (target == "Right"){
					File.Move(files[count], directories[1] + "/" + Path.GetFileName(files[count]));
				}
				else if (target == "Up"){
					File.Move(files[count], directories[2] + "/" + Path.GetFileName(files[count]));
				}
				// Now delete that file from the list
				count++;
			}
		}
	}
		
	public void loadNextPicture() {
		Texture2D tex =  images[count];
		Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f,    //last two parts added for performance
									// unity had to figure out texture type? meant 1+ second of computation
			tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect);
		focusImage.GetComponent<Image>().sprite = mySprite;
	}


}

