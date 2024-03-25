using System.Collections;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine.Events;

[ExecuteInEditMode]
public class RandomSpawn : MonoBehaviour
{
    [Header("Spawner Characteristics")]
    [SerializeField] public GameObject[] spawnerArray;
    [SerializeField] public GameObject[] mobArray;

    [Header("Spawned Mob Parameters")]
    [SerializeField] GameObject target;
    [SerializeField] GameObject mobRotator;

    [Header("Difficulty Parameters")]
    [SerializeField] float spawnInterval;
    [SerializeField] [Range(1, 8)] int numberSpawnerActivated;
    [SerializeField] [Range(1, 2)] int enemyTypeAvailable;

    [Space(10)]

    [SerializeField] int weightEnemy1;
    [SerializeField] int weightEnemy2;

    private int _percentageEnemy1;
    private int _percentageEnemy2;

    [NaughtyAttributes.ReadOnly] [SerializeField] string CurrentPercentageEnemy1;
    [NaughtyAttributes.ReadOnly] [SerializeField] string CurrentPercentageEnemy2;


    [Header("Progression Parameters")]
    [SerializeField] int countSpent = 0;
    [SerializeField] int interval = 10;
    [SerializeField] UnityEvent progressionMilestone = new();

    void Start()
    {
        StartCoroutine(SpawnCycle());
        CountMinutes();
    } 

    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying) SpawnPercentage();
    }

    private IEnumerator SpawnCycle()
    {
        while (true)
        {
            (GameObject, GameObject) selectedMobAndSpawner = SelectRandomMobAndSpawner();
            SpawnMob(selectedMobAndSpawner.Item1, selectedMobAndSpawner.Item2);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private (GameObject, GameObject) SelectRandomMobAndSpawner()
    {
        if (enemyTypeAvailable is 1) return (spawnerArray[Random.Range(0, numberSpawnerActivated)], mobArray[0]);
        return (spawnerArray[Random.Range(0, numberSpawnerActivated)], SelectByPercentage());
    } 

    private void SpawnMob(GameObject _spawner, GameObject _mob)
    {
        Debug.Log(_mob);
        GameObject newMob = Instantiate(_mob, _spawner.transform);
        Mob mobBehaviors = newMob.GetComponent<Mob>();

        if(mobBehaviors.canRotate) AddRotator(newMob);

        if (mobBehaviors != null) mobBehaviors.target = target.transform;
    }

    private void AddRotator(GameObject newMob)
    {
        GameObject newParent = Instantiate(mobRotator, target.transform);
        newMob.transform.parent = newParent.transform;
    }


    private void SpawnPercentage()
    {
        float frequencyEnemy1 = weightEnemy1;
        float frequencyEnemy2 = weightEnemy2 + frequencyEnemy1;

        _percentageEnemy1 = (int)((frequencyEnemy1/frequencyEnemy2)*100);
        _percentageEnemy2 = (int)((frequencyEnemy2/frequencyEnemy2)*100);

        CurrentPercentageEnemy1 = _percentageEnemy1.ToString() +  "%";
        CurrentPercentageEnemy2 = (_percentageEnemy2 - _percentageEnemy1).ToString() +  "%";
    }

    public GameObject SelectByPercentage()
    {
        SpawnPercentage();
        System.Random _rnd = new System.Random();
        int drop = _rnd.Next(0, 100);
        if (drop <= _percentageEnemy1) return mobArray[0];
        else if (drop <= _percentageEnemy2 && drop > _percentageEnemy1) return mobArray[1];
        return null;
    }

    private void CountMinutes()
    {
        countSpent += 1;
        progressionMilestone.Invoke();
        Invoke("CountMinutes", interval);
    }

    public void ChangeInterval(int newInterval) => spawnInterval = newInterval;
    public void ChangeNumberSpawner(int newNumber) => numberSpawnerActivated = newNumber;
    public void ChangeEnemyAvailable(int newNumber) => enemyTypeAvailable = newNumber;
    public void ChangeWeightEnemy1(int newWeightsEnemy1) => weightEnemy1 = newWeightsEnemy1;
    public void ChangeWeightEnemy2(int newWeightsEnemy2) => weightEnemy2 = newWeightsEnemy2;
}
