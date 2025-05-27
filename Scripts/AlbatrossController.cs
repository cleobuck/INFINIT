using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// The character controller for the main character in the Flight of the Albatross demo scene
	/// </summary>
	public class AlbatrossController : FreeLeftToRightMovement 
	{
		/// the gameobject containing the albatross model, used for rotation without affecting the boxcollider
		public GameObject AlbatrossBody;
		/// the maximum angle, in degrees, at which the albatross can rotate
		public float MaximumAlbatrossRotation= 45f;
		/// the frequency at which the albatross completes a low/high/low cycle
		protected float Frequency=2f;

		protected Vector3 _originalPosition;
		protected float _randomVariation;

		/// <summary>
		/// On Start, we get a random variation for the albatross movement
		/// </summary>
		protected override void Start ()
		{
			base.Start();
			_originalPosition = transform.position;
			_randomVariation = UnityEngine.Random.Range(0f,100f);
			
			// Debug animation state
			Animator animator = GetComponent<Animator>();
			if (animator != null)
			{
				Debug.Log("Animator found, is playing: " + animator.isActiveAndEnabled);
				Debug.Log("Current animation state: " + animator.GetCurrentAnimatorStateInfo(0).shortNameHash);
				Debug.Log("Animation speed: " + animator.speed);
				Debug.Log("Animation layer count: " + animator.layerCount);
				
				// Force play the animation
				animator.Play("Idle", 0, 0f);
				animator.speed = 1f;
			}
			else
			{
				Debug.LogWarning("No Animator component found!");
			}
		}

		/// <summary>
		/// On update we just handle our character's movement
		/// </summary>
		protected override void Update () 
		{
			base.Update();
			HandleAlbatrossMovement();

			// Debug animation state every frame
			Animator animator = GetComponent<Animator>();
			if (animator != null && animator.isActiveAndEnabled)
			{
				AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
				Debug.Log("Animation playing: " + stateInfo.IsName("Idle") + ", normalized time: " + stateInfo.normalizedTime);
			}
		}

		/// <summary>
		/// Handles the albatross movement.
		/// </summary>
		protected virtual void HandleAlbatrossMovement()
		{
			// we move the albatross up and down periodically
			_newPosition = transform.position;
			_newPosition.y = _originalPosition.y + Mathf.Sin(Frequency * (Time.time + _randomVariation));
			transform.position = _newPosition;

			// we make it rotate according to direction
			if (AlbatrossBody!=null)
			{
				AlbatrossBody.transform.localEulerAngles = -_currentMovement * MaximumAlbatrossRotation * Vector3.forward;
			}
		}

		public override void LeftEnd()
		{
			// Don't reset movement to prevent drifting back to center
		}

		public override void RightEnd()
		{
			// Don't reset movement to prevent drifting back to center
		}

		protected override void UpdateAllMecanimAnimators()
		{
			// Do nothing - let the animation play continuously
		}
	}
}