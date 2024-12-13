using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WarriorAnimsFREE
{
    public class AIController : SuperStateMachine
    {
        [Header("Components")]
        private WarriorController warriorController;
        private Transform opponentTransform;
        public float Health, MaxHealth;

        [Header("Movement")]
        public bool canMove = true;
        public float movementAcceleration = 90.0f;
        public float runSpeed = 6f;
        private readonly float rotationSpeed = 40f;
        public float groundFriction = 50f;
        [HideInInspector] public Vector3 currentVelocity;

        [Header("Jumping")]
        public float gravity = 25.0f;
        public float jumpAcceleration = 5.0f;
        public float jumpHeight = 3.0f;
        public float inAirSpeed = 6f;

        [Header("Attacking")]
        public float attackRange = 3.5f; // Range within which AI attacks
        public float attackCooldown = 2f; // Cooldown between attacks
        private bool canAttack = true;

        [Header("Decision")]
        private float directionChangeCooldown = 1.0f; // Time in seconds before changing direction again
        private float lastDirectionChangeTime = 0f;   // Tracks the last time direction was changed
        private bool isMovingAway = false;            // Keeps track of whether the AI is moving away or towards the player

        [HideInInspector] public Vector3 lookDirection { get; private set; }

        private void Start()
        {
            warriorController = GetComponent<WarriorController>();
            // Assign opponent's transform (this can be set dynamically based on game logic)
            opponentTransform = FindOpponent();

            // Set initial state
            currentState = WarriorState.Idle;
        }

        private Transform FindOpponent()
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        }

        #region Updates

        // Update is not used in this case, as state updates are handled in state functions.
        protected override void EarlyGlobalSuperUpdate()
        {
        }

        protected override void LateGlobalSuperUpdate()
        {
            transform.position += currentVelocity * warriorController.superCharacterController.deltaTime;

            if ((warriorController.canMove) & canMove == true)
            {
                if (currentVelocity.magnitude > 0)
                {
                    warriorController.isMoving = true;
                    warriorController.SetAnimatorBool("Moving", true);
                    warriorController.SetAnimatorFloat("Velocity", currentVelocity.magnitude);
                }
                else
                {
                    warriorController.isMoving = false;
                    warriorController.SetAnimatorBool("Moving", false);
                    warriorController.SetAnimatorFloat("Velocity", 0);
                }
            }

            RotateTowardsMovementDir();
            warriorController.SetAnimatorFloat("Velocity", transform.InverseTransformDirection(currentVelocity).z);
        }

        #endregion

        #region Gravity / Jumping

        public void RotateGravity(Vector3 up)
        {
            lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection;
        }

        private float CalculateJumpSpeed(float jumpHeight, float gravity)
        {
            return Mathf.Sqrt(2 * jumpHeight * gravity);
        }

        #endregion

        #region States

        private void Idle_EnterState()
        {
            warriorController.superCharacterController.EnableSlopeLimit();
            warriorController.superCharacterController.EnableClamping();
            warriorController.LockJump(false);
            warriorController.SetAnimatorInt("Jumping", 0);
            warriorController.SetAnimatorBool("Moving", false);
        }

        private void Idle_SuperUpdate()
        {
            // AI decision-making (move towards opponent)
            if ((Vector3.Distance(transform.position, opponentTransform.position) > 2f) && canMove == true);
            {
                currentState = WarriorState.Move;
            }

            // Jump if certain conditions met
            if (ShouldJump())
            {
                currentState = WarriorState.Jump;
            }

            // Apply friction
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, groundFriction * warriorController.superCharacterController.deltaTime);
        }

        private bool ShouldJump()
        {
            // Placeholder logic for AI jump condition. Can be modified to suit the game logic.
            return Random.value < 0.0000001f; 
        }


        private void Attack_EnterState()
        {
            if (canAttack)
            {
                warriorController.Attack1(); // Trigger the attack animation
                StartCoroutine(AttackCooldown());
                canMove = false;
            }
            currentState = WarriorState.Move; // Resume chasing the player after the attack
            canMove = true;
        }   

        private IEnumerator AttackCooldown()
        {
            canAttack = false;
            canMove = false;
            yield return new WaitForSeconds(attackCooldown); // Wait for cooldown
            canAttack = true;
            canMove = true;
        }

        private void Move_SuperUpdate()
        {
            // AI decision-making for movement and attack
            float distanceToPlayer = Vector3.Distance(transform.position, opponentTransform.position);

            // Check if enough time has passed to change direction
            if (Time.time - lastDirectionChangeTime > directionChangeCooldown)
            {
                // Randomize behavior: either move towards or away from the player
                isMovingAway = Random.value < 0.05f;  // 50% chance to either run towards or away
                lastDirectionChangeTime = Time.time;  // Update the last direction change time
            }

            Vector3 moveDirection;

            // Decide the movement direction based on whether AI is moving towards or away
            if (isMovingAway)
            {
                moveDirection = (transform.position - opponentTransform.position).normalized; // Move away from player
            }
            else
            {
                moveDirection = (opponentTransform.position - transform.position).normalized; // Move towards player
            }

            // If the player is far, move towards or away from them
            if (distanceToPlayer > 3.5f)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, moveDirection * runSpeed, movementAcceleration * warriorController.superCharacterController.deltaTime);
                currentState = WarriorState.Move;
            }
            else if (canAttack)
            {
                // Player is in range, stop moving and initiate attack
                currentVelocity = Vector3.zero;  // Stop moving
                currentState = WarriorState.Attack;  // Transition to attack state
            }

            // Jump logic
            if (ShouldJump())
            {
                currentState = WarriorState.Jump;
            }

            // Fall condition
            if (!warriorController.MaintainingGround())
            {
                currentState = WarriorState.Fall;
            }
        }






        private void Jump_EnterState()
        {
            warriorController.SetAnimatorInt("Jumping", 1);
            warriorController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
            warriorController.superCharacterController.DisableClamping();
            warriorController.superCharacterController.DisableSlopeLimit();
            currentVelocity += warriorController.superCharacterController.up * CalculateJumpSpeed(jumpHeight, gravity);
            warriorController.LockJump(true);
        }

        private void Jump_SuperUpdate()
        {
            Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(warriorController.superCharacterController.up, currentVelocity);
            Vector3 verticalMoveDirection = currentVelocity - planarMoveDirection;

            if (currentVelocity.y < 0)
            {
                currentVelocity = planarMoveDirection;
                currentState = WarriorState.Fall;
                return;
            }

            planarMoveDirection = Vector3.MoveTowards(planarMoveDirection, warriorController.moveInput * inAirSpeed, jumpAcceleration * warriorController.superCharacterController.deltaTime);
            verticalMoveDirection -= warriorController.superCharacterController.up * gravity * warriorController.superCharacterController.deltaTime;
            currentVelocity = planarMoveDirection + verticalMoveDirection;
        }

        private void Fall_SuperUpdate()
        {
            if (warriorController.AcquiringGround())
            {
                currentVelocity = Math3d.ProjectVectorOnPlane(warriorController.superCharacterController.up, currentVelocity);
                currentState = WarriorState.Idle;
                return;
            }

            currentVelocity -= warriorController.superCharacterController.up * gravity * warriorController.superCharacterController.deltaTime;
        }

        #endregion

        private void RotateTowardsMovementDir()
        {
            if (currentVelocity != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentVelocity), Time.deltaTime * rotationSpeed);
            }
        }



    }
}




