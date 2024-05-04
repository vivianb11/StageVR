using UnityEngine;

namespace JeuA
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(SoundEmiter))]
    public class MascotteSpriteHandler : MonoBehaviour
    {
        [SerializeField] SO_MascotteEmotion cleaning;
        [SerializeField] SO_MascotteEmotion confuse;
        [SerializeField] SO_MascotteEmotion happy;
        [SerializeField] SO_MascotteEmotion neutral;
        [SerializeField] SO_MascotteEmotion neutral2;
        [SerializeField] SO_MascotteEmotion thinking;

        private SpriteRenderer spriteRenderer;
        private SoundEmiter soundEmiter;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            soundEmiter = GetComponent<SoundEmiter>();
        }

        public void SwitchSprite(Mascotte.MascotteState state)
        {
            switch (state)
            {
                case Mascotte.MascotteState.IDLE:
                    spriteRenderer.sprite = neutral.sprites.PickRandom();
                    PlaySound(neutral.sounds.PickRandom());
                    break;
                case Mascotte.MascotteState.HELP_TARTAR:
                    spriteRenderer.sprite = thinking.sprites.PickRandom();
                    PlaySound(thinking.sounds.PickRandom());
                    break;
                case Mascotte.MascotteState.HELP_DECAY:
                    spriteRenderer.sprite = thinking.sprites.PickRandom();
                    PlaySound(thinking.sounds.PickRandom());
                    break;
                case Mascotte.MascotteState.HELP_DIRTY:
                    spriteRenderer.sprite = thinking.sprites.PickRandom();
                    PlaySound(thinking.sounds.PickRandom());
                    break;
                case Mascotte.MascotteState.HELP_SMELL:
                    spriteRenderer.sprite = thinking.sprites.PickRandom();
                    PlaySound(thinking.sounds.PickRandom());
                    break;
                case Mascotte.MascotteState.HAPPY:
                    spriteRenderer.sprite = happy.sprites.PickRandom();
                    PlaySound(happy.sounds.PickRandom());
                    break;
            }
        }

        private void PlaySound(Sound sound)
        {
            if (sound == null)
                return;

            soundEmiter.sound = sound;
            soundEmiter.PlaySound();
        }
    }

}