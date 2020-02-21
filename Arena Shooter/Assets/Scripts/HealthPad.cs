﻿using Mirror;
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
	private float maxHealthRestored = 100f;

	public TMP_Text healthRestoreText;
	public GameObject pickupModel;
	private const float rotateSpeed = 30;

	void Update()
	{
		if (amountOfHealthRestored >= 100f)
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
		pickupModel.transform.localPosition = new Vector3(0, amountOfHealthRestored / 100 + healthIncreaseTimer / 10, 0);
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

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Collected");
		}
	}
}
