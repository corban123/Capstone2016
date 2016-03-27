using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIScript : MonoBehaviour {
    public Texture2D m_CrosshairTex;
    Vector2 m_WindowSize;
    Rect m_CrosshairRect;

    BoardScript boardScript;
    Image elementHeldImage;

    Image quarkMeter;
    readonly int quarkMeterWidth = 10;
    readonly int quarkMeterMax = 150;
    readonly int quarkMeterMin = 0;
    readonly int quarkSize = 10;
    int quarkMeterHeight = 30;

    private float delay = 1.633f;

    Animator elementPickedUpAnimator;
    Image elementPickedUpImage;
    float elementPickUpStartTime;
    bool animatingElementPickUp;

    Animator youScoredAnimator;
    Image youScoredImage;
    float youScoredStartTime;
    bool animatingYouScored;

    Animator glowGaugeAnimator;
    Image glowGaugeImage;
    bool animatingGlowGauge;

	// Use this for initialization
	void Start () {
        // Set up the cross hair
        m_CrosshairTex.Apply ();
        m_WindowSize = new Vector2(Screen.width, Screen.height);
        CalculateRect();

        boardScript = GetComponent<BoardScript> ();

        quarkMeter = GameObject.Find ("QuarkMeter").GetComponent<Image>();
        elementHeldImage = GameObject.Find ("ElementHeld").GetComponent<Image>();
        elementPickedUpImage = GameObject.Find ("ElementPickedUp").GetComponent<Image> ();
        youScoredImage = GameObject.Find ("YouScored").GetComponent<Image> ();
        glowGaugeImage = GameObject.Find ("GaugeGlow").GetComponent<Image> ();

        elementPickedUpAnimator = GameObject.Find ("ElementPickedUp").GetComponent<Animator> ();
        youScoredAnimator = GameObject.Find ("YouScored").GetComponent<Animator> ();
        glowGaugeAnimator = GameObject.Find ("GaugeGlow").GetComponent<Animator> ();

        setDefaults ();
	}

    /**
     * GUI defaults: no element, updated quarks, deactivate pickup texts
     */
    void setDefaults() {
        updateQuarkMeter (3);
        DeleteElementUI ();
        disableElementPickedUp ();
        disableYouScored ();
        disableGaugeGlow ();
    }
	
	/**
     * 
     */
	void Update () {
        // If the screen size changed, calculate it again.
        if(m_WindowSize.x != Screen.width || m_WindowSize.y != Screen.height)
        {
            CalculateRect();
        }

        if (animatingElementPickUp && Time.time - elementPickUpStartTime > delay) {
            disableElementPickedUp ();
        }
        if (animatingYouScored && Time.time - youScoredStartTime > delay) {
            disableYouScored ();
        }

        // TODO (@paige): figure out why sometimes quark meter isn't found in start.
        if (quarkMeter == null) {
            quarkMeter = GameObject.Find ("QuarkMeter").GetComponent<Image> ();
        }
	}

    /**
     * Calculate the size of the screen
     */
    void CalculateRect()
    {
        m_WindowSize = new Vector2(Screen.width, Screen.height);
        m_CrosshairRect = new Rect( (m_WindowSize.x - m_CrosshairTex.width)/2.0f,
            (m_WindowSize.y - m_CrosshairTex.height)/2.0f,
            m_CrosshairTex.width, m_CrosshairTex.height);
    }

    /**
     * Draw the crosshair in the middle of the GUI
     */
    void OnGUI() {
        GUI.DrawTexture (m_CrosshairRect, m_CrosshairTex);
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
    }

    /**
     * Set the held element to show in the circle at the bottom of the gauge.
     */
    public void SetElementUI(int heldElement) {
        print ("adding element " + heldElement);
        Sprite elemSprite = boardScript.GetColorSprite (heldElement);
        elementHeldImage.sprite = elemSprite;
        elementHeldImage.color = Color.white;
    }

    /**
     * Remove the held element from the circle at the bottom of the gauge.
     */
    public void DeleteElementUI() {
        elementHeldImage.sprite = null;
        elementHeldImage.color = Color.black;
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
        animatingGlowGauge = true;
        glowGaugeImage.enabled = true;
        glowGaugeAnimator.SetBool ("animating", true);
    }

    public void disableGaugeGlow() {
        animatingGlowGauge = false;
        glowGaugeImage.enabled = false;
        glowGaugeAnimator.SetBool ("animating", false);
    }
}
