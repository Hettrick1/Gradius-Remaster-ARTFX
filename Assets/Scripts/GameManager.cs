using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int waveNumber, numberOfEnemies;
    [SerializeField] GameObject enemies;
    [SerializeField] GameObject boss;

    [SerializeField] Transform[] positions;

    private List<List<GameObject>> waveParts = new List<List<GameObject>>();

    int moveState1, moveState2, moveState3;

    bool justRevived, beginning = true, isInWave, part1, part2, part3;

    public static GameManager instance;

    private void Start()
    {
        instance = this;
        waveParts.Add(new List<GameObject>());
        waveParts.Add(new List<GameObject>());
        waveParts.Add(new List<GameObject>());
    }

    private void Update()
    {
        if (!beginning)
        {
            bool allEnemiesDead = true;

            for (int i = 0; i < waveParts.Count; i++)
            {
                for (int j = 0; j < waveParts[i].Count; j++)
                {
                    if (!waveParts[i][j].GetComponent<EnemyBehavior>().HasDied())
                    {
                        allEnemiesDead = false;
                        break;
                    }
                }

                if (!allEnemiesDead)
                    break;
            }

            if (allEnemiesDead)
            {
                isInWave = false;
                NewWave();
            }
        }
        else
        {
            beginning = false;
            NewWave();
        }
        if (part1)
        {
            for (int i = 0; i < waveParts[0].Count; i++)
            {
                waveParts[0][i].GetComponent<EnemyBehavior>().SetJustSpawned(true);
                if (waveParts[0][i].transform.position.x > positions[3].position.x)
                {
                    waveParts[0][i].GetComponent<EnemyBehavior>().SetJustSpawned(false);
                    waveParts[0][i].GetComponent<EnemyBehavior>().ChooseMoveState(moveState1);
                }
            }
        }
        if (part2)
        {
            for (int i = 0; i < waveParts[1].Count; i++)
            {
                waveParts[1][i].GetComponent<EnemyBehavior>().SetJustSpawned(true);
                if (waveParts[1][i].transform.position.x > positions[4].position.x)
                {
                    waveParts[1][i].GetComponent<EnemyBehavior>().SetJustSpawned(false);                    
                    waveParts[1][i].GetComponent<EnemyBehavior>().ChooseMoveState(moveState2);
                }
            }
        }
        if (part3)
        {
            for (int i = 0; i < waveParts[2].Count; i++)
            {
                waveParts[2][i].GetComponent<EnemyBehavior>().SetJustSpawned(true);
                if (waveParts[2][i].transform.position.x > positions[4].position.x)
                {
                    waveParts[2][i].GetComponent<EnemyBehavior>().SetJustSpawned(false);                   
                    waveParts[2][i].GetComponent<EnemyBehavior>().ChooseMoveState(moveState3);
                }

            }
        }
    }

    private void NewWave()
    {
        if (!isInWave)
        {
            part1 = false; 
            part2 = false; 
            part3 = false;
            isInWave = true;
            if (!justRevived)
            {
                moveState1 = ChooseState();
                moveState2 = ChooseState();
                moveState3 = ChooseState();
                for (int i = 0; i < waveParts.Count; i++)
                {
                    waveParts[i].Clear();
                    numberOfEnemies = Random.Range(2, 4);
                    for (int j = 0; j < numberOfEnemies; j++)
                    {
                        waveParts[i].Add(Instantiate(enemies, positions[j].position, Quaternion.identity));
                    }
                }
            }
            Invoke(nameof(Part1), 0.1f);
            Invoke(nameof(Part2), 5f);
            Invoke(nameof(Part3), 10f);
        }  
    }

    void Part1()
    {
        part1 = true; 
    }
    void Part2()
    {
        part2 = true;
    }
    void Part3()
    {
        part3 = true;
    }

    private int ChooseState()
    {
        return Random.Range(1, 4);
    }
}
