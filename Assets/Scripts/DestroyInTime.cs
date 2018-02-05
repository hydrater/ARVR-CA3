using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour {

	void Start () {
		StartCoroutine (DestroyInSecond ());
	}

	IEnumerator DestroyInSecond()
	{
		yield return new WaitForSeconds (10);
		Destroy (this.gameObject);
	}
}
