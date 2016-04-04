using UnityEngine;
using System.Collections;

public class ShaderCode : MonoBehaviour {


	public Shader shade;
	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<Camera> ().RenderWithShader (shade, "TSF");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
