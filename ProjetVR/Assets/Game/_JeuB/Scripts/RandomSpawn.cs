using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using NaughtyAttributes;
using System.Linq;
using Unity.VisualScripting;

[ExecuteInEditMode]
public class RandomSpawn : MonoBehaviour
{
    [Header("Spawner Characteristics")]
    [SerializeField] public List<GameObject> spawnerList = new();
    private List<GameObject> _availableSpawnerList = new();
    [SerializeField] public GameObject[] mobArray;

    [Header("Spawned Mob Parameters")]
    [SerializeField] ProtectedToothBehaviours target;
    [SerializeField] GameObject rotationPoint;
    [SerializeField] GameObject mobRotator;

    [Header("Difficulty Parameters")]
    [SerializeField] float spawnInterval;
    [SerializeField] float mobSpeed;

    [SerializeField] bool isNumberSpawnerBased;
    [SerializeField] [ShowIf("isNumberSpawnerBased")] [Range(1, 6)] int numberSpawnerActivated;
    [SerializeField] [HideIf("isNumberSpawnerBased")] bool spawnerTop;
    [SerializeField] [HideIf("isNumberSpawnerBased")] bool spawnerTopRight;
    [SerializeField] [HideIf("isNumberSpawnerBased")] bool spawnerBottomRight;
    [SerializeField] [HideIf("isNumberSpawnerBased")] bool spawnerBottom;
    [SerializeField] [HideIf("isNumberSpawnerBased")] bool spawnerBottomLeft;
    [SerializeField] [HideIf("isNumberSpawnerBased")] bool spawnerTopLeft;

    [Space(10)]

    [SerializeField] int weightEnemy1;
    [SerializeField] int weightEnemy2;
    [SerializeField] int weightEnemy3;

    private int _percentageEnemy1;
    private int _percentageEnemy2;
    private int _percentageEnemy3;

    [NaughtyAttributes.ReadOnly] [SerializeField] string CurrentPercentageEnemy1;
    [NaughtyAttributes.ReadOnly] [SerializeField] string CurrentPercentageEnemy2;
    [NaughtyAttributes.ReadOnly] [SerializeField] string CurrentPercentageEnemy3;


    [Header("Progression Parameters")]
    [SerializeField] bool enableProgression = true; 
    [NaughtyAttributes.ReadOnly] [SerializeField] int milestoneCount = 0;
    [SerializeField] int interval = 10;

    [SerializeField] DifficultyPresets[] tutorialDifficulties;
    [SerializeField] DifficultyPresets[] difficultyPresets;

    [Header("Death Event")]
    [SerializeField] UnityEvent allShooted = new();

    private static bool skipTutorial = false;

    void OnEnable()
    {
        if (enableProgression) InvokeRepeating(nameof(CountMinutes), 0, interval);

        milestoneCount = 0;

        target.gameObject.SetActive(true);

        skipTutorial = true;

        GameManager.Instance.gameStopped.AddListener(() => skipTutorial = false);
    }

    private void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();

