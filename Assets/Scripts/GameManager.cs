using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Le boss et quand on recommence la vague
    // Quand le joueur meurt, il relance la vague
    // Faire de l'équilibrage
    // Les sons
    // Les menus
    // Le GameOver
    // Le score
    // Les feedbacks (score, camerashake, VFX)
    // Le power up temporaire

    [SerializeField] int waveNumber = 1, numberOfEnemies;
    [SerializeField] GameObject enemiesPrefabs;
    [SerializeField] GameObject bossPrefabs;
    GameObject playerGameObject, bossGameObject;
    Vector3 playerTransform;

    [SerializeField] float timeBetweenLaserShoot, timeBetweenMissileShoot, enemyMoveSpeed, enemyShootSpeed;

    [SerializeField] float enemiesLife, bossLife;

    [SerializeField] Transform[] positions;

    private List<List<GameObject>> waveParts = new List<List<GameObject>>();

    int moveState1, moveState2, moveState3, shootState1, shootState2, shootState3;

    public bool justRevived;
    bool beginning = true, isInWave, part1, part2, part3, isInBossFight,bossIsDead;

    public static GameManager instance;

    private void Start()
    {
        instance = this;
        playerGameObject = GameObject.FindWithTag("Player");
        playerTransform = playerGameObject.transform.position;
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
                    if (waveParts[i][j] != null && !waveParts[i][j].GetComponent<EnemyBehavior>().HasDied())
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
                for (int i = 0; i < waveParts.Count; i++)
                {
                    for (int j = 0; j < waveParts[i].Count; j++)
                    {
                        Destroy(waveParts[i][j].gameObject);
                    }
                }
                if (!isInBossFight)
                {
                    waveNumber++;
                }
                isInWave = false;
                if (waveNumber % 5 == 0)
                {
                    SpawnBoss();
                }
                else
                {
                    NewWave();
                }
            }
            if(isInBossFight && !bossIsDead && bossGameObject.GetComponent<EnemyBehavior>().HasDied())
            {
                bossIsDead = true;
                isInBossFight = false;
            }
            if (allEnemiesDead && bossIsDead && !isInBossFight)
            {
                bossIsDead = false;
                waveNumber++;
                isInWave = false;
                NewWave();
            }
        }
        else
        {
            beginning = false;
            NewWave();
        }
        if (part1 && !isInBossFight)
        {
            for (int i = 0; i < waveParts[0].Count; i++)
            {
                waveParts[0][i].GetComponent<EnemyBehavior>().SetJustSpawned(true);
                if (waveParts[0][i].transform.position.x > positions[3].position.x)
                {
                    waveParts[0][i].GetComponent<EnemyBehavior>().SetJustSpawned(false);
                    waveParts[0][i].GetComponent<EnemyBehavior>().ChooseMoveState(moveState1);
                    waveParts[0][i].GetComponent<EnemyBehavior>().ChooseShootState(shootState1);
                }
            }
        }
        if (part2 && !isInBossFight)
        {
            for (int i = 0; i < waveParts[1].Count; i++)
            {
                waveParts[1][i].GetComponent<EnemyBehavior>().SetJustSpawned(true);
                if (waveParts[1][i].transform.position.x > positions[4].position.x)
                {
                    waveParts[1][i].GetComponent<EnemyBehavior>().SetJustSpawned(false);                    
                    waveParts[1][i].GetComponent<EnemyBehavior>().ChooseMoveState(moveState2);
                    waveParts[1][i].GetComponent<EnemyBehavior>().ChooseShootState(shootState2);
                }
            }
        }
        if (part3 && !isInBossFight)
        {
            for (int i = 0; i < waveParts[2].Count; i++)
            {
                waveParts[2][i].GetComponent<EnemyBehavior>().SetJustSpawned(true);
                if (waveParts[2][i].transform.position.x > positions[5].position.x)
                {
                    waveParts[2][i].GetComponent<EnemyBehavior>().SetJustSpawned(false);                   
                    waveParts[2][i].GetComponent<EnemyBehavior>().ChooseMoveState(moveState3);
                    waveParts[2][i].GetComponent<EnemyBehavior>().ChooseShootState(shootState3);
                }
            }
        }
        if (justRevived)
        {
            if (isInBossFight)
            {
                bossGameObject.transform.position = positions[1].position;
                //
                //
                //  LA ON RAJOUTE LE REVIVE EN PHASE DE BOSS
                //
                //
            }
            else
            {
                isInWave = false;
                for (int i = 0; i < waveParts.Count; i++)
                {
                    for (int j = 0; j < waveParts[i].Count; j++)
                    {
                        waveParts[i][j].transform.position = positions[j].position;
                        waveParts[i][j].GetComponent<Rigidbody>().velocity = Vector3.zero;
                        waveParts[i][j].GetComponent<EnemyBehavior>().ChooseMoveState(2);
                        waveParts[i][j].GetComponent<EnemyBehavior>().ChooseShootState(1);
                        waveParts[i][j].GetComponent<EnemyBehavior>().Revive();
                    }
                }
                playerGameObject.transform.position = playerTransform;
                CancelInvoke();
                NewWave();
                GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
                for (int i = 0;i < projectiles.Length;i++)
                {
                    Destroy(projectiles[i]);
                }
                GameObject[] enemyProjectiles = GameObject.FindGameObjectsWithTag("EnemyProjectile");
                for (int i = 0; i < enemyProjectiles.Length; i++)
                {
                    Destroy(enemyProjectiles[i]);
                }
                justRevived = false;
            }
        }
    }

    private void NewWave()
    {
        if (!isInWave && !isInBossFight)
        {
            part1 = false; 
            part2 = false; 
            part3 = false;
            isInWave = true;
            if (!justRevived)
            {
                LevelUp();
                moveState1 = ChooseState();
                moveState2 = ChooseState();
                moveState3 = ChooseState();
                shootState1 = ChooseState();
                shootState2 = ChooseState();
                shootState3 = ChooseState();
                for (int i = 0; i < waveParts.Count; i++)
                {
                    waveParts[i].Clear();
                    numberOfEnemies = Random.Range(2, 4);
                    for (int j = 0; j < numberOfEnemies; j++)
                    {
                        waveParts[i].Add(Instantiate(enemiesPrefabs, positions[j].position, Quaternion.identity));
                    }
                }
            }
            Invoke(nameof(Part1), 0.1f);
            Invoke(nameof(Part2), 5f);
            Invoke(nameof(Part3), 10f);
        }  
    }

    private void SpawnBoss()
    {
        if (!isInWave && !isInBossFight)
        {
            print("boss");
            isInWave = true;
            isInBossFight = true;
            bossGameObject = Instantiate(enemiesPrefabs, positions[1].position, Quaternion.identity);
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

    void LevelUp()
    {
        if (timeBetweenLaserShoot > 0.1f)
        {
            timeBetweenLaserShoot -= 0.05f;
        }
        if (timeBetweenMissileShoot > 0.4f)
        {
            timeBetweenMissileShoot -= 0.05f;
        }
        if (enemyMoveSpeed < 4)
        {
            enemyMoveSpeed += 0.1f;
        }
        if (enemyShootSpeed < 1)
        {
            enemyShootSpeed += 0.05f;
        }
    }

    private int ChooseState()
    {
        return Random.Range(1, 4);
    }

    public float GetEnemiesLife()
    {
        return enemiesLife;
    }
    public float GetTimeBetweenLaserShoot()
    {
        return timeBetweenLaserShoot;
    }
    public float GetTimeBetweenMissileShoot()
    {
        return timeBetweenMissileShoot;
    }
    public float GetEnemyMoveSpeed()
    {
        return enemyMoveSpeed;
    }
    public float GetEnemyShootSpeed()
    {
        return enemyShootSpeed;
    }
    public float GetWaveNumber() 
    {
        return waveNumber; 
    }
}
