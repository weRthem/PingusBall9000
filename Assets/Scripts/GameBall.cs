using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

public class GameBall : GameBallBehavior
{
	public string LastPlayerToTouch { get; private set; }
	private ulong updateTime = 16;
	private Rigidbody rigidbodyRef = null;
	private GameLogic gameLogic = null;

    // Start is called before the first frame update
    void Start()
    {

	}

	protected override void NetworkStart()
	{
		rigidbodyRef = GetComponent<Rigidbody>();
		gameLogic = GameLogic.Instance;
		networkObject.UpdateInterval = updateTime;

		if (!networkObject.IsServer)
		{
			//Destroy(GetComponent<Rigidbody>());
		}
	}

	// Update is called once per frame
	void Update()
    {
		/*if (!networkObject.IsOwner)
		{
			transform.position = networkObject.position;
			transform.rotation = networkObject.rotation;
			return;
		}
		
		networkObject.rotation = transform.rotation;
		networkObject.position = transform.position;*/


		if (networkObject.IsServer)
		{
			networkObject.SendRpc(RPC_RESET_BALL_TO_SERVER, Receivers.All, transform.position, transform.rotation, GetComponent<Rigidbody>().velocity);
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

		Vector3 force = new Vector3(0, 200, 0);

		myRigidbody.AddForce(force);
	}

	public override void ResetBallToServer(RpcArgs args)
	{
		if (networkObject.IsServer) return;
		Vector3 pos = args.GetNext<Vector3>();
		Quaternion rot = args.GetNext<Quaternion>();
		Vector3 vel = args.GetNext<Vector3>();

		if (Vector3.Distance(pos, transform.position) > 0.8f)
		{
			transform.position = pos;
		}

		transform.rotation = rot;

		if (Vector3.Distance(vel, GetComponent<Rigidbody>().velocity) > 0.8f)
		{
			GetComponent<Rigidbody>().velocity = vel;
		}
	}
}
