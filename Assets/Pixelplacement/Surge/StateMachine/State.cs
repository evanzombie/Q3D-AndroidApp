/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Base class for States to be used as children of StateMachines.
/// 
/// </summary>

using UnityEngine;
using System.Collections;

namespace Pixelplacement
{
	public class State : MonoBehaviour 
	{
		#region Public Properties
		public StateMachine StateMachine {
			get;
			set;
		}
		#endregion

		#region Init
		void Awake ()
		{
			if (transform.parent == null) return;
			StateMachine = transform.parent.GetComponent<StateMachine> ();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Changes the state.
		/// </summary>
		public void ChangeState (GameObject state)
		{
			StateMachine.ChangeState (state.name);
		}

		/// <summary>
		/// Changes the state.
		/// </summary>
		public void ChangeState (string state)
		{
			if (StateMachine == null) return;
			StateMachine.ChangeState (state);
		}

		/// <summary>
		/// Change to the next state if possible.
		/// </summary>
		public GameObject Next ()
		{
			return StateMachine.Next ();
		}

		/// <summary>
		/// Change to the previous state if possible.
		/// </summary>
		public GameObject Previous ()
		{
			return StateMachine.Previous ();
		}

		/// <summary>
		/// Exit the current state.
		/// </summary>
		public void Exit ()
		{
			StateMachine.Exit ();
		}
		#endregion
	}
}