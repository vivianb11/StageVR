using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JeuA
{
    public class TeethProgress : MonoBehaviour
    {
        public int toothCount = 1;
        public float animationDelay = 0.5f;

        public Transform toothPrefab;

        [SerializeField] Material cleanMaterial;
        [SerializeField] Material fillMaterial;

        [SerializeField] ToothManager toothManager;

        private int cleanedTeeth;

        private List<CleanTeethHologram> teeth = new List<CleanTeethHologram>();

        void Awake()
        {
            toothManager.OnTeethCleaned.AddListener(SetFullTooth);
        }

        public void SetFullTooth()
        {
            if (cleanedTeeth == teeth.Count)
                return;

            teeth[cleanedTeeth].GetComponent<MeshRenderer>().material = cleanMaterial;
            teeth[cleanedTeeth].GetComponent<Tween>().PlayTween("bump");
            teeth[cleanedTeeth].isEnable = false;

            cleanedTeeth++;

            if (cleanedTeeth < transform.childCount)
            {
                teeth[cleanedTeeth].GetComponent<MeshRenderer>().material = fillMaterial;
                teeth[cleanedTeeth].isEnable = true;
            }
        }

        [Button]
        public void SpawnTeeth()
        {
            cleanedTeeth = 0;

            StopAllCoroutines();

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            List<Tween> tweens = new List<Tween>();

            for (int i = 0; i < toothCount; i++)
            {
                Transform newTooth = Instantiate(toothPrefab, transform);
                teeth.Add(newTooth.GetComponent<CleanTeethHologram>());
                teeth[teeth.Count - 1].SetToothManager(toothManager);

                newTooth.localScale = Vector3.zero;

                tweens.Add(newTooth.GetComponent<Tween>());
            }

            transform.GetChild(0).GetComponent<MeshRenderer>().material = fillMaterial;
            teeth[0].isEnable = true;

            StartCoroutine(SpawnAnimation(tweens.ToArray(), animationDelay));
        }

        private IEnumerator SpawnAnimation(Tween[] tweener, float delay)
        {
            foreach (Tween tween in tweener)
            {
                tween.PlayTween("spawn");

                yield return new WaitForSeconds(delay);
            }
        }
    }

}