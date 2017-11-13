using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour {
	public int life;
	public Image hurtImage;

	float flashRate;
	bool topRegen;
	bool midRegen;
	bool regenRunning;
	bool damageRunning;
	bool flashRunning;
	Color hurtIndcate;
	// Use this for initialization
	void Start () {
		life = 80; 
		regenRunning = false;
		topRegen = false;
		midRegen = false;
		damageRunning = false;
		flashRate = 1f;
		hurtIndcate = new Color (1f, 0f, 0f, 0.2f);
		flashRunning = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("[") && damageRunning == false) {
			StartCoroutine (damage ());
		}
		if (Input.GetKey ("]")) {
			heal ();
		}
		if (hurtImage.color == Color.clear) {
			flashRunning = false;
		}
		if (life <= 0) {
			//remove this line before building, it refuses to build if any editor application code is present
			//replace it with loading the menu or end screen
			StopCoroutine (regen ());
			StopCoroutine (damage ());
			UnityEditor.EditorApplication.isPlaying = false;
		}
		if (life < 5) {
			midRegen = false;
			topRegen = false;
		}
		if ((life < 60 && life > 30) && topRegen == false) {
			topRegen = true;
			midRegen = false;
		}
		if ((life < 30 && life > 5) && midRegen == false) {
			topRegen = false;
			midRegen = true;
		}
		if (life == 30 && midRegen) {
			midRegen = false;
			topRegen = false;
			StopCoroutine (regen ());
		}
		if (life == 60 && topRegen) {
			midRegen = false;
			topRegen = false;
			StopCoroutine (regen ());
		}
		if (life > 80) {
			StopCoroutine (regen ());
			midRegen = false;
			topRegen = false;
		}
		if (topRegen || midRegen) {
			if (regenRunning == false) {
				StartCoroutine (regen ());
			}
		}

		if ((life < 30) && (flashRunning == false)) {
			hurtImage.color = hurtIndcate;
			flashRunning = true;
			flashRate = 5f;
		} else if ((life < 60) && (flashRunning == false)) {
			hurtImage.color = hurtIndcate;
			flashRunning = true;
			flashRate = 2f;
		}else {
			hurtImage.color = Color.Lerp (hurtImage.color, Color.clear, flashRate * Time.deltaTime);
		}
		if (hurtImage.color == Color.clear) {
			flashRunning = false;
		}
	}
	IEnumerator damage(){
		damageRunning = true;
		if (regenRunning) {
			StopCoroutine (regen ());
		}
		life -= 5;
		yield return new WaitForSeconds (3f);
		damageRunning = false;
	}

	void heal(){
		if (regenRunning) {
			StopCoroutine (regen ());
		}
		life = 80;
		midRegen = false;
		topRegen = false;
	}

	IEnumerator regen(){
		regenRunning = true;
		if (topRegen || midRegen) {
			++life;
		}
		yield return new WaitForSeconds (5f);
		regenRunning = false;
		StartCoroutine (regen());
	}
}
