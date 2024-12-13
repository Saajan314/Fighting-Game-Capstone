using UnityEngine;

namespace WarriorAnimsFREE
{
    public class GUIControls : MonoBehaviour
    {
        private WarriorController warriorController;

        public bool isAttacking = false;   

        private void Awake()
        {
            warriorController = GetComponent<WarriorController>();
        }

        private void Update()
        {
   
            if (!isAttacking && warriorController.canAction)
            {
                Attacking();
                Jumping();
                SwirlAttack();
            }
        }

        private void Attacking()
        {
            if (warriorController.MaintainingGround() && warriorController.canAction)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    warriorController.Attack1();
                    isAttacking = true;  
                    StartCoroutine(WaitForAttackAnimation());  
                }
            }
        }

        private void Jumping()
        {
            if (warriorController.canJump && warriorController.canAction && !isAttacking)
            {
                if (warriorController.MaintainingGround())
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        warriorController.inputJump = true;
                    }
                }
            }
        }

        private void SwirlAttack()
        {
            if (warriorController.MaintainingGround() && warriorController.canAction && !isAttacking)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    StartCoroutine(PerformSwirlAttack());
                }
            }
        }

        private System.Collections.IEnumerator PerformSwirlAttack()
        {
            // ANIM
            warriorController.Attack1();
            isAttacking = true;

            // SPIN ANIM
            float spinDuration = 0.5f;
            float spinSpeed = 1440f;
            float elapsedTime = 0f;

            while (elapsedTime < spinDuration)
            {
                float rotationAmount = spinSpeed * Time.deltaTime;
                transform.Rotate(0f, rotationAmount, 0f);   
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = Quaternion.Euler(0f, Mathf.Round(transform.rotation.eulerAngles.y), 0f);
            isAttacking = false;  
        }

        private System.Collections.IEnumerator WaitForAttackAnimation()
        {
 
            float attackDuration = 1.5f;  
            yield return new WaitForSeconds(attackDuration);

            isAttacking = false;   
        }
    }
}

