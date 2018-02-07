using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour {

	public int hp;
	NavMeshAgent agent;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		GetComponent<NavMeshAgent> ().SetDestination (GameManager.instance.AIDestination.position);
        hp = 10;
	}

	void Update()
	{
		if (Vector3.Distance(transform.position, GameManager.instance.AIDestination.position) < 2)
		{
			agent.enabled = false;
			GetComponent<Animator>().SetBool("attacking", true);
		}
	}

	public void DealDamage(int damage)
	{
		hp -= damage;
        Debug.Log(hp);
		if (hp <= 0)
		{
			GameManager.instance.UpdateScore ();
			--GameManager.instance.enemyCount;
			Instantiate (Resources.Load ("DeathParticle"), transform.position+ Vector3.down, transform.rotation);
			Destroy (gameObject); //Replace with death animation
		}
	}

	public void AttackCabin()
	{
		GameManager.instance.DealDamageToCabin ();
	}

}
