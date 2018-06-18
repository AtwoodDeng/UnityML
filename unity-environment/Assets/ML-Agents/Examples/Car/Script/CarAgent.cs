using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarAgent : Agent {


	[Header("Specific to Car")]
	public GameObject target;
	public Rigidbody m_rigidbody;
	public CarArea area;
	public float forwardForce = 40f;
	public float turnForce = 1f;
	public float turnRate = 20f;
//	public float limitSpeed = 15f;
	public float detectRange = 5f;
	float[] detectAngle = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };//, 70f , 110f , 250f , 290f };

//	[HideInInspector]public float forwardDis;
//	[HideInInspector]public float leftDis;
//	[HideInInspector]public float rightDis;

	public override void InitializeAgent ()
	{
		base.InitializeAgent ();

		ResetReward ();

	}


	/// <summary>
	/// Converts polar coordinate to cartesian coordinate.
	/// </summary>
	public static Vector3 PolarToCartesian(float radius, float angle)
	{
		float x = radius * Mathf.Cos(DegreeToRadian(angle));
		float z = radius * Mathf.Sin(DegreeToRadian(angle));
		return new Vector3(x, 0f, z);
	}

	/// <summary>
	/// Converts degrees to radians.
	/// </summary>
	public static float DegreeToRadian(float degree)
	{
		return degree * Mathf.PI / 180f;
	}

	public override void CollectObservations ()
	{
		List<float> state = new List<float> ();
//		state.Add (target.transform.position.x - transform.position.x);
//		state.Add (target.transform.position.z - transform.position.z);
//		state.Add (m_rigidbody.velocity.x);
//		state.Add (m_rigidbody.velocity.z);

		AddVectorObs (target.transform.position.x - transform.position.x);
		AddVectorObs (target.transform.position.z - transform.position.z);
		AddVectorObs (m_rigidbody.velocity.x);
		AddVectorObs (m_rigidbody.velocity.z);



		for (int i = 0; i < detectAngle.Length; ++i) {
			float distance = 0;
			float angle = detectAngle [i];

			Vector3 direction = transform.TransformDirection (PolarToCartesian (1f, angle));
			direction.y = 0;

			RaycastHit hitInfo;

			if (Physics.Raycast (transform.position, direction, out hitInfo, detectRange)) {
				distance = hitInfo.distance;
			}


			AddVectorObs (distance);


		}

//		forwardDis = 999f;
//		leftDis = 999f;
//		rightDis = 999f;
//		/// Collect obstacle info
//		Vector3 forward = m_rigidbody.transform.forward;
//		Vector3 leftSide = Vector3.Cross (forward, Vector3.down * 0.5f);
//		leftSide = Vector3.Lerp (forward, leftSide, 0.7f).normalized;
//		Vector3 rightSide = Vector3.Cross (forward, Vector3.up * 0.5f);
//		rightSide = Vector3.Lerp (forward, rightSide, 0.7f).normalized;
//
//		RaycastHit hitInfo;
//		if (Physics.Raycast (transform.position, forward, out hitInfo, detectRange )) {
//			forwardDis = hitInfo.distance;
//		}
//		if (Physics.Raycast (transform.position, leftSide, out hitInfo, detectRange )) {
//			leftDis = hitInfo.distance;
//		}
//		if (Physics.Raycast (transform.position, rightSide, out hitInfo, detectRange )) {
//			rightDis = hitInfo.distance;
//		}
//
////		state.Add (forwardDis);
////		state.Add (leftDis);
////		state.Add (rightDis);
//
//		AddVectorObs (forwardDis);
//		AddVectorObs (leftDis);
//		AddVectorObs (rightDis);

//		return state;
	}
