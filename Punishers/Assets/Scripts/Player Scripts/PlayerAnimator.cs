using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private Animator _animator;
    private Animation _animationComponent;


    //Float type
    private static readonly int Speed = Animator.StringToHash("Speed");

    //bool type
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    //trigger type
    private static readonly int TakeHit = Animator.StringToHash("TakeHit");
    private static readonly int Attack1 = Animator.StringToHash("Attack1");
    private static readonly int Attack2 = Animator.StringToHash("Attack2");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void MovementAnim(float speed)
    {
        _animator.SetFloat(Speed, Mathf.Abs(speed));
    }
    

    public void Attack1Anim()
    {
        _animator.SetTrigger(Attack1);
    }

    public void Attack2Anim()
    {
        _animator.SetTrigger(Attack2);
    }

    public void DeathAnim(bool isDead)
    {
        _animator.SetBool(IsDead, isDead);
    }

    public void TakeHitAnim()
    {
        _animator.SetTrigger(TakeHit);
    }

    public void JumpingAnim(bool isJumping)
    {
        _animator.SetBool(IsJumping, isJumping);
    }
}