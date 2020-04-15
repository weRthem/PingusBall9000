using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

public class GameBall : GameBallBehavior
{
	public string LastPlayerToTouch { get; private set; }
	private ulong updateTime = 32;
	private Rigidbody rigidbodyRef = null;
	private GameLogic gameLogic = null;

	protected override void NetworkStart()
	{
		base.NetworkStart();

		rigidbodyRef = GetComponent<Rigidbody>();
		gameLogic = GameLogic.Instance;
		networkObject.UpdateInterval = updateTime;

		if (!networkObject.IsServer)
		{
			Destroy(GetComponent<Rigidbody>());
		}
	}

	// Update is called once per frame
	void Update()
    {
		if (!networkObject.IsServer)
		{
			transform.position = networkObject.position;
			transform.rotation = networkObject.rotation;
			return;
		}

		if (networkObject.IsServer)
		{
			networkObject.position = transform.position;
			networkObject.rotation = transform.rotation;
		}

    }

	private void OnCollisionEnter(Collision collision)
	{
		if (!networkObject.IsServer) return;

		if (collision.gameObject.GetComponent<Player>() == null) return;

		LastPlayerToTouch = collision.gameObject.GetComponent<Player>().Name;
	}

	public void Reset()
	{
		transform.position = Vector3.up * 15;

		Rigidbody myRigidbody = GetComponent<Rigidbody>();

		myRigidbody.velocity = Vector3.zero;

		myRigidbody.isKinematic = true;

		Invoke("PutBallInPlay", 1.75f);

	}

	private void PutBallInPlay()
	{
		Rigidbody myRigidbody = GetComponent<Rigidbody>();

		myRigidbody.isKinematic = false;

		Vector3 force = new Vector3(0, 400, 0);

		myRigidbody.AddForce(force);
	}

}
