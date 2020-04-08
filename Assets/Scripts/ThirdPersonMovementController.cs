using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

public class ThirdPersonMovementController : MonoBehaviour
{
	[SerializeField] float walkSpeed = 10f;
	[SerializeField] float jumpPower = 350f;

    // Start is called before the first frame update
    void Start()
    {
		//GetComponent<Player>().SetAxisDataFromPlayer(0, 0);
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

		Debug.Log("Player ver: " + verticalMovement + " Player horz: " + horizontalMovement);

		Vector3 forwardVector = transform.forward * verticalMovement * walkSpeed;
		Vector3 sidewaysVector = transform.right * horizontalMovement * walkSpeed;

		Vector3 playerMovement = forwardVector + sidewaysVector;
		Rigidbody myRigidbody = GetComponent<Rigidbody>();
		playerMovement.y = myRigidbody.velocity.y;

		myRigidbody.velocity = playerMovement;
	}

	private void PlayerJump()
	{
		if (!Input.GetKeyDown(KeyCode.Space)) return;

		Ray ray = new Ray(transform.position, -Vector3.up);
		RaycastHit hitInfo;
		Debug.Log(Physics.Raycast(ray, out hitInfo, 1.1f));
		if (Physics.Raycast(ray, out hitInfo, 1.1f))
		{
			if (hitInfo.collider.gameObject.GetComponent<Player>()) return;

			GetComponent<Rigidbody>().AddForce(0, jumpPower, 0);
		}
	}
}
