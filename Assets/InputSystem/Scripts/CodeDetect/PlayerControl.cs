using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
	public float moveSpeed = 5f;
	
	PlayerInput playerInput;
	Rigidbody rb;
	private Vector2 moveInput;
	
	// Start is called before the first frame update
	void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		rb = GetComponent<Rigidbody>();
		
		playerInput.onActionTriggered += OnActionTriggered;
	}

	void OnActionTriggered(InputAction.CallbackContext context)
	{
		switch(context.action.name)
		{
			case "Fire":
				if(context.phase == InputActionPhase.Performed)
				{
					Debug.Log("Fire Performed");
				}
				break;
			case "Move":
				moveInput = context.ReadValue<Vector2>();
				if(moveInput.magnitude > 1)
					moveInput = moveInput.normalized;
				Vector3 moveVelocity = new Vector3(moveInput.x * moveSpeed, rb.velocity.y, moveInput.y * moveSpeed);
				rb.velocity = moveVelocity;
				break;
		}
	}
}
