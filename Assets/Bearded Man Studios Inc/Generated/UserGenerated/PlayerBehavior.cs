using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[][\"string\"][\"Vector3\", \"Quaternion\"][\"Vector3\", \"Quaternion\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[][\"name\"][\"TargetStartPoint\", \"TargetStartRotation\"][\"TargetStartPoint\", \"TargetStartRotation\"]]")]
	public abstract partial class PlayerBehavior : NetworkBehavior
	{
		public const byte RPC_PLAYER_JUMP = 0 + 5;
		public const byte RPC_UPDATE_PLAYERS_NAME_FOR_CLIENTS = 1 + 5;
		public const byte RPC_RIGHT_CLICK = 2 + 5;
		public const byte RPC_LEFT_CLICK = 3 + 5;
		
		public PlayerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (PlayerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("PlayerJump", PlayerJump);
			networkObject.RegisterRpc("UpdatePlayersNameForClients", UpdatePlayersNameForClients, typeof(string));
			networkObject.RegisterRpc("RightClick", RightClick, typeof(Vector3), typeof(Quaternion));
			networkObject.RegisterRpc("LeftClick", LeftClick, typeof(Vector3), typeof(Quaternion));

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
		/// </summary>
		public abstract void PlayerJump(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void UpdatePlayersNameForClients(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void RightClick(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void LeftClick(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}