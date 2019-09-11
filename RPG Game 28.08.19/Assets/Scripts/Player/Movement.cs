using UnityEngine;

namespace RPG.Player
{
    [AddComponentMenu("RPG/Player/Movement")]
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        [Header("Speed Vars")]
        //Value Variables
        public float moveSpeed;
        public float runSpeed, walkSpeed, crouchSpeed, jumpSpeed;
        private float _gravity = 20;
        //Struct - Contains Multiple Variables (eg...3 floats)
        private Vector3 _moveDir;
        //Reference Variable
        private CharacterController _charController;

        private void Start()
        {
            _charController = GetComponent<CharacterController>();
        }
        private void Update()
        {
            Move();
        }
        private void Move()
        {
            if (_charController.isGrounded)
            {
                //set speed
                if (Input.GetButton("Sprint"))
                {
                    moveSpeed = runSpeed;
                }
                else if (Input.GetButton("Crouch"))
                {
                    moveSpeed = crouchSpeed;
                    runSpeed = crouchSpeed;
                }
                else
                {
                    moveSpeed = walkSpeed;
                }
                
                //move this direction based off inputs
                _moveDir = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed);
                if (Input.GetButton("Jump"))
                {
                    _moveDir.y = jumpSpeed;
                }
            }
            //Regardless if we are grounded or not
            //apply grvity
            _moveDir.y -= _gravity * Time.deltaTime;
            //apply mo
            _charController.Move(_moveDir * Time.deltaTime);
        }
    }
}
