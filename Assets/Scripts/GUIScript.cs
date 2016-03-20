using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {
    public Texture2D m_CrosshairTex;
    Vector2 m_WindowSize;    //More like "last known window size".
    Rect m_CrosshairRect;


	// Use this for initialization
	void Start () {
        print (m_CrosshairTex.format);
        m_CrosshairTex.Resize(1, 1, TextureFormat.DXT5, true);
        m_CrosshairTex.Apply ();
        m_WindowSize = new Vector2(Screen.width, Screen.height);
        CalculateRect();
	}
	
	// Update is called once per frame
	void Update () {
        if(m_WindowSize.x != Screen.width || m_WindowSize.y != Screen.height)
        {
            CalculateRect();
        }
	}

    void CalculateRect()
    {
        m_WindowSize = new Vector2(Screen.width, Screen.height);
        m_CrosshairRect = new Rect( (m_WindowSize.x - m_CrosshairTex.width)/2.0f,
            (m_WindowSize.y - m_CrosshairTex.height)/2.0f,
            m_CrosshairTex.width, m_CrosshairTex.height);
    }

    void OnGUI() {
        GUI.DrawTexture (m_CrosshairRect, m_CrosshairTex);
    }
}
