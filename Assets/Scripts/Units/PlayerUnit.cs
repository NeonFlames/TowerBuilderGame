using UnityEngine;
using System.Collections;
using TowerBuilder.Utilities;

namespace TowerBuilder.Units {
	public class PlayerUnit : MonoBehaviour {

		private Vector3 initialPosition;

		public float movementSpeed = 10f;
		public float maximumSpeed = 100f;
		public float knockbackForce = 5f;
		public float jumpForce = 50f;

		private Rigidbody rigidbody;
		private BoxCollider boxCollider;

		// Use this for initialization
		void Start () {
			this.initialPosition = this.transform.position;
			this.rigidbody = this.GetComponent<Rigidbody>();
			this.boxCollider = this.GetComponent<BoxCollider>();
		}
		
		// Update is called once per frame
		void Update () {
			if (Input.GetKeyDown(KeyCode.Z)) {
				this.rigidbody.MovePosition(this.initialPosition);
				this.rigidbody.velocity = Vector3.zero;
			}

			if (this.IsGrounded ()) {
				if (this.ShouldRun ()) {
					// move the object forward
					this.rigidbody.velocity = Vector3.ClampMagnitude (this.rigidbody.velocity, maximumSpeed);
					this.rigidbody.AddForce (new Vector3 (-1 * movementSpeed, 0));
				}

				if (this.ShouldJump ()) {
					Debug.Log ("jump jump jump");
					this.rigidbody.AddForce (new Vector3 (0, jumpForce, 0));
				}
			}

			// constrain the object's rotation
			Quaternion rotation = this.rigidbody.rotation;
			rotation.eulerAngles =
				new Vector3(rotation.eulerAngles.x,
				            rotation.eulerAngles.y,
				            Mathf.Clamp(rotation.eulerAngles.z, -90, 90));
			this.rigidbody.rotation = rotation;
		}

		private bool IsGrounded() {
			return Physics.Raycast(transform.position, -Vector3.up, this.boxCollider.bounds.extents.y + 0.1f);
		}

		private bool ShouldJump() {
			return Physics.Raycast(transform.position, Vector3.left, this.boxCollider.bounds.extents.x + 0.1f)
				&& !Physics.Raycast(transform.position, Vector3.up, this.boxCollider.bounds.extents.y + 2f);
		}

		private bool ShouldRun() {
			Debug.DrawRay (transform.position + Vector3.down * (this.boxCollider.bounds.extents.y - 0.1f), Vector3.left);
			return !Physics.Raycast(transform.position + Vector3.down * (this.boxCollider.bounds.extents.y + 0.01f), Vector3.left, this.boxCollider.bounds.extents.x + 0.1f);
		}

		void OnCollisionEnter(Collision collision) {
			Wall wall = collision.gameObject.GetComponent<Wall>();
			if (wall != null) {
				// we collided with the wall
				float knockback = this.rigidbody.velocity.x * knockbackForce;
				this.rigidbody.velocity = new Vector3(knockback, knockback, this.rigidbody.velocity.z);
			}
		}
	}
}
