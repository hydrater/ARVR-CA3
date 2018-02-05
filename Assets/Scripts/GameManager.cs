using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public Transform[] spawnPoints;
	public int wave = 1, score = 0, cabinHP;
	public Text scoreUI, waveUI;
	static IEnumerator spawnLoop;
	public GameObject[] enemyUnits;
	public Transform AIDestination;
	[HideInInspector]
	public float enemyCount = 5;

	void Awake()
	{
		instance = this;
		spawnLoop = spawnMachine ();
		spawnPoints = GameObject.Find ("SpawnPoints").GetComponentsInChildren<Transform>();
	}
	
	public void StartGame()
	{
		StartCoroutine(spawnMachine());
	}

	public void EndGame()
	{
		StopCoroutine(spawnMachine());
	}

	IEnumerator spawnMachine()
	{
		for (;;)
		{
			yield return new WaitForSeconds(Random.Range(1, (wave / (wave + 5)) - wave));
			int desinatedSpawn = Random.Range(0, spawnPoints.Length);

			// Code needs to change for other variants
			GameObject temp = Instantiate(enemyUnits[0], spawnPoints[desinatedSpawn].position, spawnPoints[desinatedSpawn].rotation);
			temp.GetComponent<EnemyUnit> ().hp = 1 + wave/3;

			do 
			{
				yield return null;
			} while (enemyCount > 0);

			++wave;
			enemyCount = 3 + wave * 2;
			waveUI.text = wave.ToString();
		}
	}

	public void UpdateScore()
	{
		++score;
		scoreUI.text = score.ToString ();
	}
}
