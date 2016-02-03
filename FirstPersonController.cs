using UnityEngine;
using System.Collections;

public class FirstPersonController : MyController {

	public void RotateCharacter (float verticalRotation) {
		Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
	}

	public void MoveCharacter (CharacterController characterController, Vector3 speed) {
		characterController.Move (speed);
	}
}
