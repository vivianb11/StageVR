using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JeuB
{
    public class GreenMob : Mob
    {
        public int[] degree;

        protected override void EntityUpdate()
        {
            base.EntityUpdate();

            RotateMesh();
        }

        protected override void EntityStart()
        {
            base.EntityStart();

            AddRotator();

            GetComponent<BoxCollider>().isTrigger = true;
            StartCoroutine(DelayRotation());
        }

        private void RotateMesh()
        {
            transform.GetChild(0).transform.Rotate(0, 0, 50 * Time.deltaTime);
        }

        private void AddRotator()
        {
            GameObject newParent = Instantiate(new GameObject(), target.transform);
            transform.parent = newParent.transform;
        }

        private IEnumerator Rotation()
        {
            _isKnocked = true;
            float elapsedTime = 0f;
            float rotationDuration = 2f;
            int RandomRotationIndex = Random.Range(0, degree.Length);
            Quaternion startRotation = transform.parent.localRotation;
            Quaternion targetRotation = Quaternion.Euler(0f, degree[RandomRotationIndex], 0f);

            while (elapsedTime < rotationDuration)
            {
                float t = elapsedTime / rotationDuration;
                transform.parent.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            //transform.parent.localRotation = targetRotation;
            _isKnocked = false;
        }

        private IEnumerator DelayRotation()
        {
            float timeBeforeRotation = Random.Range(2.5f, 3f);
            yield return new WaitForSeconds(timeBeforeRotation);
            yield return Rotation();
        }
    }
}
