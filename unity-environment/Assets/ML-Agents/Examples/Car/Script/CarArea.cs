using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarArea : MonoBehaviour {

	[SerializeField]CarAcademy academy;
	[SerializeField] List<GameObject> blocks;
	[SerializeField] float areaSize;
//	[SerializeField] float startSize;
//	[SerializeField] float targetSize;
	[SerializeField] GameObject target;
	[SerializeField]  bool randomBlocks = false;

	// Use this for initialization
	void Start () {
		academy = FindObjectOfType<CarAcademy> ();	
	}

	public Vector3 ResetArea()
	{
//		targetSize  = academy.targetSize;
//		Debug.Log( "Reset Area");
		Vector3 startPos = Vector3.zero;
		if (randomBlocks) {
			ResetBlocks ();

			startPos = GetRandomEdge ();

			RaycastHit hitInfo;
			while (Physics.Raycast (new Vector3 (startPos.x, 10f, startPos.z) + transform.position, Vector3.down, out hitInfo, 20)) {
//			Debug.Log ("Hit " + hitInfo.collider.name);
				if (hitInfo.collider.name.Contains ("Block")) {
					startPos = GetRandomEdge ();
					ResetBlocks ();
				} else
					break;
			}

			Vector3 targetPos = -startPos;

			targetPos.x = targetPos.x < 0 ? -areaSize + 0.5f : areaSize - 0.5f;
			targetPos.z = targetPos.z < 0 ? -areaSize + 0.5f : areaSize - 0.5f;

			target.transform.localPosition = targetPos;


		} else {
			startPos = -target.transform.localPosition + new Vector3( Random.Range(-0.5f , 0.5f) , 0 , Random.Range(-0.5f,0.5f));
		}

		return startPos;
	}

	public void ResetBlocks()
	{
		if (randomBlocks) {
			int num = Random.Range (blocks.Count / 2, blocks.Count);
			for (int i = 0; i < blocks.Count; ++i) {
				if (i < num) {
					blocks [i].SetActive (true);
					blocks [i].transform.localPosition = new Vector3 (
						Random.Range (-areaSize, areaSize),
						0,
						Random.Range (-areaSize, areaSize));
					blocks [i].transform.localScale = new Vector3 (
						Random.Range (0.5f, 2f),
						1f,
						Random.Range (0.5f, 2f));
				} else {
					blocks [i].SetActive (false);
				}
			}
		}
	}

	public Vector3 GetRandomEdge()
	{
		float sgnX = Random.Range (0, 1f) > 0.5f ? 1f : -1f;
		float sgnZ = Random.Range (0, 1f) > 0.5f ? 1f : -1f;
		return new Vector3 (Random.Range (0.75f, 1f) * areaSize * sgnX,
			0 ,
			Random.Range (0.75f, 1f) * areaSize * sgnZ);
	}


	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube (transform.position, new Vector3 (areaSize * 2f , 1f, areaSize * 2f )); 
	}
}
