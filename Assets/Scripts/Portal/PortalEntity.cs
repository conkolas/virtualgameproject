using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortalEntity : MonoBehaviour {

	public PortalEntity Destination;
	public Shader ScreenCutout;
	
	private PortalCamera _portalCamera;
	private PortalTeleport _portalTeleport;
	
	private Camera _camera;
	private GameObject _cameraObject;
	private Material _cameraMaterial;
	private RenderTexture _cameraRenderTexture;

	private bool _initialized;
	private Transform _player;
	private Transform _transform;
	private MeshRenderer _renderer;
	
	public Camera Camera {
		get { return _camera; }
	}

	public Transform PortalTransform {
		get { return _transform; }
	}

	public bool Initialized {
		get { return _initialized; }
	}

	private void Start() {
		_initialized = false;
		foreach (var trans in FindObjectsOfType<Transform>())
			if (trans.gameObject.CompareTag("Player"))
				_player = trans;
		foreach (var trans in GetComponentsInChildren<Transform>()) 
			if (trans.gameObject.CompareTag("PortalPlane")) 
				_transform = trans;
		foreach (var render in GetComponentsInChildren<MeshRenderer>()) 
			if (render.gameObject.CompareTag("PortalPlane"))
				_renderer = render;
		foreach (var teleport in GetComponentsInChildren<PortalTeleport>()) 
			if (teleport.gameObject.CompareTag("PortalPlane"))
				_portalTeleport = teleport;
		
		if (_player == null) {
			Debug.LogWarning("Portal can't find Transform with 'Player' tag in the scene!");
			return;
		}
		if (ScreenCutout == null) {
			Debug.LogWarning("Portal shader is unassigned!");
			return;
		}
		if (PortalTransform == null) {
			Debug.LogWarning("Portal entity can't find Transform component with 'PortalPlane' tag!");
			return;
		}
		if (_renderer == null) {
			Debug.LogWarning("Portal entity can't find MeshRenderer component with 'PortalPlane' tag!");
			return;
		}
		if (_portalTeleport == null) {
			Debug.LogWarning("Portal entity can't find PortalTeleport component with 'PortalPlane' tag!");
			return;
		}

		GameObject go = new GameObject("Portal Camera");
		_cameraObject = Instantiate(go);
		_camera = _cameraObject.AddComponent<Camera>();
		_portalCamera = _cameraObject.AddComponent<PortalCamera>();
		DestroyObject(go);

		_cameraRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);
		
		_cameraMaterial = new Material(ScreenCutout);
		_cameraMaterial.mainTexture = _cameraRenderTexture;
		
		_renderer.material = _cameraMaterial;
	}

	private void Update () {
		if (!Initialized && Destination != null && Destination.Camera != null) {
			_initialized = true;

			if (Destination.Camera.targetTexture != null) {
				Destination.Camera.targetTexture.Release();
			}
			Destination.Camera.targetTexture = _cameraRenderTexture;
			
			_portalTeleport.Player = _player;
			_portalTeleport.Destination = Destination.PortalTransform;
			
			_portalCamera.PlayerCamera = Camera.main.transform;
			_portalCamera.Portal = PortalTransform;
			_portalCamera.OtherPortal = Destination.PortalTransform;
		}
	}
}
