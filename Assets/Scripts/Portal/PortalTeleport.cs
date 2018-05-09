using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour {

	public event Action<PortalTeleport, Transform> OnTeleportEnter = delegate { };

	public Transform Player;
	public Transform Destination;
	
	private bool _isTriggered = false;
	private float _rotationDifference;
	private Vector3 _distance;
	private Vector3 _positionOffset;
	private Transform _transform;

	private void Start() {
		_transform = transform;
	}
	
	void Update () {
		if (_isTriggered) {
			_distance = Player.position - _transform.position;

			// check if moved across collider
			if (Vector3.Dot(_transform.up, _distance) < 0.0f) {
				// flip rotation and set new position
				_rotationDifference = -Quaternion.Angle(_transform.rotation, Destination.rotation);
				_rotationDifference += 180; // turn around
				Player.Rotate(Vector3.up, _rotationDifference);

				_positionOffset = Quaternion.Euler(0.0f, _rotationDifference, 0.0f) * _distance;
				Player.position = Destination.position + _positionOffset;

				_isTriggered = false;
				OnTeleportEnter(this, Player);
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_isTriggered = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			_isTriggered = false;
		}
	}
}
