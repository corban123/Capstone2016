using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player_SyncRotation : NetworkBehaviour {

	[SyncVar (hook = "OnPlayerRotSynced")] private float syncPlayerRotation;
	[SyncVar (hook = "OnCamRotSynced")] private float syncCamRotation;

	[SerializeField] private Transform playerTransform;
	[SerializeField] private Transform camTransform;
	private float lerpRate = 20;

	private float lastPlayerRot;
	private float lastCamRot;
	private float threshold = 0.3f;

	private List<float> syncPlayerRotList = new List<float>();
	private List<float> syncCamRotList = new List<float>();
	private float closeEnough = 0.4f;
	[SerializeField] private bool useHistoricalInterpolation;

	// Use this for initialization
	void Start () {
	
	}

	void Update ()
	{
		LerpRotations();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		TransmitRotations();

	}

	void LerpRotations ()
	{
		if(!isLocalPlayer)
		{
			if(useHistoricalInterpolation)
			{
				HistoricalInterpolation();
			}
			else
			{
				OrdinaryLerping();
			}
		}
	}

	void HistoricalInterpolation()
	{
		if(syncPlayerRotList.Count > 0)
		{
			LerpPlayerRotation(syncPlayerRotList[0]);

			if(Mathf.Abs(playerTransform.localEulerAngles.y - syncPlayerRotList[0]) < closeEnough)
			{
				syncPlayerRotList.RemoveAt(0);
			}

			//Debug.Log(syncPlayerRotList.Count.ToString() + " syncPlayerRotList Count");
		}

		if(syncCamRotList.Count > 0)
		{
			LerpCamRot(syncCamRotList[0]);

			if(Mathf.Abs(camTransform.localEulerAngles.x - syncCamRotList[0]) < closeEnough)
			{
				syncCamRotList.RemoveAt(0);
			}

			//Debug.Log(syncCamRotList.Count.ToString() + " syncCamRotList Count");
		}
	}

	void OrdinaryLerping ()
	{
		LerpPlayerRotation(syncPlayerRotation);
		LerpCamRot(syncCamRotation);
	}

	void LerpPlayerRotation(float rotAngle)
	{
		Vector3 playerNewRot = new Vector3(0, rotAngle, 0);
		playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.Euler(playerNewRot), lerpRate * Time.deltaTime);
	}

	void LerpCamRot (float rotAngle)
	{
		Vector3 camNewRot = new Vector3(rotAngle, 0, 0);
		camTransform.localRotation = Quaternion.Lerp(camTransform.localRotation, Quaternion.Euler(camNewRot), lerpRate * Time.deltaTime);
	}

	[Command]
	void CmdProvideRotationsToServer (float playerRot, float camRot)
	{
		syncPlayerRotation = playerRot;
		syncCamRotation = camRot;
	}

	[Client]
	void TransmitRotations()
	{
		if(isLocalPlayer)
		{
			if(CheckIfBeyondThreshold(playerTransform.localEulerAngles.y, lastPlayerRot) || CheckIfBeyondThreshold(camTransform.localEulerAngles.x, lastCamRot))
			{
				lastPlayerRot = playerTransform.localEulerAngles.y;
				lastCamRot = camTransform.localEulerAngles.x;
				CmdProvideRotationsToServer(lastPlayerRot, lastCamRot);
			}
		}
	}

	bool CheckIfBeyondThreshold (float rot1, float rot2)
	{
		if(Mathf.Abs(rot1-rot2) > threshold)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	[Client]
	void OnPlayerRotSynced (float latestPlayerRot)
	{
		syncPlayerRotation = latestPlayerRot;
		syncPlayerRotList.Add(syncPlayerRotation);
	}

	[Client]
	void OnCamRotSynced (float latestCamRot)
	{
		syncCamRotation = latestCamRot;
		syncCamRotList.Add(syncCamRotation);
	}
}















