using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;


public class PlayerServer : PlayerServerBehavior
{
	[SerializeField] float walkSpeed = 5f;
	[SerializeField] float jumpPower = 350f;

    // Start is called before the first frame update
    void Start()
    {
		
    }


	protected override void NetworkStart()
	{
		base.NetworkStart();

		networkObject.UpdateInterval = 17;

		if (networkObject.IsServer)
		{
			networkObject.AuthorityUpdateMode = true;
		}
	}

	// Update is called once per frame
	void Update()
    {
		if (networkObject.IsServer)
		{
			networkObject.rotation = transform.rotation;
			networkObject.velocity = transform.position;
		}

		if (!networkObject.IsServer)
		{
			//GetComponent<Player>().SetPlayerPos(networkObject.velocity);
			transform.rotation = networkObject.rotation;
		}
	}

	private void FixedUpdate()
	{
		if (!networkObject.IsServer)
		{
			return;
		}

		Player player = GetComponent<Player>();

		//Debug.Log("Player ver: " + verticalMovement + " Player horz: " + horizontalMovement);

		//Vector3 forwardVector = transform.forward * player.networkObject.verticalAxis * walkSpeed;
		//Vector3 sidewaysVector = transform.right * player.networkObject.horizontalAxis * walkSpeed;

		//Vector3 playerMovement = forwardVector + sidewaysVector;
		Rigidbody myRigidbody = GetComponent<Rigidbody>();
		//playerMovement.y = myRigidbody.velocity.y;

		//myRigidbody.velocity = playerMovement;

		//transform.rotation = Quaternion.Euler(0, player.networkObject.mouseX, 0);
	}
}
