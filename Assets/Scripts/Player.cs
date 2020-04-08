using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

public class Player : PlayerBehavior
{

	public static Player player;

	public string Name { get; private set; }
	public Vector3 PlayerVelocity { get; private set; }
	public Transform PlayerCameraTransform;
	[SerializeField] TextMesh namePlate = null;
	[SerializeField] float walkSpeed = 10f;


	protected override void NetworkStart()
	{
		base.NetworkStart();

		NetworkManager.Instance.Networker.disconnected += OnDisconnected;
		networkObject.UpdateInterval = 17;

		if (!networkObject.IsOwner)
		{
			transform.GetChild(0).gameObject.SetActive(false);

			GetComponent<ThirdPersonMovementController>().enabled = false;
			// TODO set it up so the server keeps the rigidbody and the player sends velocity data to the server but only transform data to the clients

			Destroy(GetComponent<Rigidbody>());
		}
		else
		{
			player = this;
			GetPlayerName();
		}


	}

	public void GetPlayerName()
	{
		if (!networkObject.IsOwner) return;

		Name = PlayerPrefs.GetString("PlayerName");

		networkObject.SendRpc(RPC_UPDATE_NAME, Receivers.AllBuffered, Name);
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

		networkObject.position = transform.position;
		networkObject.rotation = transform.rotation;
		
    }

	private void FixedUpdate()
	{
		if (!networkObject.IsServer) return;

		Vector3 forwardVector = transform.forward * networkObject.verticalAxis;
		Vector3 sidewaysVector = transform.right * networkObject.horizontalAxis;

		Vector3 playerMovement = forwardVector + sidewaysVector;
		Rigidbody myRigidbody = GetComponent<Rigidbody>();
		playerMovement.y = myRigidbody.velocity.y;

		myRigidbody.velocity = playerMovement * walkSpeed;

		transform.rotation = Quaternion.Euler(0, networkObject.mouseX, 0);
	}

	public override void updateName(RpcArgs args)
	{
		Debug.Log("called update name");
		Name = args.GetNext<string>();
		if (!networkObject.IsOwner)
		{
			namePlate.text = Name;
		}
		else
		{
			namePlate.gameObject.transform.parent.gameObject.SetActive(false);
		}
	}

	private void OnDisconnected(NetWorker sender)
	{
		NetworkManager.Instance.Networker.disconnected -= OnDisconnected;

		MainThreadManager.Run(() =>
		{
			foreach (var no in sender.NetworkObjectList)
			{
				if (no.Owner.IsHost)
				{
					BMSLogger.Instance.Log("Server disconnected");
					UnityEngine.SceneManagement.SceneManager.LoadScene(0);
				}
			}

			NetworkManager.Instance.Disconnect();
			Cursor.lockState = CursorLockMode.None;
		});
	}

	public void SetMouseX(float mouseX)
	{
		networkObject.mouseX = mouseX;
	}

	public void SetAxisDataFromPlayer(float xAxis, float yAxis)
	{
		networkObject.verticalAxis = yAxis;
		networkObject.horizontalAxis = xAxis;
	}
}
