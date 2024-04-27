using System.Collections;
using UnityEngine.VFX;

namespace JeuA
{
    public class GodGlowBehavior : Interactable_Event
    {
        public VisualEffect godGlow;

        public override void OnSelected()
        {
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