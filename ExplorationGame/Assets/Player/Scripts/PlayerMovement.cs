using System.Collections;
using UnityEngine;
namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(0.1f, 50f)] public float playerSpeed = 10f;
        [Range(0.1f, 100f)] public float jumpForce = 10f;
        Vector2 _horizontalInput;
        [SerializeField]  bool _onRail;
        //[HideInInspector] Vector3 mass;
        [SerializeField] Transform groundCheck;
        [SerializeField] float groundCheckArea = 0.3f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool inRamp;

        [SerializeField] public Rigidbody rb;
        [SerializeField] public float speedMultiplier = 10f;
        [SerializeField] public float rbDrag = 8f;
        [SerializeField] Transform cam;

        Animator anim;
        string paramSpeed = "Velocity";
        string paramJumpTrigger = "Jump";

        public float turnSmoothTime = 0.1f;
        float turnSmoothVelocity;
        public void ReceiveInput(Vector2 moveInput)
        {
            _horizontalInput = moveInput;
        }

        public void ReceiveJumpInput()
        {
            if (isGrounded)
            {
                anim.SetTrigger(paramJumpTrigger);
                StartCoroutine(AddJumpForce());
            }
        }

        public IEnumerator AddJumpForce()
        {
            yield return new WaitForSeconds(0.5f);
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        }
        public void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            anim = GetComponentInChildren<Animator>();
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

            if (_onRail)
                return;

            float x = _horizontalInput.x;
            float y = _horizontalInput.y;

            Vector3 movement = new Vector3(x, 0f, y).normalized;

            if (movement.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                rb.AddForce(moveDir.normalized * playerSpeed * speedMultiplier, ForceMode.Impulse);

                //transform.position += moveDir.normalized* playerSpeed*speedMultiplier* Time.deltaTime;
            }

            anim.SetFloat(paramSpeed, movement.magnitude);
        }

        public void MoveAlongRail(Vector3 destination,Vector3 contactPoint)
        {
            _onRail = true;
           // transform.position = contactPoint + Vector3.up*1.5f  ;
            StartCoroutine(MoveOnRail(destination));
        }

        public IEnumerator MoveOnRail(Vector3 destination)
        {
            while (Vector3.Distance(transform.position, destination) > 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, 0.1f);
                yield return new WaitForSeconds(0.001f);
            }
            _onRail = false;
        }
    }
}

