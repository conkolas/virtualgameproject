using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {

	public Transform PlayerCamera;
	public Transform Portal;
	public Transform OtherPortal;

	private Transform _transform;
	private Vector3 _newPosition;
	private Vector3 _playerPortalPositionOffset;
	private float _portalAngleRotationDifference;
	private Quaternion _portalRotationalDifference;
	
	void Start () {
		_transform = GetComponent<Transform>();
	}
	
	void Update () {		
		// setting position offset
		_playerPortalPositionOffset = PlayerCamera.position - OtherPortal.position;
		_transform.position = Portal.position + _playerPortalPositionOffset;
		
		// setting rotational offset
		_portalAngleRotationDifference = Quaternion.Angle(Portal.rotation, OtherPortal.rotation);
		Debug.Log("_portalAngleRotationDifference " + _portalAngleRotationDifference);
		_portalRotationalDifference = Quaternion.AngleAxis(_portalAngleRotationDifference, Vector3.up);
		_newPosition = _portalRotationalDifference * -PlayerCamera.forward;
		_transform.rotation = Quaternion.LookRotation(_newPosition, Vector3.up);
	}
}
