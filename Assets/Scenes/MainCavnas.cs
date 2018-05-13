using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainCavnas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("MainCanas Start()");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onClickButton () {
		Debug.Log("MainCanvas OnClickButton()");
		SceneManager.LoadScene("LoginScene");
	}
}
