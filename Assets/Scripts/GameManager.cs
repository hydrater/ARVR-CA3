using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public Transform[] spawnPoints;
	public int wave = 1, score = 0, cabinHP;
	public Text scoreUI, waveUI, ammoCount, cabinHealth;
	public GameObject[] enemyUnits;
	public Transform AIDestination;
	[HideInInspector]
	public float enemyCount = 5;

	public VRTK_InteractableObject temp;
	IEnumerator game;
	bool isRunning;

	void Awake()
	{
		instance = this;
		spawnPoints = GameObject.Find ("SpawnPoints").GetComponentsInChildren<Transform>();
		temp = GameObject.Find ("M4A1").GetComponent<VRTK_InteractableObject> ();
		game = spawnMachine();
		AIDestination = transform;

		StartGame ();
	}

	void Update()
	{
		if (temp.IsGrabbed() && !isRunning)
		{
			StartGame ();
		}
	}
	
	public void StartGame()
	{
		StartCoroutine (game);
		isRunning = true;
		waveUI.text = "1";
		wave = 1;
		score = 0;
		scoreUI.text = "0";
		ammoCount.text = "Ammo 30/30";
		cabinHealth.text = "Health 100%";
	}

	public void EndGame()
	{
		StopCoroutine(game);
		isRunning = false;
		foreach (GameObject i in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			Destroy (i);
		}
	}

	IEnumerator spawnMachine()
	{
		for (;;)
		{
			float enemyToSpawn = enemyCount;
			for (float i = 0; i < enemyToSpawn; ++i)
			{
				yield return new WaitForSeconds(Random.Range(3, 5));
				int desinatedSpawn = Random.Range(0, spawnPoints.Length);

				// Code needs to change for other variants
				GameObject temp = Instantiate(enemyUnits[0], spawnPoints[desinatedSpawn].position, spawnPoints[desinatedSpawn].rotation);
				temp.GetComponent<EnemyUnit> ().hp = 1 + wave/3;
			}

			do 
			{
				yield return null;
			} while (enemyCount > 0);

			++wave;
			enemyCount = 3 + wave * 2;
			waveUI.text = wave.ToString();
		}
	}

	public void updateAmmoCount(int bulletShot)
	{
		ammoCount.text = "Ammo " + (30 - bulletShot) + "/30";
	}

	public void UpdateScore()
	{
		++score;
		scoreUI.text = score.ToString ();
	}

	public void DealDamageToCabin()
	{
		--cabinHP;
		cabinHealth.text = "Health " + cabinHP;
		if (cabinHP < 1)
		{
			EndGame ();
		}
	}
}
