using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"string\"][\"Vector3\", \"Quaternion\", \"float\"][][\"Vector3\"][\"bool\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"playerName\"][\"velocity\", \"rotation\", \"runEnergy\"][][\"position\"][\"IsOnBlueTeam\"]]")]
	public abstract partial class PlayerBehavior : NetworkBehavior
	{
		public const byte RPC_UPDATE_NAME = 0 + 5;
		public const byte RPC_SET_PLAYERS_POS_AND_ROT = 1 + 5;
		public const byte RPC_PLAYER_JUMP = 2 + 5;
		public const byte RPC_SET_POS_TO_SERVER = 3 + 5;
		public const byte RPC_UPDATE_PLAYER_TEAM = 4 + 5;
		
		public PlayerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (PlayerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("UpdateName", UpdateName, typeof(string));
			networkObject.RegisterRpc("SetPlayersPosAndRot", SetPlayersPosAndRot, typeof(Vector3), typeof(Quaternion), typeof(float));
			networkObject.RegisterRpc("PlayerJump", PlayerJump);
			networkObject.RegisterRpc("SetPosToServer", SetPosToServer, typeof(Vector3));
			networkObject.RegisterRpc("UpdatePlayerTeam", UpdatePlayerTeam, typeof(bool));

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId)){
					uint newId = obj.NetworkId + 1;
					ProcessOthers(gameObject.transform, ref newId);
				}
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() =>
						{
							transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
							transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
						});
					}
					else if ((transformFlags & 0x01) != 0)
					{
						MainThreadManager.Run(() => { transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform); });
					}
					else if ((transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() => { transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); });
					}
				}
			}

			MainThreadManager.Run(() =>
			{
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new PlayerNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new PlayerNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// string playerName
		/// </summary>
		public abstract void UpdateName(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// Vector3 velocity
		/// Quaternion rotation
		/// float runEnergy
		/// </summary>
		public abstract void SetPlayersPosAndRot(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void PlayerJump(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void SetPosToServer(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void UpdatePlayerTeam(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}