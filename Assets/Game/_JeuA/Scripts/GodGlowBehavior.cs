using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace JeuA
{
    public class GodGlowBehavior : Interactable_Event
    {
        public VisualEffect godGlow;

        public bool DestroyAfterDelay = true;
        public float Delay = 10f;

        private void OnEnable()
        {
            if (DestroyAfterDelay)
                StartCoroutine(DestroyAfterDelayCo());
        }

        public override void OnSelected()
        {
            godGlow.Stop();

            StopAllCoroutines();

            StartCoroutine(DestroyCo());
        }

        IEnumerator DestroyAfterDelayCo()
        {
            yield return new WaitForSeconds(Delay);

            godGlow.Stop();

            StartCoroutine(DestroyCo());
        }

        private IEnumerator DestroyCo()
        {
            while (godGlow.aliveParticleCount > 0)
            {
                yield return null;
            }

            Destroy(godGlow.gameObject);

            Destroy(this);
        }
    }

}