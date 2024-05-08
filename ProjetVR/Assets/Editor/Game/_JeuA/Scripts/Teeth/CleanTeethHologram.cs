using System.Collections;
using UnityEngine;

namespace JeuA
{
    public class CleanTeethHologram : MonoBehaviour
    {
        [SerializeField][Range(0f, 1f)] float lerpSpeed;

        [SerializeField] Material liquideMaterial;
        [SerializeField] Material snapMaterial;
        [SerializeField] MeshRenderer mesh;
        [SerializeField] Wobble wobble;

        public bool isEnable = false;

        private float currentValue;

        public void SetToothManager(ToothManager newToothManager)
        {
            newToothManager.OnCleanAmountChange.AddListener(OnCleanAmountChanged);
        }

        private IEnumerator LerpCoroutine(float b, float lerp)
        {
            float difference = Mathf.Abs(currentValue - b);

            while (difference > 0.01f)
            {
                currentValue = Mathf.Lerp(currentValue, b, lerp);
                mesh.material.SetFloat("_fill", currentValue);
                difference = Mathf.Abs(currentValue - b);

                yield return null;
            }

            currentValue = b;
            mesh.material.SetFloat("_fill", currentValue);
        }

        private void OnCleanAmountChanged(float amount)
        {
            if (!isEnable)
                return;

            if (amount == 1)
                mesh.material = snapMaterial;
            else
                mesh.material = liquideMaterial;

            wobble.wobbleAmountToAddX = amount * 2f;
            wobble.wobbleAmountToAddZ = amount * 2f;

            StartCoroutine(LerpCoroutine(amount, lerpSpeed));
        }
    }

}