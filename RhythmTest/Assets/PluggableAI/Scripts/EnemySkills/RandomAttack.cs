﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAttack : EnemySkills {

    private GameObject user;
    private GameObject spells;

    private int columnStartIndex;
    private int columnEndIndex;
    private int rowStartIndex;
    private int rowEndIndex;
    private int attacksPerRow;

    private List<int> rowValues;
    private List<List<int>> chosenAttackGrid;

    public RandomAttack(int chargeRequired, GameObject user, int attacksPerRow) : base(chargeRequired) {
        this.user = user;
        spells = Resources.Load<GameObject>("Prefabs/Spells");

        columnStartIndex = -2;
        columnEndIndex = 2;
        rowStartIndex = -4;
        rowEndIndex = 4;
        this.attacksPerRow = attacksPerRow;
        
        setupRowValues();
        setupChosenAttackGrid();
    }

    private void setupRowValues() {
        rowValues = new List<int>();
        for (int i = rowStartIndex; i <= rowEndIndex; i++) {
            rowValues.Add(i);
        }
    }

    private void setupChosenAttackGrid() {
        chosenAttackGrid = new List<List<int>>();
        List<int> temp = new List<int>();
        for (int i = columnStartIndex; i < columnEndIndex + 1; i++) {
            shuffle(rowValues);

            temp = new List<int>();
            for (int j = 0; j < attacksPerRow; j++) {
                temp.Add(rowValues[j]);
            }

            chosenAttackGrid.Add(new List<int>(temp));
        }
    }

    private void setupAndCastSpell() {
        Vector3 targetPosition;
        GameObject spell;

        for (int i = columnStartIndex; i < columnEndIndex + 1; i++)
        {
            List<int> currentRow = chosenAttackGrid[i + 2];
            for (int j = 0; j < currentRow.Count; j++)
            {

                targetPosition = new Vector3(currentRow[j], user.transform.position.y, i);
                spell = GameObject.Instantiate(spells);
                spell.GetComponentInChildren<Spells>().setup(user, targetPosition, "enemyRandomAttack");
            }
        }
        // only play sound once
        targetPosition = new Vector3(0, 0, 0);
        spell = GameObject.Instantiate(spells);
        spell.GetComponentInChildren<Spells>().setup(user, targetPosition, "enemyRandomAttackSound");

    }


    public override void doSkill() {
        Debug.Log("do random attack");

        for (int i = columnStartIndex; i < columnEndIndex + 1; i++) {
            List<int> currentRow = chosenAttackGrid[i + 2];
            for (int j = 0; j < currentRow.Count; j++) {
                DamageController.instance.checkAndDoDamageToPlayer(i, currentRow[j]);
            }
        }

        setupAndCastSpell();
    }

    public override void handleTelegraphAttack(Vector3 position, int stage) {
        switch (stage) {
            case 0:
                changeToOrignalMaterial(position);
                break;
            case 1:
                changeToFirstMaterial(position);
                break;
            case 2:
                changeToSecondMaterial(position);
                break;
        }
    }

    private void changeToFirstMaterial(Vector3 position) {
        for (int i = columnStartIndex; i < columnEndIndex + 1; i++) {
            List<int> currentRow = chosenAttackGrid[i + 2];
            for (int j = 0; j < currentRow.Count; j++) {
                Grid.instance.changeToFirstMaterial(i, currentRow[j]);
            }
        }
    }

    private void changeToSecondMaterial(Vector3 position) {
        for (int i = columnStartIndex; i < columnEndIndex + 1; i++) {
            List<int> currentRow = chosenAttackGrid[i + 2];
            for (int j = 0; j < currentRow.Count; j++) {
                Grid.instance.changeToSecondMaterial(i, currentRow[j]);
            }
        }
    }

    private void changeToOrignalMaterial(Vector3 position) {
        for (int i = columnStartIndex; i < columnEndIndex + 1; i++) {
            List<int> currentRow = chosenAttackGrid[i + 2];
            for (int j = 0; j < currentRow.Count; j++) {
                Grid.instance.changeToOriginalMaterial(i, currentRow[j]);
            }
        }
    }

    private void shuffle<T>(IList<T> list) {
        int count = list.Count;

        while (count > 1) {
            int k = Random.Range(0, count);
            count--;
            T temp = list[k];
            list[k] = list[count];
            list[count] = temp;
        }
    }
}
