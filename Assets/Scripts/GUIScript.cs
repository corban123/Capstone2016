using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GUIScript : NetworkBehaviour {
    public GameObject BlackHoleSprite;
    public GameObject BlackOutSprite;
    public GameObject FreezeSprite;
    public GameObject AtomBombSprite;
    public Canvas canvas;

    BoardScript boardScript;
    Image elementHeldImage;

    Image powerUpImage;

    Image youWonImage;
    Image youLostImage;

    Image quarkMeter;
    readonly int quarkMeterWidth = 10;
    readonly int quarkMeterMax = 310;
    readonly int quarkMeterMin = 0;
    readonly int quarkSize = 10;
    int quarkMeterHeight = 30;

    private float delay = 1.633f;
    private float fadeTransitionTime = 3.0f;
    private float fadeBlindedTime = 6.0f;

    Image blackout;
    Image freeze;

    Animator elementPickedUpAnimator;
    Image elementPickedUpImage;
    float elementPickUpStartTime;
    bool animatingElementPickUp;

    Animator youScoredAnimator;
    Image youScoredImage;
    float youScoredStartTime;
    bool animatingYouScored;

    Animator enemyScoredAnimator;
    Image enemyScoredImage;
    float enemyScoredStartTime;
    bool animatingEnemyScored;

    Animator glowGaugeAnimator;
    Image glowGaugeImage;

    Text numQuarksText;

    GameObject powerUpObject;

	// Use this for initialization
	void Start () {
        boardScript = GetComponent<BoardScript> ();

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        quarkMeter = GameObject.Find ("QuarkMeter").GetComponent<Image>();
        elementHeldImage = GameObject.Find ("ElementHeld").GetComponent<Image>();
        elementPickedUpImage = GameObject.Find ("ElementPickedUp").GetComponent<Image> ();
        powerUpImage = GameObject.Find ("PowerUpImage").GetComponent<Image> ();
        youScoredImage = GameObject.Find ("YouScored").GetComponent<Image> ();
        enemyScoredImage = GameObject.Find ("EnemyScored").GetComponent<Image> ();
        glowGaugeImage = GameObject.Find ("GaugeGlow").GetComponent<Image> ();
        youWonImage = GameObject.Find ("YouWon").GetComponent<Image> ();
        youLostImage = GameObject.Find ("YouLost").GetComponent<Image> ();

        elementPickedUpAnimator = GameObject.Find ("ElementPickedUp").GetComponent<Animator> ();
        youScoredAnimator = GameObject.Find ("YouScored").GetComponent<Animator> ();
        enemyScoredAnimator = GameObject.Find ("EnemyScored").GetComponent<Animator> ();
        glowGaugeAnimator = GameObject.Find ("GaugeGlow").GetComponent<Animator> ();

        numQuarksText = GameObject.Find ("NumQuarksText").GetComponent<Text> ();

        blackout = GameObject.Find ("Blackout").GetComponent<Image> ();
        freeze = GameObject.Find ("Freeze").GetComponent<Image> ();

        setDefaults ();
	}

    /**
     * GUI defaults: no element, updated quarks, deactivate pickup texts
     */
    void setDefaults() {
        updateQuarkMeter (3);
        DeleteElementUI ();
        disableYouWon ();
        disableElementPickedUp ();
        disableYouScored ();
        disableEnemyScored ();
        disableGaugeGlow ();
        blackout.canvasRenderer.SetAlpha( 0.01f );
        freeze.canvasRenderer.SetAlpha( 0.01f );
    }
	
	/**
     * 
     */
	void Update () {
        if (animatingElementPickUp && Time.time - elementPickUpStartTime > delay) {
            disableElementPickedUp ();
        }
        if (animatingYouScored && Time.time - youScoredStartTime > delay) {
            disableYouScored ();
        }
        if (animatingEnemyScored && Time.time - enemyScoredStartTime > delay) {
            disableEnemyScored ();
        }

        // TODO (@paige): figure out why sometimes quark meter isn't found in start.
        // Because it gets called in OnChange for health even before the UI element is found.
        if (quarkMeter == null) {
            quarkMeter = GameObject.Find ("QuarkMeter").GetComponent<Image> ();
        }
	}

    /**
     * Changes the quark meter to match the number of quarks held by the player.
     * Caps quark meter between quarkMeterMin and quarkMeterMax.
     * Each quark is 10 pixels on the meter.
     */
    public void updateQuarkMeter(int numQuarks) {

        quarkMeterHeight = numQuarks * quarkSize;

        if (quarkMeterHeight > quarkMeterMax)
            quarkMeterHeight = quarkMeterMax;
        else if (quarkMeterHeight < quarkMeterMin)
            quarkMeterHeight = quarkMeterMin;

        quarkMeter.rectTransform.sizeDelta = new Vector2 (quarkMeterWidth, quarkMeterHeight);
        numQuarksText.text = "" + numQuarks;
    }
        

    /**
     * Set the held element to show in the circle at the bottom of the gauge.
     */
    public void SetElementUI(int heldElement) {
        Sprite elemSprite = boardScript.GetColorSprite (heldElement);
        elementHeldImage.sprite = elemSprite;
        elementHeldImage.color = Color.white;
        SetPowerUpUI (heldElement);
    }

    public void SetPowerUpUI(int heldElement) {
        powerUpObject = GetPowerUpObject (heldElement);
        powerUpObject = Instantiate (powerUpObject) as GameObject;
        powerUpObject.transform.SetParent (powerUpImage.transform, false);
    }

    /**
     * Remove the held element from the circle at the bottom of the gauge.
     */
    public void DeleteElementUI() {
        elementHeldImage.sprite = null;
        elementHeldImage.color = Color.black;
        DeletePowerUpUI ();
    }

    public void DeletePowerUpUI() {
        Destroy(powerUpObject);
    }

    public void enableYouWon() {
        youWonImage.enabled = true;

        GameObject otherPlayer;
        if (gameObject.name.Contains("1"))
            otherPlayer = GameObject.Find("Player 2");
        else 
            otherPlayer = GameObject.Find("Player 1");

        NetworkInstanceId id = otherPlayer.GetComponent<NetworkIdentity>().netId;

        if (isServer)
            RpcYouLost (id);
        else
            CmdYouLost (id);
    }

    [Command]
    public void CmdYouLost(NetworkInstanceId id) {
        GameObject player = NetworkServer.FindLocalObject (id);
        GUIScript gui = player.GetComponent<GUIScript> ();
        try {
            gui.enableYouLost();
        } catch (NullReferenceException e) {
            print ("error: " + e);
        }
    }

    [ClientRpc]
    public void RpcYouLost (NetworkInstanceId id) {
        GameObject player = ClientScene.FindLocalObject (id);
        GUIScript gui = player.GetComponent<GUIScript> ();
        try {
            gui.enableYouLost();
        } catch (NullReferenceException e) {
            print ("you lost: " + e);
        }
    }  

    public void disableYouWon() {
        youWonImage.enabled = false;
    }

    public void enableYouLost() {
        youLostImage.enabled = true;
    }

    public void disableYouLost() {
        youLostImage.enabled = false;
    }

    public void disableElementPickedUp() {
        animatingElementPickUp = false;
        elementPickedUpImage.enabled = false;
        elementPickedUpAnimator.SetBool ("animating", false);
    }

    public void enableElementPickedUp() {
        animatingElementPickUp = true;
        elementPickedUpImage.enabled = true;
        elementPickedUpAnimator.SetBool ("animating", true);
        elementPickUpStartTime = Time.time;
    }

    public void disableYouScored() {
        animatingYouScored = false;
        youScoredImage.enabled = false;
        youScoredAnimator.SetBool ("animating", false);
    }

    public void enableYouScored() {
        animatingYouScored = true;
        youScoredImage.enabled = true;
        youScoredAnimator.SetBool ("animating", true);
        youScoredStartTime = Time.time;
    }

    public void enableGaugeGlow() {
        glowGaugeImage.enabled = true;
        glowGaugeAnimator.SetBool ("animating", true);
    }

    public void disableGaugeGlow() {
        glowGaugeImage.enabled = false;
        glowGaugeAnimator.SetBool ("animating", false);
    }

    public void disableEnemyScored() {
        animatingEnemyScored = false;
        enemyScoredImage.enabled = false;
        enemyScoredAnimator.SetBool ("animating", false);
    }

    public void enableEnemyScored() {
        animatingEnemyScored = true;
        enemyScoredImage.enabled = true;
        enemyScoredAnimator.SetBool ("animating", true);
        enemyScoredStartTime = Time.time;
    }

    public void freezeUI() {
        StartCoroutine (freezeCoroutine());
    }


    IEnumerator freezeCoroutine() {
        freeze.CrossFadeAlpha (1.0f, fadeTransitionTime, false);
        yield return new WaitForSeconds(fadeBlindedTime);
        freeze.CrossFadeAlpha (0.0f, fadeTransitionTime, false);
    }

    public void blackOutUI() {
        StartCoroutine (blackOutCoroutine());
    }

    IEnumerator blackOutCoroutine() {
        blackout.CrossFadeAlpha (1.0f, fadeTransitionTime, false);
        yield return new WaitForSeconds(fadeBlindedTime);
        blackout.CrossFadeAlpha (0.0f, fadeTransitionTime, false);
    }

    private GameObject GetPowerUpObject(int elementID) {
        if (elementID >= 0 && elementID <= 3) {
            return BlackHoleSprite;
        } else if (elementID >= 4 && elementID <= 7) {
            return FreezeSprite;
        } else if (elementID >= 8 && elementID <= 11) {
            return AtomBombSprite;
        } else if (elementID >= 12 && elementID <= 15) {
            return BlackOutSprite;
        } else {
            print ("element number is incorrect");
        }
        return new GameObject ();
    }

    public void disableCanvas() {
        canvas.enabled = false;
    }

    public void enableCanvas() {
        canvas.enabled = true;
    }
        
}
