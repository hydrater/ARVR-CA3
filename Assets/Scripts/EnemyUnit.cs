using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour {

	public int hp;
    public GameObject BloodSplatter;
   
	NavMeshAgent agent;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		GetComponent<NavMeshAgent> ().SetDestination (GameManager.instance.AIDestination.position);
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
		if (hp <= 0)
		{
            GameManager.instance.enemyDieSound.Play();
			GameManager.instance.UpdateScore ();
			--GameManager.instance.enemyCount;
			Instantiate (BloodSplatter, transform.position, transform.rotation);
			Destroy (gameObject); //Replace with death animation
		}
	}

	public void AttackCabin()
	{
		GameManager.instance.DealDamageToCabin ();
	}

}
