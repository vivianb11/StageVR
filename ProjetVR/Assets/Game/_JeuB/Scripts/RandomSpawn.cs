using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        [Header("Objects")]
        [SerializeField] public List<GameObject> spawnerList = new();
        private List<GameObject> _availableSpawnerList = new();
        [SerializeField] public GameObject[] mobArray;
        [SerializeField] public GameObject[] bonusArray;


        [Space(10)]
        [Header("Spawned Mob Parameters")]
        [SerializeField] ProtectedToothBehaviours target;
        [SerializeField] GameObject rotationPoint;
        [SerializeField] GameObject mobRotator;
        [SerializeField] float mobSpeed;


        [Space(10)]
        [Header("Spawner Parameters")]
        [SerializeField] float spawnInterval;
        [SerializeField] float bonusSpawnMinInterval;
        [SerializeField] float bonusSpawnMaxInterval;
        [SerializeField] float bonusSpawnDelay;
        [SerializeField] bool isNumberSpawnerBased;
        [SerializeField] bool weightBased = true;


        [Space(10)]
        [Header("Locations & Mob Types")]
        [SerializeField][ShowIf("isNumberSpawnerBased")][Range(1, 6)] int numberSpawnerActivated;
        [SerializeField][HideIf("isNumberSpawnerBased")] bool spawnerTop;
        [SerializeField][HideIf("isNumberSpawnerBased")] bool spawnerTopRight;
        [SerializeField][HideIf("isNumberSpawnerBased")] bool spawnerBottomRight;
        [SerializeField][HideIf("isNumberSpawnerBased")] bool spawnerBottom;
        [SerializeField][HideIf("isNumberSpawnerBased")] bool spawnerBottomLeft;
        [SerializeField][HideIf("isNumberSpawnerBased")] bool spawnerTopLeft;
        [SerializeField][HideIf("weightBased")] MobType[] mobTypePerSpawner = new MobType[6];
        [ShowIf("weightBased")] public int weightEnemy1;
        [ShowIf("weightBased")] public int weightEnemy2;
        [ShowIf("weightBased")] public int weightEnemy3;
        private int _percentageEnemy1;
        private int _percentageEnemy2;
        private int _percentageEnemy3;
        [NaughtyAttributes.ReadOnly][SerializeField][ShowIf("weightBased")] string CurrentPercentageEnemy1;
        [NaughtyAttributes.ReadOnly][SerializeField][ShowIf("weightBased")] string CurrentPercentageEnemy2;
        [NaughtyAttributes.ReadOnly][SerializeField][ShowIf("weightBased")] string CurrentPercentageEnemy3;


        [Space(10)]
        [Header("Death Event")]
        [SerializeField] UnityEvent allShooted = new();

        void OnEnable()
        {
            target.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            CancelInvoke();
            StopAllCoroutines();
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
            if (GameManager.Instance.gamePaused)
                return;

            var mob = Instantiate(bonusArray.PickRandom(), spawnerList.PickRandom().transform);

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
            if (GameManager.Instance.gamePaused)
                return;

            GameObject newMob = Instantiate(_mob, _spawner.transform);
            Entity mobBehaviors = newMob.GetComponent<Entity>();
            mobBehaviors.moveSpeed = mobSpeed;
            mobBehaviors.target = target.transform;
        }

        public void SpawnPercentage()
        {
            float frequencyEnemy1 = weightEnemy1;
            float frequencyEnemy2 = weightEnemy2 + frequencyEnemy1;
            float frequencyEnemy3 = weightEnemy3 + frequencyEnemy2;

            _percentageEnemy1 = CalculatePercentage(frequencyEnemy1, frequencyEnemy3);
            _percentageEnemy2 = CalculatePercentage(frequencyEnemy2, frequencyEnemy3);
            _percentageEnemy3 = CalculatePercentage(frequencyEnemy3, frequencyEnemy3);

            CurrentPercentageEnemy1 = _percentageEnemy1.ToString() + "%";
            CurrentPercentageEnemy2 = (_percentageEnemy2 - _percentageEnemy1).ToString() + "%";
            CurrentPercentageEnemy3 = (_percentageEnemy3 - _percentageEnemy2).ToString() + "%";
        }

        private int CalculatePercentage(float mainFrequency, float otherFrequency) => (int)((mainFrequency / otherFrequency) * 100);

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

        public void CreateAvailableSpawnerList()
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

        public void ChangeInterval(float newInterval) => spawnInterval = newInterval;
        public void ChangeNumberSpawner(int newNumber, bool newIsNumberSpawnerBased, params bool[] newSpawners)
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
    }
}
