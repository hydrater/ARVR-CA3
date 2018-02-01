using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour {

	public int hp;

	void Start () 
	{
		GetComponent<NavMeshAgent> ().SetDestination (GameManager.instance.AIDestination.position);
	}

	public void DealDamage(int damage)
	{
		hp -= damage;
		if (hp < 0)
		{
			GameManager.instance.UpdateScore ();
			Destroy (gameObject); //Replace with death animation
		}
	}
}
