using UnityEngine;

namespace Enemy_Scripts
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;

        private Animator _animator;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsDead = Animator.StringToHash("IsDead");
        private static readonly int TakeHit = Animator.StringToHash("TakeHit");
        //test for git 2

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            WalkIdleAnim();
        }

        private void WalkIdleAnim()
        {
            _animator.SetFloat(Speed, Mathf.Abs(enemyData.speed));
        }

        public void AttackAnim()
        {
            _animator.SetTrigger(Attack);
        }

        public void DeathAnim(GameObject deadEnemy)
        {
            if (deadEnemy == gameObject)
            {
                _animator.SetBool(IsDead, true);
            }
        }

        public void TakeHitAnim()
        {
            _animator.SetTrigger(TakeHit);
        }
    }
}