using UnityEngine;
namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(0.1f, 50f)] public float playerSpeed = 10f;
        [Range(0.1f, 50f)] public float jumpForce = 10f;
        Vector2 _horizontalInput;
        bool _jump;
        //[HideInInspector] Vector3 mass;
        [SerializeField] Transform groundCheck;
        [SerializeField, Range(0.1f, 1f)] float groundCheckArea = 0.3f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private bool isGrounded;

        [SerializeField] public Rigidbody rb;
        [SerializeField] public float speedMultiplier = 10f;
        [SerializeField] public float rbDrag = 8f;
        Transform cam;

        public float turnSmoothTime = 0.1f;
        float turnSmoothVelocity;
        public void ReceiveInput(Vector2 moveInput)
        {
            _horizontalInput = moveInput;
        }
        // Start is called before the first frame update
        public void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            cam = Camera.main.transform;
        }

        void Update() =>
            ControlDrag();

        private void FixedUpdate() =>
            manageMovement();

        void ControlDrag() =>
            rb.drag = rbDrag;

        void manageMovement()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckArea, groundMask);

            float x = _horizontalInput.x;
            float y = _horizontalInput.y;

            Vector3 movement = new Vector3(x, 0f, y).normalized;

            if (movement.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                transform.position += movement * playerSpeed*speedMultiplier* Time.deltaTime;
            }
        }
    }
}

