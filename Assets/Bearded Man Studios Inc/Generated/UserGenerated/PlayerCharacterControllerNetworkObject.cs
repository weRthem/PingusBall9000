using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0.15,0]")]
	public partial class PlayerCharacterControllerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 10;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
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
				_dirtyFields[0] |= 0x1;
				_horizontalAxis = value;
				hasDirtyFields = true;
			}
		}

		public void SethorizontalAxisDirty()
		{
			_dirtyFields[0] |= 0x1;
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
				_dirtyFields[0] |= 0x2;
				_verticalAxis = value;
				hasDirtyFields = true;
			}
		}

		public void SetverticalAxisDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_verticalAxis(ulong timestep)
		{
			if (verticalAxisChanged != null) verticalAxisChanged(_verticalAxis, timestep);
			if (fieldAltered != null) fieldAltered("verticalAxis", _verticalAxis, timestep);
		}
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
				_dirtyFields[0] |= 0x4;
				_mouseX = value;
				hasDirtyFields = true;
			}
		}

		public void SetmouseXDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_mouseX(ulong timestep)
		{
			if (mouseXChanged != null) mouseXChanged(_mouseX, timestep);
			if (fieldAltered != null) fieldAltered("mouseX", _mouseX, timestep);
		}
		[ForgeGeneratedField]
		private bool _isPressingShift;
		public event FieldEvent<bool> isPressingShiftChanged;
		public Interpolated<bool> isPressingShiftInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool isPressingShift
		{
			get { return _isPressingShift; }
			set
			{
				// Don't do anything if the value is the same
				if (_isPressingShift == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_isPressingShift = value;
				hasDirtyFields = true;
			}
		}

		public void SetisPressingShiftDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_isPressingShift(ulong timestep)
		{
			if (isPressingShiftChanged != null) isPressingShiftChanged(_isPressingShift, timestep);
			if (fieldAltered != null) fieldAltered("isPressingShift", _isPressingShift, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			horizontalAxisInterpolation.current = horizontalAxisInterpolation.target;
			verticalAxisInterpolation.current = verticalAxisInterpolation.target;
			mouseXInterpolation.current = mouseXInterpolation.target;
			isPressingShiftInterpolation.current = isPressingShiftInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _horizontalAxis);
			UnityObjectMapper.Instance.MapBytes(data, _verticalAxis);
			UnityObjectMapper.Instance.MapBytes(data, _mouseX);
			UnityObjectMapper.Instance.MapBytes(data, _isPressingShift);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_horizontalAxis = UnityObjectMapper.Instance.Map<float>(payload);
			horizontalAxisInterpolation.current = _horizontalAxis;
			horizontalAxisInterpolation.target = _horizontalAxis;
			RunChange_horizontalAxis(timestep);
			_verticalAxis = UnityObjectMapper.Instance.Map<float>(payload);
			verticalAxisInterpolation.current = _verticalAxis;
			verticalAxisInterpolation.target = _verticalAxis;
			RunChange_verticalAxis(timestep);
			_mouseX = UnityObjectMapper.Instance.Map<float>(payload);
			mouseXInterpolation.current = _mouseX;
			mouseXInterpolation.target = _mouseX;
			RunChange_mouseX(timestep);
			_isPressingShift = UnityObjectMapper.Instance.Map<bool>(payload);
			isPressingShiftInterpolation.current = _isPressingShift;
			isPressingShiftInterpolation.target = _isPressingShift;
			RunChange_isPressingShift(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _horizontalAxis);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _verticalAxis);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _mouseX);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _isPressingShift);

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
			if ((0x2 & readDirtyFlags[0]) != 0)
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
			if ((0x4 & readDirtyFlags[0]) != 0)
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
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (isPressingShiftInterpolation.Enabled)
				{
					isPressingShiftInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					isPressingShiftInterpolation.Timestep = timestep;
				}
				else
				{
					_isPressingShift = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_isPressingShift(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

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
			if (mouseXInterpolation.Enabled && !mouseXInterpolation.current.UnityNear(mouseXInterpolation.target, 0.0015f))
			{
				_mouseX = (float)mouseXInterpolation.Interpolate();
				//RunChange_mouseX(mouseXInterpolation.Timestep);
			}
			if (isPressingShiftInterpolation.Enabled && !isPressingShiftInterpolation.current.UnityNear(isPressingShiftInterpolation.target, 0.0015f))
			{
				_isPressingShift = (bool)isPressingShiftInterpolation.Interpolate();
				//RunChange_isPressingShift(isPressingShiftInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerCharacterControllerNetworkObject() : base() { Initialize(); }
		public PlayerCharacterControllerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerCharacterControllerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
