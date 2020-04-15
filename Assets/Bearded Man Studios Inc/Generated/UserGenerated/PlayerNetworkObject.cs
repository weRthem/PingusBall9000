using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0,0]")]
	public partial class PlayerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 8;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _position;
		public event FieldEvent<Vector3> positionChanged;
		public InterpolateVector3 positionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 position
		{
			get { return _position; }
			set
			{
				// Don't do anything if the value is the same
				if (_position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_position = value;
				hasDirtyFields = true;
			}
		}

		public void SetpositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_position(ulong timestep)
		{
			if (positionChanged != null) positionChanged(_position, timestep);
			if (fieldAltered != null) fieldAltered("position", _position, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _rotation;
		public event FieldEvent<Quaternion> rotationChanged;
		public InterpolateQuaternion rotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion rotation
		{
			get { return _rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_rotation(ulong timestep)
		{
			if (rotationChanged != null) rotationChanged(_rotation, timestep);
			if (fieldAltered != null) fieldAltered("rotation", _rotation, timestep);
		}
		[ForgeGeneratedField]
		private bool _isRunning;
		public event FieldEvent<bool> isRunningChanged;
		public Interpolated<bool> isRunningInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool isRunning
		{
			get { return _isRunning; }
			set
			{
				// Don't do anything if the value is the same
				if (_isRunning == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_isRunning = value;
				hasDirtyFields = true;
			}
		}

		public void SetisRunningDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_isRunning(ulong timestep)
		{
			if (isRunningChanged != null) isRunningChanged(_isRunning, timestep);
			if (fieldAltered != null) fieldAltered("isRunning", _isRunning, timestep);
		}
		[ForgeGeneratedField]
		private bool _isBlueTeam;
		public event FieldEvent<bool> isBlueTeamChanged;
		public Interpolated<bool> isBlueTeamInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool isBlueTeam
		{
			get { return _isBlueTeam; }
			set
			{
				// Don't do anything if the value is the same
				if (_isBlueTeam == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_isBlueTeam = value;
				hasDirtyFields = true;
			}
		}

		public void SetisBlueTeamDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_isBlueTeam(ulong timestep)
		{
			if (isBlueTeamChanged != null) isBlueTeamChanged(_isBlueTeam, timestep);
			if (fieldAltered != null) fieldAltered("isBlueTeam", _isBlueTeam, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			positionInterpolation.current = positionInterpolation.target;
			rotationInterpolation.current = rotationInterpolation.target;
			isRunningInterpolation.current = isRunningInterpolation.target;
			isBlueTeamInterpolation.current = isBlueTeamInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _isRunning);
			UnityObjectMapper.Instance.MapBytes(data, _isBlueTeam);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			positionInterpolation.current = _position;
			positionInterpolation.target = _position;
			RunChange_position(timestep);
			_rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			rotationInterpolation.current = _rotation;
			rotationInterpolation.target = _rotation;
			RunChange_rotation(timestep);
			_isRunning = UnityObjectMapper.Instance.Map<bool>(payload);
			isRunningInterpolation.current = _isRunning;
			isRunningInterpolation.target = _isRunning;
			RunChange_isRunning(timestep);
			_isBlueTeam = UnityObjectMapper.Instance.Map<bool>(payload);
			isBlueTeamInterpolation.current = _isBlueTeam;
			isBlueTeamInterpolation.target = _isBlueTeam;
			RunChange_isBlueTeam(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _isRunning);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _isBlueTeam);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (positionInterpolation.Enabled)
				{
					positionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					positionInterpolation.Timestep = timestep;
				}
				else
				{
					_position = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (rotationInterpolation.Enabled)
				{
					rotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					rotationInterpolation.Timestep = timestep;
				}
				else
				{
					_rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_rotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (isRunningInterpolation.Enabled)
				{
					isRunningInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					isRunningInterpolation.Timestep = timestep;
				}
				else
				{
					_isRunning = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_isRunning(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (isBlueTeamInterpolation.Enabled)
				{
					isBlueTeamInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					isBlueTeamInterpolation.Timestep = timestep;
				}
				else
				{
					_isBlueTeam = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_isBlueTeam(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (positionInterpolation.Enabled && !positionInterpolation.current.UnityNear(positionInterpolation.target, 0.0015f))
			{
				_position = (Vector3)positionInterpolation.Interpolate();
				//RunChange_position(positionInterpolation.Timestep);
			}
			if (rotationInterpolation.Enabled && !rotationInterpolation.current.UnityNear(rotationInterpolation.target, 0.0015f))
			{
				_rotation = (Quaternion)rotationInterpolation.Interpolate();
				//RunChange_rotation(rotationInterpolation.Timestep);
			}
			if (isRunningInterpolation.Enabled && !isRunningInterpolation.current.UnityNear(isRunningInterpolation.target, 0.0015f))
			{
				_isRunning = (bool)isRunningInterpolation.Interpolate();
				//RunChange_isRunning(isRunningInterpolation.Timestep);
			}
			if (isBlueTeamInterpolation.Enabled && !isBlueTeamInterpolation.current.UnityNear(isBlueTeamInterpolation.target, 0.0015f))
			{
				_isBlueTeam = (bool)isBlueTeamInterpolation.Interpolate();
				//RunChange_isBlueTeam(isBlueTeamInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerNetworkObject() : base() { Initialize(); }
		public PlayerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
