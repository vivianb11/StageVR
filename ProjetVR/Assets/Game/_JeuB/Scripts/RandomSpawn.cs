using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using NaughtyAttributes;
using System.Linq;

namespace JeuB
{
    public enum MobType
    {
        MobDroit,
        MobBounce,
        MobRotate
    }

    [ExecuteInEditMode]
    public class RandomSpawn : MonoBehaviour
    {

        [Header("Spawner Characteristics")]
        [SerializeField] public List<GameObject> spawnerList = new();
        private List<GameObject> _availableSpawnerList = new();
        [SerializeField] public GameObject[] mobArray;
        [SerializeField] public GameObject[] bonusArray;

        [Header("Spawned Mob Parameters")]
        [SerializeField] ProtectedToothBehaviours target;
        [SerializeField] GameObject rotationPoint;
        [SerializeField] GameObject mobRotator;

        [Header("Difficulty Parameters")]
        [SerializeField] float spawnInterval;
        [SerializeField] float bonusSpawnMinInterval;
        [SerializeField] float bonusSpawnMaxInterval;
        [SerializeField] float bonusSpawnDelay;
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
        [SerializeField] bool weightBased = true;
        [SerializeField] [HideIf("weightBased")] MobType[] mobTypePerSpawner = new MobType[6];

        [Space(10)]

        [SerializeField] [ShowIf("weightBased")] int weightEnemy1;
        [SerializeField] [ShowIf("weightBased")] int weightEnemy2;
        [SerializeField] [ShowIf("weightBased")] int weightEnemy3;

        private int _percentageEnemy1;
        private int _percentageEnemy2;
        private int _percentageEnemy3;

        [NaughtyAttributes.ReadOnly] [SerializeField] [ShowIf("weightBased")] string CurrentPercentageEnemy1;
        [NaughtyAttributes.ReadOnly] [SerializeField] [ShowIf("weightBased")] string CurrentPercentageEnemy2;
        [NaughtyAttributes.ReadOnly] [SerializeField] [ShowIf("weightBased")] string CurrentPercentageEnemy3;


        [Header("Progression Parameters")]
        [SerializeField] bool enableProgression = true; 
        [NaughtyAttributes.ReadOnly] [SerializeField] int currentLevel = 0;
        [SerializeField] int maxLevel;
        [SerializeField] PulsatingText pulsatingTextBehavior;
        [NaughtyAttributes.ReadOnly] [SerializeField] int milestoneCount = 0;
        [Button("Skip")] private void Skip() 
        {
            CountMinutes();
            CancelInvoke();
        }
    
        [SerializeField] int interval = 10;

        [NaughtyAttributes.ReadOnly] [SerializeField] DifficultyPresets currentPreset;
        [SerializeField] DifficultyPresets deathPreset;

        [SerializeField] DifficultyPresets[] tutorialDifficulties;
        [SerializeField] int tutorialDifficultiesCount = 0;
        [SerializeField] DifficultyPresets[] difficultyPresets;
        [SerializeField] int difficultyPresetsCount = 0;

        [Header("Death Event")]
        [SerializeField] UnityEvent allShooted = new();

        private static bool _skipTutorial = false;
        [SerializeField] bool skipTutorial;

        void OnEnable()
        {
            if (enableProgression) Invoke(nameof(CountMinutes), interval);

            milestoneCount = currentLevel = 0;

            target.gameObject.SetActive(true);

#if UNITY_EDITOR
            _skipTutorial = skipTutorial;
#endif
        }

        private void OnDisable()
        {
            CancelInvoke();
            StopAllCoroutines();

            //GameManager.Instance.gameStopped.RemoveListener(() => _skipTutorial = false);
        }

        private void Start()
        {
            target.onDeath.AddListener(StopAllCoroutines);
            //GameManager.Instance.gameStopped.AddListener(() => _skipTutorial = false);
        }

        private void Update()
        {
            if (Application.isEditor && !Application.isPlaying) SpawnPercentage();

            if (OVRInput.Get(OVRInput.RawButton.X)) Order66();
        }

