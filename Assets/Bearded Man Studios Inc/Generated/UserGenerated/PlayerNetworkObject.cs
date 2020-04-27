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
				_dirtyFields[0] |= 0x4;
				_isBlueTeam = value;
				hasDirtyFields = true;
			}
		}

		public void SetisBlueTeamDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_isBlueTeam(ulong timestep)
		{
			if (isBlueTeamChanged != null) isBlueTeamChanged(_isBlueTeam, timestep);
			if (fieldAltered != null) fieldAltered("isBlueTeam", _isBlueTeam, timestep);
		}
		[ForgeGeneratedField]
		private float _runEnergy;
		public event FieldEvent<float> runEnergyChanged;
		public InterpolateFloat runEnergyInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float runEnergy
		{
			get { return _runEnergy; }
			set
			{
				// Don't do anything if the value is the same
				if (_runEnergy == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_runEnergy = value;
				hasDirtyFields = true;
			}
		}

		public void SetrunEnergyDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_runEnergy(ulong timestep)
		{
			if (runEnergyChanged != null) runEnergyChanged(_runEnergy, timestep);
			if (fieldAltered != null) fieldAltered("runEnergy", _runEnergy, timestep);
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
			isBlueTeamInterpolation.current = isBlueTeamInterpolation.target;
			runEnergyInterpolation.current = runEnergyInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _isBlueTeam);
			UnityObjectMapper.Instance.MapBytes(data, _runEnergy);

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
			_isBlueTeam = UnityObjectMapper.Instance.Map<bool>(payload);
			isBlueTeamInterpolation.current = _isBlueTeam;
			isBlueTeamInterpolation.target = _isBlueTeam;
			RunChange_isBlueTeam(timestep);
			_runEnergy = UnityObjectMapper.Instance.Map<float>(payload);
			runEnergyInterpolation.current = _runEnergy;
			runEnergyInterpolation.target = _runEnergy;
			RunChange_runEnergy(timestep);
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
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _isBlueTeam);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _runEnergy);

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
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (runEnergyInterpolation.Enabled)
				{
					runEnergyInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					runEnergyInterpolation.Timestep = timestep;
				}
				else
				{
					_runEnergy = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_runEnergy(timestep);
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
			if (isBlueTeamInterpolation.Enabled && !isBlueTeamInterpolation.current.UnityNear(isBlueTeamInterpolation.target, 0.0015f))
			{
				_isBlueTeam = (bool)isBlueTeamInterpolation.Interpolate();
				//RunChange_isBlueTeam(isBlueTeamInterpolation.Timestep);
			}
			if (runEnergyInterpolation.Enabled && !runEnergyInterpolation.current.UnityNear(runEnergyInterpolation.target, 0.0015f))
			{
				_runEnergy = (float)runEnergyInterpolation.Interpolate();
				//RunChange_runEnergy(runEnergyInterpolation.Timestep);
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
