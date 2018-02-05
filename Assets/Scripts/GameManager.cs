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

	void Awake()
	{
		instance = this;
		spawnLoop = spawnMachine ();
		spawnPoints = GameObject.Find ("SpawnPoints").GetComponentsInChildren<Transform>();
	}

	void Update()
	{
		
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
		float enemyCount = 5;
		for (;;)
		{
			yield return new WaitForSeconds(Random.Range(1, (wave / (wave + 5)) - wave));
			int desinatedSpawn = Random.Range(0, spawnPoints.Length);

			// Code needs to change for other variants
			Instantiate(enemyUnits[0], spawnPoints[desinatedSpawn].position, spawnPoints[desinatedSpawn].rotation);

			--enemyCount;
			if (enemyCount == 0)
			{
				++wave;
				enemyCount = 3 + wave * 2;
				waveUI.text = wave.ToString();
			}
		}
	}

	public void UpdateScore()
	{
		++score;
		scoreUI.text = score.ToString ();
	}
}