        GameManager.Instance.gameStopped.RemoveListener(() => skipTutorial = false);
    }

    private void Start()
    {
        target.onDeath.AddListener(StopAllCoroutines);
    }

    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying) SpawnPercentage();
    }

    public void StartSpawn()
    {
        CreateAvailableSpawnerList();
        StartCoroutine(SpawnCycle());
    }

    private IEnumerator SpawnCycle()
    {
        while (target is not null)
        {
            (GameObject, GameObject) selectedMobAndSpawner = SelectRandomMobAndSpawner();
            SpawnMob(selectedMobAndSpawner.Item1, selectedMobAndSpawner.Item2);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private (GameObject, GameObject) SelectRandomMobAndSpawner()
    {
        if (isNumberSpawnerBased) return (_availableSpawnerList[Random.Range(0, numberSpawnerActivated)], SelectByPercentage());
        return (_availableSpawnerList[Random.Range(0, _availableSpawnerList.Count)], SelectByPercentage());
    }

    private void SpawnMob(GameObject _spawner, GameObject _mob)
    {
        GameObject newMob = Instantiate(_mob, _spawner.transform);
        Mob mobBehaviors = newMob.GetComponent<Mob>();
        mobBehaviors.moveSpeed = mobSpeed;

        if(mobBehaviors.canRotate) AddRotator(newMob);

        if (mobBehaviors != null) mobBehaviors.target = target.transform;
    }

    private void AddRotator(GameObject newMob)
    {
        GameObject newParent = Instantiate(mobRotator, rotationPoint.transform);
        newMob.transform.parent = newParent.transform;
    }


    private void SpawnPercentage()
    {
        float frequencyEnemy1 = weightEnemy1;
        float frequencyEnemy2 = weightEnemy2 + frequencyEnemy1;
        float frequencyEnemy3 = weightEnemy3 + frequencyEnemy2;
        
        _percentageEnemy1 = CalculatePercentage(frequencyEnemy1, frequencyEnemy3);
        _percentageEnemy2 = CalculatePercentage(frequencyEnemy2, frequencyEnemy3);
        _percentageEnemy3 = CalculatePercentage(frequencyEnemy3, frequencyEnemy3);
        
        CurrentPercentageEnemy1 = _percentageEnemy1.ToString() +  "%";
        CurrentPercentageEnemy2 = (_percentageEnemy2 - _percentageEnemy1).ToString() +  "%";
        CurrentPercentageEnemy3 = (_percentageEnemy3 - _percentageEnemy2).ToString() +  "%";
    }

    private int CalculatePercentage(float mainFrequency, float otherFrequency) => (int)((mainFrequency/otherFrequency)*100);

    public GameObject SelectByPercentage()
    {
        SpawnPercentage();
        System.Random _rnd = new System.Random();
        int drop = _rnd.Next(0, 100);
        if (drop <= _percentageEnemy1) return mobArray[0];
        else if (drop <= _percentageEnemy2 && drop > _percentageEnemy1) return mobArray[1];
        else if (drop <= _percentageEnemy3 && drop > _percentageEnemy2) return mobArray[2];
        return null;
    }

    private void CreateAvailableSpawnerList()
    {
        _availableSpawnerList.Clear();

        if (isNumberSpawnerBased)
        {
            _availableSpawnerList = spawnerList;
            _availableSpawnerList.Shuffle();
            return;
        }

        if (spawnerTop) _availableSpawnerList.Add(spawnerList[0]);
        if (spawnerTopRight) _availableSpawnerList.Add(spawnerList[1]);
        if (spawnerBottomRight) _availableSpawnerList.Add(spawnerList[2]);
        if (spawnerBottom) _availableSpawnerList.Add(spawnerList[3]);
        if (spawnerBottomLeft) _availableSpawnerList.Add(spawnerList[4]);
        if (spawnerTopLeft) _availableSpawnerList.Add(spawnerList[5]);
    }

    private void CountMinutes()
    {
        if (milestoneCount < tutorialDifficulties.Length && !skipTutorial)
            ChangeDifficulty(tutorialDifficulties[milestoneCount]);
        else
            ChangeDifficulty(difficultyPresets[milestoneCount - tutorialDifficulties.Length]);
        milestoneCount += 1;

        if (milestoneCount == difficultyPresets.Length + tutorialDifficulties.Length)
            CancelInvoke();

        Debug.Log(milestoneCount);
    }

    public void ChangeInterval(float newInterval) => spawnInterval = newInterval;
    public void ChangeMilestoneInterval(int newInterval) => interval = newInterval;
    public void ChangeNumberSpawner(int newNumber, bool newIsNumberSpawnerBased ,params bool[] newSpawners)
    {
        numberSpawnerActivated = newNumber;
        isNumberSpawnerBased = newIsNumberSpawnerBased;
        spawnerTop = newSpawners[0];
        spawnerTopRight = newSpawners[1];
        spawnerBottomRight = newSpawners[2];
        spawnerBottom = newSpawners[3];
        spawnerBottomLeft = newSpawners[4];
        spawnerTopLeft = newSpawners[5];
    } 

    public void ChangeWeightEnemy1(int newWeightsEnemy1) => weightEnemy1 = newWeightsEnemy1;
    public void ChangeWeightEnemy2(int newWeightsEnemy2) => weightEnemy2 = newWeightsEnemy2;
    public void ChangeWeightEnemy3(int newWeightsEnemy3) =>  weightEnemy3 = newWeightsEnemy3;
    public void ChangeMobSpeed(float speed) => mobSpeed = speed;

    public void ChangeDifficulty(DifficultyPresets preset)
    {
        ChangeMilestoneInterval(preset.milestoneInterval);
        ChangeInterval(preset.spawnInterval);
        ChangeMobSpeed(preset.mobSpeed);
        ChangeNumberSpawner(preset.numberSpawner, preset.isNumberSpawnerBased, preset.spawnerTop, preset.spawnerTopRight, preset.spawnerBottomRight, preset.spawnerBottom, preset.spawnerBottomLeft, preset.spawnerTopLeft);
        ChangeWeightEnemy1(preset.weightEnemy1);
        ChangeWeightEnemy2(preset.weightEnemy2);
        ChangeWeightEnemy3(preset.weightEnemy3);
        CreateAvailableSpawnerList();
        SpawnPercentage();
    }
}
