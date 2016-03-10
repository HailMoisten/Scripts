using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class CameraManager : MonoBehaviour
	{

		// The target we are following
		[SerializeField]
		private Transform target = null;
        [SerializeField]
        private float targetHeight = 2;
        // The distance in the x-z plane to the target
        [SerializeField]
		private float distance = 7.0f;
        public void inclementDistance() { if (distance <= 14) { distance++; } }
        public void declementDistance() { if (distance >= 2) { distance--; } }
        // the height we want the camera to be above the target
        [SerializeField]
		private float height = 4.5f;
        public void inclementHeight() { if (height <= 9.5f) { height++; } }
        public void declementHeight() { if (height >= 1.0f) { height--; } }
        [SerializeField]
		private float rotationDamping = 0.0f;
		[SerializeField]
		private float heightDamping = 128.0f;

		// Use this for initialization
		void Start()
        {
            height = 4.5f;
            distance = 7.0f;
        }

		// Update is called once per frame
		void LateUpdate()
		{
			// Early out if we don't have a target
			if (!target)
				return;

            // Calculate the current rotation angles
            var wantedRotationAngle = target.eulerAngles.y;
			var wantedHeight = target.position.y + height;

			var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = transform.position.y;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			transform.position = target.position;
			transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			transform.position = new Vector3(transform.position.x ,currentHeight, transform.position.z);

            // Always look at the target

            transform.position = new Vector3(transform.position.x, transform.position.y - targetHeight, transform.position.z);
            transform.LookAt(target);
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        }
    }
}