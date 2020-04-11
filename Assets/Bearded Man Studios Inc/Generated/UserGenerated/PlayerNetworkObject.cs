using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0.15,0.15,0.15,0]")]
	public partial class PlayerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 7;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private float _mouseX;
		public event FieldEvent<float> mouseXChanged;
		public InterpolateFloat mouseXInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float mouseX
		{
			get { return _mouseX; }
			set
			{
				// Don't do anything if the value is the same
				if (_mouseX == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_mouseX = value;
				hasDirtyFields = true;
			}
		}

		public void SetmouseXDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_mouseX(ulong timestep)
		{
			if (mouseXChanged != null) mouseXChanged(_mouseX, timestep);
			if (fieldAltered != null) fieldAltered("mouseX", _mouseX, timestep);
		}
		[ForgeGeneratedField]
		private float _horizontalAxis;
		public event FieldEvent<float> horizontalAxisChanged;
		public InterpolateFloat horizontalAxisInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float horizontalAxis
		{
			get { return _horizontalAxis; }
			set
			{
				// Don't do anything if the value is the same
				if (_horizontalAxis == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_horizontalAxis = value;
				hasDirtyFields = true;
			}
		}

		public void SethorizontalAxisDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_horizontalAxis(ulong timestep)
		{
			if (horizontalAxisChanged != null) horizontalAxisChanged(_horizontalAxis, timestep);
			if (fieldAltered != null) fieldAltered("horizontalAxis", _horizontalAxis, timestep);
		}
		[ForgeGeneratedField]
		private float _verticalAxis;
		public event FieldEvent<float> verticalAxisChanged;
		public InterpolateFloat verticalAxisInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float verticalAxis
		{
			get { return _verticalAxis; }
			set
			{
				// Don't do anything if the value is the same
				if (_verticalAxis == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_verticalAxis = value;
				hasDirtyFields = true;
			}
		}

		public void SetverticalAxisDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_verticalAxis(ulong timestep)
		{
			if (verticalAxisChanged != null) verticalAxisChanged(_verticalAxis, timestep);
			if (fieldAltered != null) fieldAltered("verticalAxis", _verticalAxis, timestep);
		}
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
				_dirtyFields[0] |= 0x8;
				_position = value;
				hasDirtyFields = true;
			}
		}

		public void SetpositionDirty()
		{
			_dirtyFields[0] |= 0x8;
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
				_dirtyFields[0] |= 0x10;
				_rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrotationDirty()
		{
			_dirtyFields[0] |= 0x10;
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
				_dirtyFields[0] |= 0x20;
				_isRunning = value;
				hasDirtyFields = true;
			}
		}

		public void SetisRunningDirty()
		{
			_dirtyFields[0] |= 0x20;
			hasDirtyFields = true;
		}

		private void RunChange_isRunning(ulong timestep)
		{
			if (isRunningChanged != null) isRunningChanged(_isRunning, timestep);
			if (fieldAltered != null) fieldAltered("isRunning", _isRunning, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			mouseXInterpolation.current = mouseXInterpolation.target;
			horizontalAxisInterpolation.current = horizontalAxisInterpolation.target;
			verticalAxisInterpolation.current = verticalAxisInterpolation.target;
			positionInterpolation.current = positionInterpolation.target;
			rotationInterpolation.current = rotationInterpolation.target;
			isRunningInterpolation.current = isRunningInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _mouseX);
			UnityObjectMapper.Instance.MapBytes(data, _horizontalAxis);
			UnityObjectMapper.Instance.MapBytes(data, _verticalAxis);
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _isRunning);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_mouseX = UnityObjectMapper.Instance.Map<float>(payload);
			mouseXInterpolation.current = _mouseX;
			mouseXInterpolation.target = _mouseX;
			RunChange_mouseX(timestep);
			_horizontalAxis = UnityObjectMapper.Instance.Map<float>(payload);
			horizontalAxisInterpolation.current = _horizontalAxis;
			horizontalAxisInterpolation.target = _horizontalAxis;
			RunChange_horizontalAxis(timestep);
			_verticalAxis = UnityObjectMapper.Instance.Map<float>(payload);
			verticalAxisInterpolation.current = _verticalAxis;
			verticalAxisInterpolation.target = _verticalAxis;
			RunChange_verticalAxis(timestep);
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
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _mouseX);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _horizontalAxis);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _verticalAxis);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rotation);
			if ((0x20 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _isRunning);

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
				if (mouseXInterpolation.Enabled)
				{
					mouseXInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					mouseXInterpolation.Timestep = timestep;
				}
				else
				{
					_mouseX = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_mouseX(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (horizontalAxisInterpolation.Enabled)
				{
					horizontalAxisInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					horizontalAxisInterpolation.Timestep = timestep;
				}
				else
				{
					_horizontalAxis = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_horizontalAxis(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (verticalAxisInterpolation.Enabled)
				{
					verticalAxisInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					verticalAxisInterpolation.Timestep = timestep;
				}
				else
				{
					_verticalAxis = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_verticalAxis(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
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
			if ((0x10 & readDirtyFlags[0]) != 0)
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
			if ((0x20 & readDirtyFlags[0]) != 0)
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
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (mouseXInterpolation.Enabled && !mouseXInterpolation.current.UnityNear(mouseXInterpolation.target, 0.0015f))
			{
				_mouseX = (float)mouseXInterpolation.Interpolate();
				//RunChange_mouseX(mouseXInterpolation.Timestep);
			}
			if (horizontalAxisInterpolation.Enabled && !horizontalAxisInterpolation.current.UnityNear(horizontalAxisInterpolation.target, 0.0015f))
			{
				_horizontalAxis = (float)horizontalAxisInterpolation.Interpolate();
				//RunChange_horizontalAxis(horizontalAxisInterpolation.Timestep);
			}
			if (verticalAxisInterpolation.Enabled && !verticalAxisInterpolation.current.UnityNear(verticalAxisInterpolation.target, 0.0015f))
			{
				_verticalAxis = (float)verticalAxisInterpolation.Interpolate();
				//RunChange_verticalAxis(verticalAxisInterpolation.Timestep);
			}
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
