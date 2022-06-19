using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoWorldUnity3D;

public class Login : MonoBehaviour {
	public UnityEngine.Canvas loginCanvas;
	public UnityEngine.UI.InputField usernameInput;
	public UnityEngine.UI.InputField passwordInput;
	public UnityEngine.UI.Button registerButton;
	public UnityEngine.UI.Button loginButton;
	public UnityEngine.UI.Text messageText;
	public GameObject UICanvas;
	public GameObject CastCanvas;
	public GameObject MonsterCanvas;
	
	// Use this for initialization
	void Start () {
		messageText.text = "";
		usernameInput.ActivateInputField ();
        showMessage("Connecting Server ...");
        UICanvas = GameObject.Find("UICanvas");
        CastCanvas = UICanvas.gameObject.transform.Find("CastCanvas").gameObject;
        CastCanvas.SetActive(false);
        MonsterCanvas = UICanvas.gameObject.transform.Find("MonsterCanvas").gameObject;
        MonsterCanvas.SetActive(false);
        DontDestroyOnLoad(UICanvas);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnRegister() {
		Debug.Log ("Register " + usernameInput.text + ", " + passwordInput.text);
		string username = usernameInput.text.Trim ();
		string password = passwordInput.text;
		if (username == "") {
			showMessage ("Username is empty!");
			return ;
		}
		if (password == "") {
			showMessage ("Password is empty!");
			return;
		}
		Debug.Log(GoWorld.ClientOwner);
		GoWorld.ClientOwner.CallServer ("Register", username, password);
	}

	public void OnLogin() {
		Debug.Log ("Login " + usernameInput.text + ", " + passwordInput.text);
		string username = usernameInput.text.Trim ();
		string password = passwordInput.text;
		if (username == "") {
			showMessage ("Username is empty!");
			return ;
		}
		if (password == "") {
			showMessage ("Password is empty!");
			return;
		}

		GoWorld.ClientOwner.CallServer ("Login", username, password);
	}

	public void showMessage(string msg) {
		messageText.text = msg;
	}
}
