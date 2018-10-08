﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPScreen : MonoBehaviour {

    public Player player;
    public Enemy enemy;
    public float playerHP;
    public float enemyHP;
    private TextMesh thisText;


	// Use this for initialization
	void Start () {
        EventManager.StartListening("ReducePlayerHP", ReducePlayerHP);
        EventManager.StartListening("ReduceEnemyHP", ReduceEnemyHP);
        thisText = this.GetComponent<TextMesh>();
        playerHP = player.GetComponent<Player>().playerHealth;
        enemyHP = enemy.GetComponent<Enemy>().health;
        updateText();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ReducePlayerHP() {
        if (playerHP > 0) {
            playerHP--;
        }
        updateText();
    }

    private void ReduceEnemyHP() {
        if (enemyHP > 0) {
            enemyHP--;
        }
        updateText();
    }

    private void updateText() {
        thisText.text = "Your HP: " + playerHP + "\nEnemy HP: " + enemyHP;
    }
}
