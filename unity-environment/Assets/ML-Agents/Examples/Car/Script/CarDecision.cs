﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarDecision : MonoBehaviour, Decision {


	public float[] Decide(
		List<float> vectorObs,
		List<Texture2D> visualObs,
		float reward,
		bool done,
		List<float> memory)
	{
		return new float[2]{ 0, 0 };
	}

	public List<float> MakeMemory(
		List<float> vectorObs,
		List<Texture2D> visualObs,
		float reward,
		bool done,
		List<float> memory)
	{
		return new List<float>();
	}
}
