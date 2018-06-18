using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarTarget : MonoBehaviour {


	public CarAgent agent;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("agent") )
		{
			agent.GetToGoal ();
		}
	}
}
