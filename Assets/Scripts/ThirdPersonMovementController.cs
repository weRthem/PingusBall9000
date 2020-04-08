using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovementController : MonoBehaviour
{
	[SerializeField] float jumpPower = 5f;
	//[SerializeField] float gravity = 9.81f;

	//private bool hasJumped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void Update()
	{
		PlayerJump();
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		PlayerMovement();
	}


	private void PlayerMovement()
	{
		float horizontalMovement = Input.GetAxis("Horizontal");
		float verticalMovement = Input.GetAxis("Vertical");



		/*
		Vector3 forwardVector = transform.forward * verticalMovement;
		Vector3 sidewaysVector = transform.right * horizontalMovement;

		Vector3 playerMovement = forwardVector + sidewaysVector;
		Rigidbody myRigidbody = GetComponent<Rigidbody>();
		playerMovement.y = myRigidbody.velocity.y;

		myRigidbody.velocity = playerMovement;*/
	}

	private void PlayerJump()
	{
		if (!Input.GetKeyDown(KeyCode.Space)) return;

		Ray ray = new Ray(transform.position, -Vector3.up);
		RaycastHit hitInfo;
		Debug.Log(Physics.Raycast(ray, out hitInfo, 1.2f));
		if (Physics.Raycast(ray, out hitInfo, 1.1f))
		{
			if (hitInfo.collider.gameObject.GetComponent<Player>()) return;

			GetComponent<Rigidbody>().AddForce(0, jumpPower, 0);
		}
	}
}
