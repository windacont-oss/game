using UnityEngine;

namespace ImbaLife.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class BasicFirstPersonMotor : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4.5f;
        [SerializeField] private float mouseSensitivity = 2.4f;
        [SerializeField] private float gravity = -18f;

        private CharacterController controller;
        private float velocityY;
        private float lookX;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);

            Camera cam = Camera.main;
            if (cam != null)
            {
                lookX = Mathf.Clamp(lookX - mouseY, -80f, 80f);
                cam.transform.localRotation = Quaternion.Euler(lookX, 0f, 0f);
            }

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 move = (transform.right * horizontal + transform.forward * vertical) * moveSpeed;

            if (controller.isGrounded && velocityY < 0f)
            {
                velocityY = -1f;
            }

            velocityY += gravity * Time.deltaTime;
            move.y = velocityY;

            controller.Move(move * Time.deltaTime);
        }
    }
}
