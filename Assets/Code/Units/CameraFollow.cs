using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	[RequireComponent(typeof(Camera))]
	public class CameraFollow : MonoBehaviour
	{
		[SerializeField]
		private Transform target;

		[SerializeField]
		private float smoothTime = 0.2f;

		private Vector3 velocity;
		private float zOffset;

		private void Start()
		{
			Camera camera = GetComponent<Camera>();
			if (camera != null)
			{
				zOffset = camera.transform.localPosition.z;
			}
		}


		// Update is called once per frame
		void Update()
		{
			// Early exit if the target is lost
			if (target == null) return;

			// Define a target position above and behind the target transform
			Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, zOffset));

			// Smoothly move the camera towards that target position
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition,
				ref velocity, smoothTime);
		}
	}
}
