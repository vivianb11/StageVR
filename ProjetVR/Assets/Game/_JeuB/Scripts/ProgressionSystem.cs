using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace JeuB
{
    [RequireComponent(typeof(RandomSpawn))]
    public class ProgressionSystem : MonoBehaviour
    {
        private RandomSpawn spawner;

        public GameObject Background;
        public List<string> levelColors;


        [Header("Progression Parameters")]
        [SerializeField] int interval = 10;
        [NaughtyAttributes.ReadOnly][SerializeField] int milestoneCount = 0;
        [NaughtyAttributes.ReadOnly][SerializeField] DifficultyPresets currentPreset;


        [Space(10)]
        [Header("Level Parameters")]
        [SerializeField] PulsatingText pulsatingTextBehavior;
        [NaughtyAttributes.ReadOnly][SerializeField] int currentLevel = 0;
        [SerializeField] int maxLevel;


        [Space(10)]
        [Header("Datas")]
        [SerializeField] DifficultyPresets deathPreset;
        [SerializeField] DifficultyPresets[] tutorialDifficulties;
        [SerializeField] DifficultyPresets[] difficultyPresets;
        private int tutorialDifficultiesCount = 0;
        private int difficultyPresetsCount = 0;


        [Space(10)]
        [Header("Debug Parameters")]
        [SerializeField] bool enableProgression = true;
        private static bool _skipTutorial = false;
        [Button("Skip a milestone")]
        private void Skip()
        {
            CountMinutes();
            CancelInvoke();
        }

        private float _holdTimeKill;
        private float _maxHoldTimeKill = 1f;
        private bool _order66Executed;

        private void OnEnable()
        {
            spawner = GetComponent<RandomSpawn>();

            if (enableProgression) Invoke(nameof(CountMinutes), interval);

            milestoneCount = currentLevel = 0;
        }

        private void OnDisable()
        {
            CancelInvoke();
            StopAllCoroutines();
        }

        private void Update()
        {
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || Input.GetKey(KeyCode.K))
            {
                if (!_order66Executed)
                {
                    _holdTimeKill += Time.deltaTime;

                    if (_holdTimeKill >= _maxHoldTimeKill)
                    {
                        _holdTimeKill = 0;
                        Order66();
                    }
                }
            }
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


        private void ColorChanger()
        {
            int colorIndex;
            colorIndex = currentLevel - 1;
            string hexColor = levelColors[colorIndex];
            Color newColor;

            if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out newColor))
            {
                Background.GetComponent<SpriteRenderer>().color = newColor;
            }

        }

        private void LevelPassed()
        {
            pulsatingTextBehavior.disappear = true;

            if (currentLevel < maxLevel)
            {
                currentLevel += 1;
                pulsatingTextBehavior.textMesh.text = "Niveau: " + currentLevel.ToString();
                ColorChanger();
            }
            else pulsatingTextBehavior.textMesh.text = "Niveau: Bonus";
        }

        public void ChangeMilestoneInterval(int newInterval) => interval = newInterval;

        public void ChangeDifficulty(DifficultyPresets preset)
        {
            if (preset.levelPassed && !_order66Executed) LevelPassed();
            currentPreset = preset;
            milestoneCount += 1;

            ChangeMilestoneInterval(preset.milestoneInterval);
            spawner.ChangeInterval(preset.spawnInterval);
            spawner.ChangeMobSpeed(preset.mobSpeed);
            spawner.ChangeNumberSpawner(preset.numberSpawner, preset.isNumberSpawnerBased, preset.spawnerTop, preset.spawnerTopRight, preset.spawnerBottomRight, preset.spawnerBottom, preset.spawnerBottomLeft, preset.spawnerTopLeft);
            spawner.ChangeWeightEnemy(ref spawner.weightEnemy1, preset.weightEnemy1);
            spawner.ChangeWeightEnemy(ref spawner.weightEnemy2, preset.weightEnemy2);
            spawner.ChangeWeightEnemy(ref spawner.weightEnemy3, preset.weightEnemy3);
            spawner.ChangeMobTypePerSpawner(preset.mobTypePerSpawner);
            spawner.ChangeWeightBased(preset.weightBased);
            spawner.CreateAvailableSpawnerList();
            spawner.SpawnPercentage();
        }

        public void Order66()
        {
            _order66Executed = true;
            Debug.Log("Order 66 executed");

            CancelInvoke(nameof(CountMinutes));
            ChangeDifficulty(deathPreset);
        }
    }
}
