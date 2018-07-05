﻿/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// A generic singleton.
/// 
/// </summary>

using UnityEngine;
using System.Collections;

namespace Pixelplacement
{
	[RequireComponent (typeof (Initialization))]
	public abstract class Singleton<T> : MonoBehaviour 
	{
		#region Public Properties
		/// <summary>
		/// Gets the instance.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (_instance == null) 
				{
					Debug.LogError ("Singleton not registered! Make sure the GameObject running your singleton is active in your scene and has a CoreInitialization component attached.");
					return default (T);
				}
				return _instance;
			}
		}
		#endregion

		#region Private Variables
		static T _instance;
		#endregion

		#region Virtual Methods
		/// <summary>
		/// Override this method to have code run when this singleton is initialized which is guaranteed to run before Awake and Start.
		/// </summary>
		protected virtual void OnRegistration ()
		{
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Generic method that registers the singleton instance.
		/// </summary>
		public void RegisterSingleton (T instance)
		{	
			_instance = instance;
		}
		#endregion

		#region Private Methods
		protected void Initialize (T instance)
		{
			_instance = instance;
			OnRegistration ();
		}
		#endregion
	}
}