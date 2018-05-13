using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginCanvas : MonoBehaviour {
    // =============================
    // IMAGE
	public RawImage rawImage;

    const int kFace = 0;
    const int kHide = 1;
    const int kNormal = 2;
    const int kOpenEye = 3;

	Texture2D face;
	Texture2D hide;
	const int maxNormal = 22;
	const int maxOpeneye = 21;

	Texture2D[] normal = new Texture2D[maxNormal];
	Texture2D[] openeye = new Texture2D[maxOpeneye];

	IEnumerator LoadAllPNG () {
		int index = 0;

		// face
		face = Resources.Load("Critter/face") as Texture2D;
		yield return face;

		// hide
		hide = Resources.Load("Critter/hide") as Texture2D;
		yield return hide;

		// normal
		for (index = 0; index < maxNormal; index++) {
			if (index >= 0 && index < 10) {
				normal[index] = Resources.Load("Critter/normal/0" + index + "normal") as Texture2D;
			} else if (index >= 10 && index < maxNormal) {
				normal[index] = Resources.Load("Critter/normal/" + index + "normal") as Texture2D;
            } else {
				int temp = maxNormal - 1;
				normal[index] = Resources.Load("Critter/normal/" + temp + "normal") as Texture2D;
            }
			yield return normal[index];
		}
      
		// openeye
		for (index = 0; index < maxOpeneye; index++) {
			if (index >= 0 && index < 10) {
                openeye[index] = Resources.Load("Critter/openeye/0" + index + "openeye") as Texture2D;
			} else if (index >= 10 && index < maxOpeneye) {
				openeye[index] = Resources.Load("Critter/openeye/" + index + "openeye") as Texture2D;
            } else {
				int temp = maxOpeneye - 1;
				openeye[index] = Resources.Load("Critter/openeye/" + temp + "openeye") as Texture2D;
            }
			yield return openeye[index];
        }

		yield return null;
	}
    
    IEnumerator LoadPNG(int[] typeIndex)
    {
		int type = typeIndex[0];
		int index = typeIndex[1];

		Debug.Log("LoadPNG:" + type + ", " + index);
        
		Texture2D texture = null;

		/*
		//### load one
		switch (type) {
			case kFace:
				texture = Resources.Load("Critter/face") as Texture2D;
				break;
			case kHide:
				texture = Resources.Load("Critter/hide") as Texture2D;
				break;
			case kNormal: 
				if (index >= 0 && index < 10) {
                    texture = Resources.Load("Critter/normal/0" + index + "normal") as Texture2D;
				} else if (index >= 10 && index <= 21) {
                    texture = Resources.Load("Critter/normal/" + index + "normal") as Texture2D;
                } else {
                    texture = Resources.Load("Critter/normal/21normal") as Texture2D;
                }
				break;
			case kOpenEye:
				if (index >= 0 && index < 10) {
					texture = Resources.Load("Critter/openeye/0" + index + "openeye") as Texture2D;
                } else if (index >= 10 && index <= 20) {
					texture = Resources.Load("Critter/openeye/" + index + "openeye") as Texture2D;
                } else {
					texture = Resources.Load("Critter/openeye/20openeye") as Texture2D;
                }
				break;
		}

		if (texture == null) {
			texture = Resources.Load("Critter/face") as Texture2D;
		}
        */

		switch (type)
        {
            case kFace:
				texture = face;
                break;
            case kHide:
				texture = hide;
                break;
            case kNormal:
				if (index >= 0 && index < maxNormal) {
					texture = normal[index];
				} else if (index >= maxNormal) {
					texture = normal[maxNormal-1];
				} else {
					texture = normal[0];
				}
                break;
            case kOpenEye:
				if (index >= 0 && index < maxOpeneye) {
					texture = openeye[index];
				} else if (index >= maxOpeneye) {
					texture = openeye[maxOpeneye-1];
				} else {
					texture = openeye[0];
				}
                break;
        }

		if (texture == null) {
			texture = face;
		}

        yield return texture;
        rawImage.texture = texture;
        rawImage.SetNativeSize();
    }
    
    // =============================
    // INPUT
	public InputField inputField;
    public void OnValueChanged() {
        Debug.Log("OnValueChanged:" + inputField.text);
    }
    public void OnEndEdit() {     
        Debug.Log("OnEndEdit:" + inputField.text);  // 入力された文字を表示
    }

    public InputField passwordField;
    public void OnValueChangedPassword() {
        Debug.Log("OnValueChangedPassword:" + passwordField.text);
    }
    public void OnEndEditPassword() {
        Debug.Log("OnEndEditPassword:" + passwordField.text);
    }
   
	// =============================
    // LOGIN   
	public Button button;
	public void onClickButton() {
        Debug.Log("LoginCanvas onClickButton()");
		SceneManager.LoadScene("MainScene");
    }

    // =============================
   
	// Use this for initialization
	void Start () {
		Debug.Log("LoginCanvas Start()");
		Screen.orientation = ScreenOrientation.Portrait;

		StartCoroutine ("LoadAllPNG");  
        
		StartCoroutine ("LoadPNG", new int[] { kFace, 0 });  
	}

	float timeOut = 0.001f;
    private float timeElapsed;

	int lastFocus = -1;
	int currFocus = kFocusOther;

	const int kFocusUser = 0;
	const int kFocusPassword = 1;
	const int kFocusOther = 2;

	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeOut) {
			// Debug.Log("currentFocus:" + currFocus);

            if (this.inputField.isFocused) {
				currFocus = kFocusUser;
            } else if (this.passwordField.isFocused) {
				currFocus = kFocusPassword;
			} else {
				currFocus = kFocusOther;
			}
       
			switch (currFocus) {
				case kFocusUser:
					// normal or openeye
					// Debug.Log("SHOW NORMAL OR OPENEYE");
					int len = this.inputField.text.Length;
					if (len >= 4) {
						StartCoroutine("LoadPNG", new int[] { kOpenEye, len });
					} else {
						StartCoroutine("LoadPNG", new int[] { kNormal, len });
					}
					break;
				case kFocusPassword:
					// Debug.Log("SHOW HIDE");
					StartCoroutine ("LoadPNG", new int[] { kHide, 0 });  
					break;
				default:
					// Debug.Log("SHOW FACE");
					StartCoroutine ("LoadPNG", new int[] { kFace, 0 });  
					break;
			}

            // =============
            timeElapsed = 0.0f;
        }
	}   
}
