using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour {

	public int buttonID;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			switch (buttonID)
			{
			case 0:
					Application.Quit();
					break;
			}
		}
	}
}
