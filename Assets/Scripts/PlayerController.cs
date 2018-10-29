using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TopdownCharacterController2D))]
public class PlayerController : MonoBehaviour {
    // movement config
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	[HideInInspector]
	private float normalizedVerticalSpeed = 0;

	private TopdownCharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;


	void Awake() {
		_animator = GetComponent<Animator>();
		_controller = GetComponent<TopdownCharacterController2D>();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += OnControllerCollider;
		_controller.onTriggerEnterEvent += OnTriggerEnterEvent;
		_controller.onTriggerExitEvent += OnTriggerExitEvent;
	}


	#region Event Listeners

	void OnControllerCollider(RaycastHit2D hit) {
		// bail out on plain old ground hits cause they arent very interesting
		//if( hit.normal.y == 1f )
		//	return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void OnTriggerEnterEvent(Collider2D col) {
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}


	void OnTriggerExitEvent(Collider2D col) {
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	void Update() {

		if(Input.GetKey(KeyCode.RightArrow)){
			normalizedHorizontalSpeed = 1;
			if(transform.localScale.x < 0f){
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
            }

            SetAnimation("Run");
		}else if(Input.GetKey(KeyCode.LeftArrow)){
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f ){
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
            }

            SetAnimation("Run");
		}else{
			normalizedHorizontalSpeed = 0;
            SetAnimation("Idle");
		}

		if(Input.GetKey(KeyCode.UpArrow)){
			normalizedVerticalSpeed = 1;
		}else if(Input.GetKey(KeyCode.DownArrow)){
			normalizedVerticalSpeed = -1;
		}else{
			normalizedVerticalSpeed = 0;
		}

		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		//var smoothedMovementFactor = groundDamping; // how fast do we change direction?
		//_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
		//_velocity.y = Mathf.Lerp( _velocity.y, normalizedVerticalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
        _velocity.x = normalizedHorizontalSpeed * runSpeed;
        _velocity.y = normalizedVerticalSpeed * runSpeed;

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

    private void SetAnimation(string animation){
        if(_animator==null) {return;}
        _animator.Play(Animator.StringToHash(animation));
    }
}
