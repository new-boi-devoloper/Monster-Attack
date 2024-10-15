using Player_Scripts;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.FeedbackUI
{
    public class CooldownUI : MonoBehaviour
    {
        [SerializeField] private PlayerCreature playerCreature;
        public Image cooldownFillImage;
        public Sprite[] fillSprites;

        private int currentSpriteIndex = 0;

        private void Start()
        {
            if (playerCreature.cooldownTimer == null)
            {
                Debug.LogError("CooldownTimer is not found on PlayerCreature.");
                return;
            }

            playerCreature.cooldownTimer.CooldownTime
                .Subscribe(timeLeft =>
                {
                    if (timeLeft > 0)
                    {
                        var fillAmount = timeLeft / playerCreature.playerData.attack2Cooldown;
                        cooldownFillImage.fillAmount = fillAmount;

                        // Calculate the sprite index based on the fill amount
                        var spriteIndex = Mathf.FloorToInt(fillAmount * (fillSprites.Length - 1));
                        cooldownFillImage.sprite = fillSprites[spriteIndex];
                    }
                    else
                    {
                        cooldownFillImage.fillAmount = 1f;
                        cooldownFillImage.sprite = fillSprites[fillSprites.Length - 1];
                    }
                })
                .AddTo(this);
        }
    }
}