        public void StartSpawn()
        {
            CreateAvailableSpawnerList();
            StartCoroutine(SpawnCycle());
            Invoke(nameof(SpawnBonus), bonusSpawnDelay);
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

        private void SpawnBonus()
        {
            int randomBonusIndex = UnityEngine.Random.Range(0, bonusArray.Length-1);
            int randomSpawnerIndex = UnityEngine.Random.Range(0, spawnerList.Count-1);
            var mob = Instantiate(bonusArray[randomBonusIndex], spawnerList[randomSpawnerIndex].transform);
            var mobBehaviors = mob.GetComponent<Entity>();
            if (mobBehaviors != null) mobBehaviors.target = target.transform;
            Invoke(nameof(SpawnBonus), UnityEngine.Random.Range(bonusSpawnMinInterval, bonusSpawnMaxInterval));
        }

        private (GameObject, GameObject) SelectRandomMobAndSpawner()
        {
            if (isNumberSpawnerBased && weightBased) return (_availableSpawnerList[UnityEngine.Random.Range(0, numberSpawnerActivated)], SelectByPercentage());
            else if (!isNumberSpawnerBased && !weightBased)
            {
                int randomIndex = UnityEngine.Random.Range(0, _availableSpawnerList.Count);
                return (_availableSpawnerList[randomIndex], SelectBySpawner(randomIndex));
            }
            return (_availableSpawnerList[UnityEngine.Random.Range(0, _availableSpawnerList.Count)], SelectByPercentage());
        }

        private void SpawnMob(GameObject _spawner, GameObject _mob)
        {
            GameObject newMob = Instantiate(_mob, _spawner.transform);
            Entity mobBehaviors = newMob.GetComponent<Entity>();
            mobBehaviors.moveSpeed = mobSpeed;

            //if(mobBehaviors.canRotate) AddRotator(newMob);

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

        public GameObject SelectBySpawner(int index)
        {
            if (mobTypePerSpawner[index] is MobType.MobDroit) return mobArray[0];
            else if (mobTypePerSpawner[index] is MobType.MobBounce) return mobArray[1];
            else if (mobTypePerSpawner[index] is MobType.MobRotate) return mobArray[2];
            return null;
        }

        private void CreateAvailableSpawnerList()
        {

            if (isNumberSpawnerBased)
            {
                _availableSpawnerList = spawnerList;
                _availableSpawnerList.Shuffle();
                return;
            }
        
            var tempList = new List<GameObject>();

            if (spawnerTop) tempList.Add(spawnerList[0]);
            if (spawnerTopRight) tempList.Add(spawnerList[1]);
            if (spawnerBottomRight) tempList.Add(spawnerList[2]);
            if (spawnerBottom) tempList.Add(spawnerList[3]);
            if (spawnerBottomLeft) tempList.Add(spawnerList[4]);
            if (spawnerTopLeft) tempList.Add(spawnerList[5]);

            _availableSpawnerList = tempList;
        }


        private void CountMinutes()
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying) return;
            #endif

            if (tutorialDifficultiesCount < tutorialDifficulties.Length && !_skipTutorial)
            {
                ChangeDifficulty(tutorialDifficulties[tutorialDifficultiesCount]);
                tutorialDifficultiesCount++;
            }
            else
            {
                _skipTutorial = true;
                ChangeDifficulty(difficultyPresets[difficultyPresetsCount]);
                difficultyPresetsCount++;
            }

            if (difficultyPresetsCount == difficultyPresets.Length)
            {
                _skipTutorial = true;
                CancelInvoke();
                return;
            }

            Invoke(nameof(CountMinutes), interval);
        }

        private void LevelPassed()
        {
            pulsatingTextBehavior.disappear = true;

            if (currentLevel < maxLevel)
            {
                currentLevel += 1;   
                pulsatingTextBehavior.textMesh.text = "Niveau: " + currentLevel.ToString();
            }
            else pulsatingTextBehavior.textMesh.text = "Niveau: Bonus";
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

        public void ChangeWeightEnemy(ref int weightsEnemy, int newWeightsEnemy) => weightsEnemy = newWeightsEnemy;
        public void ChangeMobSpeed(float speed) => mobSpeed = speed;
        public void ChangeMobTypePerSpawner(MobType[] newMobTypePerSpawner) => mobTypePerSpawner = newMobTypePerSpawner;
        public void ChangeWeightBased(bool newWeightBased) => weightBased = newWeightBased;



        public void ChangeDifficulty(DifficultyPresets preset)
        {
            if (preset.levelPassed) LevelPassed();
            currentPreset = preset;
            milestoneCount += 1;
            ChangeMilestoneInterval(preset.milestoneInterval);
            ChangeInterval(preset.spawnInterval);
            ChangeMobSpeed(preset.mobSpeed);
            ChangeNumberSpawner(preset.numberSpawner, preset.isNumberSpawnerBased, preset.spawnerTop, preset.spawnerTopRight, preset.spawnerBottomRight, preset.spawnerBottom, preset.spawnerBottomLeft, preset.spawnerTopLeft);
            ChangeWeightEnemy(ref weightEnemy1, preset.weightEnemy1);
            ChangeWeightEnemy(ref weightEnemy2, preset.weightEnemy2);
            ChangeWeightEnemy(ref weightEnemy3, preset.weightEnemy3);
            ChangeMobTypePerSpawner(preset.mobTypePerSpawner);
            ChangeWeightBased(preset.weightBased);
            CreateAvailableSpawnerList();
            SpawnPercentage();
        }

        public void Order66()
        {
            CancelInvoke(nameof(CountMinutes));
            ChangeDifficulty(deathPreset);
        }
    }
}