//	public override List<float> CollectState ()
//	{
//		List<float> state = new List<float> ();
//		state.Add (target.transform.position.x - transform.position.x);
//		state.Add (target.transform.position.z - transform.position.z);
//		state.Add (m_rigidbody.velocity.x);
//		state.Add (m_rigidbody.velocity.z);
//
//
//		forwardDis = 999f;
//		leftDis = 999f;
//		rightDis = 999f;
//		/// Collect obstacle info
//		Vector3 forward = m_rigidbody.transform.forward;
//		Vector3 leftSide = Vector3.Cross (forward, Vector3.down * 0.5f);
//		leftSide = Vector3.Lerp (forward, leftSide, 0.7f).normalized;
//		Vector3 rightSide = Vector3.Cross (forward, Vector3.up * 0.5f);
//		rightSide = Vector3.Lerp (forward, rightSide, 0.7f).normalized;
//
//		RaycastHit hitInfo;
//		if (Physics.Raycast (transform.position, forward, out hitInfo, detectRange )) {
//			forwardDis = hitInfo.distance;
//		}
//		if (Physics.Raycast (transform.position, leftSide, out hitInfo, detectRange )) {
//			leftDis = hitInfo.distance;
//		}
//		if (Physics.Raycast (transform.position, rightSide, out hitInfo, detectRange )) {
//			rightDis = hitInfo.distance;
//		}
//
//		state.Add (forwardDis);
//		state.Add (leftDis);
//		state.Add (rightDis);
//
//
//		return state;
//	}


	public override void AgentAction (float[] action , string textAction)
	{
		// Recieve the input from the action list
		float inputForward = 0;
		float inputLeft = 0;

		inputForward = Mathf.Clamp (action [0], -1f, 1f);
		inputLeft = Mathf.Clamp (action [1], -1f, 1f);

//		if (brain.brainParameters.actionSpaceType == StateType.continuous) {
//			inputForward = Mathf.Clamp (action [0], -1f, 1f);
//			inputLeft = Mathf.Clamp (action [1], -1f, 1f);
//		} else {
//			int movement = Mathf.FloorToInt (action [0]);
//			if (movement == 1) { directX = -1; }
//			if (movement == 2) { directX = 1; }
//			if (movement == 3) { directZ = -1; }
//			if (movement == 4) { directZ = 1; }
//		}


		// calculate the direction vector
		Vector3 forward = transform.forward;
		Vector3 right = Vector3.Cross (forward , Vector3.up);
		Vector3 rotateDir = Vector3.down;

//		transform.Rotate (rotateDir * directZ, Time.fixedDeltaTime * 200f  * turnForce );
//		m_rigidbody.AddForce (forward * directX * forwardForce  + directionZ * directZ * turnForce * m_rigidbody.velocity.magnitude);

		// the force is:
		// forward force => the force to push the object to move forward
		// side force => the force to rotate the object
		// the side force is calculate by :
		// f_s = mv^2/r = m*omiga*v = mass * angleSpeed * velocity
		Vector3 force = forward * inputForward * forwardForce + // force in forward direction
			right * turnRate * m_rigidbody.velocity.magnitude * m_rigidbody.mass * inputLeft ; // force in side

		// add force to rigidbody
		// notice that this will not rotate the object
		m_rigidbody.AddForce( force );
		// manually rotate the object
		transform.Rotate (rotateDir * inputLeft, Time.deltaTime * turnRate * Mathf.Rad2Deg );

//		if (Mathf.Abs (directZ) > 0) {
//			m_rigidbody.AddTorque(rotateDir * directZ * turnForce * m_rigidbody.velocity.sqrMagnitude);
//		} else {
//			m_rigidbody.angularVelocity = Vector3.zero;
//		}


//		Vector3 pushForce = ( direction * directX * forwardForce  + directionZ * directZ * turnForce * m_rigidbody.velocity.magnitude );

//		m_rigidbody.AddForce ( pushForce );

//		if (m_rigidbody.velocity.sqrMagnitude > limitSpeed)
//			m_rigidbody.velocity *= 0.95f;

//		if (m_rigidbody.velocity.magnitude > Mathf.Epsilon && m_rigidbody.velocity.magnitude > 0.001f) {
//				transform.forward = m_rigidbody.velocity.normalized;
//		}


		// calculate the reward of each frame
		// the vector of the object to target
		Vector3 disToDes = (target.transform.position - transform.position);

//		reward = -0.005f;
		// the reward is reduced every frame (punishment)
	    // because we want the car to approach the destination as fast as possible
		// the larger distance toward the target, the greater the punishment
		// the low velocity will also brings the punishment
		AddReward( -0.001f * Mathf.Pow( disToDes.magnitude , 2f ) *
			( m_rigidbody.velocity.magnitude < 0.1f ? 5f : 1f ) );

//		float squareRange = 5f;
//		if (gameObject.transform.position.y < 0f ||
//		    Mathf.Abs (transform.localPosition.x) > squareRange ||
//		    Mathf.Abs (transform.localPosition.z) > squareRange) {
//			Done ();
//			AddReward (-100f);
//		}


//		Debug.Log ("Force " + pushForce);
	}

	public override void AgentReset ()
	{
		transform.localPosition = area.ResetArea ();
		transform.LookAt (area.transform.position);

		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.angularVelocity = Vector3.zero;

//		float squareRange = 2f;
//		transform.localPosition = new Vector3 (Random.Range (-squareRange, squareRange), 0, Random.Range (-squareRange, squareRange));
	}

	public void GetToGoal()
	{
		Done ();

		AddReward (100f);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;

		for (int i = 0; i < detectAngle.Length; ++i) {
			float distance = 0;
			float angle = detectAngle [i];

			Vector3 direction = transform.TransformDirection (PolarToCartesian (1f, angle));
			direction.y = 0;

			RaycastHit hitInfo;

			if (Physics.Raycast (transform.position, direction, out hitInfo, detectRange)) {
				distance = hitInfo.distance;
			}

			Gizmos.DrawLine (transform.position, transform.position + direction * distance);
		}

//		Vector3 forward = m_rigidbody.transform.forward;
//		Vector3 leftSide = Vector3.Cross (forward, Vector3.down * 0.5f);
//		leftSide = Vector3.Lerp (forward, leftSide, 0.7f).normalized;
//		Vector3 rightSide = Vector3.Cross (forward, Vector3.up * 0.5f);
//		rightSide = Vector3.Lerp (forward, rightSide, 0.7f).normalized;
//
//		Gizmos.color = Color.red;
//
//		Gizmos.DrawLine (transform.position, transform.position + forward.normalized * forwardDis);
//		Gizmos.DrawLine (transform.position, transform.position + leftSide.normalized * leftDis);
//		Gizmos.DrawLine (transform.position, transform.position + rightSide.normalized * rightDis);
	}


}
