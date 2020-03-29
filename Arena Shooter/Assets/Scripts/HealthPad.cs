using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HealthPad : NetworkBehaviour
{
	private float amountOfHealthRestored = 5f;
	private float healthIncreaseRate = 0.1f;
	private float healthIncreaseTimer = 0;
	private float maxHealthRestored = 80f;

	public TMP_Text healthRestoreText;
	public GameObject pickupModel;
	private const float rotateSpeed = 30;

	void Update()
	{
		if (amountOfHealthRestored >= maxHealthRestored)
		{
			// Health pickup is fully charged
		}
		else
		{
			healthIncreaseTimer += Time.deltaTime;
			if (healthIncreaseTimer > healthIncreaseRate)
			{
				healthIncreaseTimer -= healthIncreaseRate;
				TickHealthIncrease();
			}
		}

		MoveModel();
	}

	private void MoveModel()
	{
		pickupModel.transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
		pickupModel.transform.localPosition = new Vector3(0, amountOfHealthRestored / 100 + healthIncreaseTimer / 10 + 0.5f, 0);
	}

	private void TickHealthIncrease()
	{
		amountOfHealthRestored += 1f;
		if(amountOfHealthRestored > maxHealthRestored)
		{
			amountOfHealthRestored = maxHealthRestored;
		}
		healthRestoreText.text = amountOfHealthRestored.ToString();
	}

	[Server]
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerEntityNetwork playerEntityNetwork = other.GetComponent<PlayerEntityNetwork>();
			float health = playerEntityNetwork.health;
			health += amountOfHealthRestored;
			if(health > 100)
			{
				health = 100;
			}
			playerEntityNetwork.health = health;
		}
		Destroy(gameObject);
	}
}
