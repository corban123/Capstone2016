﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TutorialScript : MonoBehaviour {
    Button forward;
    Button back;
    Button home;

    Animator tutorialAnimator;

    NewHUD hud;

    int index;

	// Use this for initialization
	void Start () {
        forward = GameObject.Find ("Forward").GetComponent<Button> () as Button;
        forward.onClick.AddListener (() => {
            forwardOnClick ();
        });

        back = GameObject.Find ("Back").GetComponent<Button> () as Button;
        back.onClick.AddListener (() => {
            backOnClick ();
        });

        home = GameObject.Find ("Home").GetComponent<Button> () as Button;
        home.onClick.AddListener (() => {
            homeOnClick ();
        });

        tutorialAnimator = GameObject.Find ("TutorialSlideshow").GetComponent<Animator> ();
        hud = GameObject.Find ("NetManager").GetComponent<NewHUD> ();

        index = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (index == 0) {
            back.interactable = false;
        } else {
            back.interactable = true;
        }
        if (index == 31) {
            forward.interactable = false;
        } else {
            forward.interactable = true;
        }
	}

    void forwardOnClick() {
        index++;
        print ("forward to " + index);
        tutorialAnimator.SetInteger ("index", index);
    }

    void backOnClick() {
        index--;
        print ("back to " + index);
        tutorialAnimator.SetInteger ("index", index);
    }

    void homeOnClick() {
        index = 0;
        hud.GoToMainScreen ();
    }
}
