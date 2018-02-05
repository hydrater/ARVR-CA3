using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour {

	public int hp;
	NavMeshAgent agent;

	void Start () 
	{
		GetComponent<NavMeshAgent> ().SetDestination (GameManager.instance.AIDestination.position);
		agent.SetDestination (GameManager.instance.gameObject.transform.position);
	}

	void Update()
	{
		if (!agent.pathPending)
		{
			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
				{
					// Done
					GetComponent<Animator>().SetBool("attacking", true);
				}
			}
		}
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
