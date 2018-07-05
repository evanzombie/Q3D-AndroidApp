﻿/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Interface for a spline's anchor and tangents.
/// 
/// </summary>

using UnityEngine;
using System.Collections;

namespace Pixelplacement
{
	public enum TangentMode {Mirrored, Aligned, Free}

	[ExecuteInEditMode]
	public class SplineAnchor : MonoBehaviour
	{
		#region Public Variables
		public TangentMode tangentMode;
		#endregion

		#region Public Properties
		public bool RenderingChange
		{
			get; set;
		}

		public bool Changed
		{
			get; set;
		}

		[SerializeField] public Transform Anchor
		{
			get; private set;
		}

		[SerializeField] public Transform InTangent
		{
			get; private set;
		}

		[SerializeField] public Transform OutTangent
		{
			get; private set;
		}
		#endregion

		#region Private Variables
		bool _initialized;
		[SerializeField][HideInInspector] Transform _masterTangent;
		[SerializeField][HideInInspector] Transform _slaveTangent;
		TangentMode _previousTangentMode;
		Vector3 _previousInPosition;
		Vector3 _previousOutPosition;
		Vector3 _previousAnchorPosition;
		Bounds _skinnedBounds;
		#endregion

		#region Init
		void Awake ()
		{
			Initialize ();
		}
		#endregion

		#region Loop
		void Update ()
		{
			//don't let an anchor scale:
			transform.localScale = Vector3.one;

			//initialization:
			if (!_initialized)
			{
				Initialize ();

				//flag initialization:
				_initialized = true;
			}

			//override any skinned mesh bounds changes:
			Anchor.localPosition = Vector3.zero;

			//has the anchor moved?
			if (_previousAnchorPosition != transform.position)
			{
				Changed = true;
				RenderingChange = true;
				_previousAnchorPosition = transform.position;
			}

			//run a tangent operation if mode has changed:
			if (_previousTangentMode != tangentMode)
			{
				Changed = true;
				RenderingChange = true;
				TangentChanged ();
				_previousTangentMode = tangentMode;
			}
				
			//detect tangent movements:
			if (InTangent.position != _previousInPosition)
			{
				Changed = true;
				RenderingChange = true;
				_previousInPosition = InTangent.position;
				_masterTangent = InTangent;
				_slaveTangent = OutTangent;
				TangentChanged ();
				return;
			}

			if (OutTangent.position != _previousOutPosition)
			{
				Changed = true;
				RenderingChange = true;
				_previousOutPosition = OutTangent.position;
				_masterTangent = OutTangent;
				_slaveTangent = InTangent;
				TangentChanged ();
				return;
			}
		}
		#endregion

		#region Private Methods
		void TangentChanged ()
		{
			//calculate tangent positions:
			switch (tangentMode)
			{
			case TangentMode.Free:
				break;

			case TangentMode.Mirrored:
				Vector3 mirroredOffset = _masterTangent.position - transform.position;
				_slaveTangent.position = transform.position - mirroredOffset;
				break;

			case TangentMode.Aligned:
				float distance = Vector3.Distance (_slaveTangent.position, transform.position);
				Vector3 alignedOffset = (_masterTangent.position - transform.position).normalized;
				_slaveTangent.position = transform.position - (alignedOffset * distance);
				break;
			}

			//cache tangent positions:
			_previousInPosition = InTangent.position;
			_previousOutPosition = OutTangent.position;
		}
		#endregion

		#region Private Methods
		void Initialize ()
		{
			//grabs references:
			InTangent = transform.GetChild (0);
			OutTangent = transform.GetChild (1);
			Anchor = transform.GetChild (2); 

			//prepopulate master and slave tangents:
			_masterTangent = InTangent;
			_slaveTangent = OutTangent;

			//hide some things to reduce clutter:
			Anchor.hideFlags = HideFlags.HideInHierarchy;

			foreach (var item in GetComponentsInChildren<Renderer> ())
			{
				item.sharedMaterial.hideFlags = HideFlags.HideInInspector;
			}

			foreach (var item in GetComponentsInChildren<MeshFilter> ())
			{
				item.hideFlags = HideFlags.HideInInspector;
			}

			foreach (var item in GetComponentsInChildren<MeshRenderer> ())
			{
				item.hideFlags = HideFlags.HideInInspector;
			}

			foreach (var item in GetComponentsInChildren<SkinnedMeshRenderer> ())
			{
				item.hideFlags = HideFlags.HideInInspector;
			}

			//synchronize status variables:
			_previousTangentMode = tangentMode;
			_previousInPosition = InTangent.position;
			_previousOutPosition = OutTangent.position;
			_previousAnchorPosition = transform.position;
		}
		#endregion

		#region Public Methods
		public void SetTangentStatus (bool inStatus, bool outStatus)
		{
			InTangent.gameObject.SetActive (inStatus);
			OutTangent.gameObject.SetActive (outStatus);
		}

		public void Tilt (Vector3 angles)
		{
			//save current rotation and rotate as requested:
			Quaternion rotation = transform.localRotation;
			transform.Rotate (angles);

			//get world position of tangents:
			Vector3 inPosition = InTangent.position;
			Vector3 outPosition = OutTangent.position;

			//reverse rotation and set tangent positions:
			//transform.localRotation = rotation;
			InTangent.position = inPosition;
			OutTangent.position = outPosition;
		}
		#endregion
	}
}