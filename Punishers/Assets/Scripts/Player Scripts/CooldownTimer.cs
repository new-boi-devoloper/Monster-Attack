using UniRx;
using UnityEngine;

namespace Player_Scripts
{
    public class CooldownTimer : MonoBehaviour
    {
        private ReactiveProperty<float> _cooldownTime = new(0f);
        public IReadOnlyReactiveProperty<float> CooldownTime => _cooldownTime;

        public void StartCooldown(float duration)
        {
            _cooldownTime.Value = duration;
            Observable.EveryUpdate()
                .TakeWhile(_ => _cooldownTime.Value > 0)
                .Subscribe(_ => _cooldownTime.Value -= Time.deltaTime)
                .AddTo(this);
        }
    }
}