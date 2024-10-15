using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Scripts
{
    public class PlayerMovement : MonoBehaviour,  IPausedMovement
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private Rigidbody2D playerRb;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private PlayerAnimator playerAnimator;

        [SerializeField] private LayerMask groundLayer;

        private float _horizontalMove;
        public bool IsFacingRight { get; private set; } = true;
        private bool _canMove = true;

        private void Start()
        {
            MovementEventManager.Instance.OnMovementStateChanged += SetMovementState;
        }

        private void OnDisable()
        {

            if (MovementEventManager.Instance != null)
            {
                MovementEventManager.Instance.OnMovementStateChanged -= SetMovementState;
            }
        }

        private void FixedUpdate()
        {
            if (_canMove)
            {
                Flip();
                playerAnimator.MovementAnim(_horizontalMove);
                playerRb.velocity = new Vector2(_horizontalMove * playerData.speed, playerRb.velocity.y);
            }
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed && IsGrounded())
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, playerData.jumpingPower);
                playerAnimator.JumpingAnim(IsGrounded());
            }

            if (context.canceled && playerRb.velocity.y > 0f) // Continuous Jump
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            _horizontalMove = context.ReadValue<Vector2>().x;
        }

        private void Flip()
        {
            if (IsFacingRight && _horizontalMove < 0f || !IsFacingRight && _horizontalMove > 0f)
            {
                IsFacingRight = !IsFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                playerAnimator.JumpingAnim(false);
            }
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        public void SetMovementState(bool canMove)
        {
            _canMove = canMove;
        }
    }
}