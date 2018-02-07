using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public Transform[] spawnPoints;
	public int wave = 1, score = 0, cabinHP;
	public Text scoreUI, waveUI, ammoCount, cabinHealth, deathText;
	public GameObject[] enemyUnits;
	public Transform AIDestination;
	[HideInInspector]
	public float enemyCount = 5;
    public float timeLeft = 5f;

    public AudioSource inGameSound;
    public AudioSource gameEndSound;

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
        isRunning = false;
        deathText.text = " ";
    }

	void Update()
	{
        if (!isRunning)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                if (temp.IsGrabbed())
                {

                    StartGame();
                }
            }
        }
        Debug.Log(isRunning);
	}
	
	public void StartGame()
    {
        timeLeft = 5f;
        inGameSound.Play();
        isRunning = true;
		waveUI.text = "1";
		wave = 1;
		score = 0;
		scoreUI.text = "0";
		ammoCount.text = "Ammo 30/30";
		cabinHealth.text = "Health 100%";
		deathText.text = " ";
        cabinHP = 10;
        enemyCount = 5;
        game = spawnMachine();
        StartCoroutine(game);
    }

    public void EndGame()
    {
        StopCoroutine(game);
        inGameSound.Stop();
        gameEndSound.Play();
        isRunning = false;
		foreach (GameObject i in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			Destroy (i);
		}
		string DeathText = "Game Over!\nWaves Survived " + wave + "\nScore " + score;
		deathText.text = DeathText.Replace("\\n", "\n");
		ammoCount.text = " ";
		cabinHealth.text = " ";
        
    }

	IEnumerator spawnMachine()
	{
		for (;;)
		{
			float enemyToSpawn = enemyCount;
            Debug.Log(enemyToSpawn);
			for (float i = 0; i < enemyToSpawn; ++i)
			{
				yield return new WaitForSeconds(Random.Range(3, 5));
				int desinatedSpawn = Random.Range(0, spawnPoints.Length);

				// Code needs to change for other variants
				GameObject temp = Instantiate(enemyUnits[0], spawnPoints[desinatedSpawn].position, spawnPoints[desinatedSpawn].rotation);
				temp.GetComponent<EnemyUnit> ().hp = 1 + wave/3;
                Debug.Log("reached");
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
		cabinHealth.text = "Health " + cabinHP * 10 + "%";
		if (cabinHP < 1)
		{
			EndGame ();
		}
	}
}
