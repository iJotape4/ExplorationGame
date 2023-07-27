using System.Collections;
using UnityEngine;
namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Player variables")]
        [Range(0.1f, 50f)] public float playerSpeed = 10f;
        [Range(0.1f, 100f)] public float jumpForce = 10f;
        [Range(0.1f, 1f)] public float turnSmoothTime = 0.5f;
        [SerializeField, Range(1f, 20f)] public float speedMultiplier = 10f;
        [HideInInspector] public float rbDrag = 0f;
        float turnSmoothVelocity;
        Vector2 _horizontalInput;

        [Header("GroundChecking")]
        [SerializeField] private LayerMask groundMask;
        [SerializeField] float groundCheckArea = 0.3f;
        [SerializeField] Transform groundCheck;
        [HideInInspector] public float m_rayDistance = 5f;

        [Header("Assignable components")]
        [SerializeField] Transform cam;
        public Transform m_skateboard;
        [HideInInspector] public Rigidbody rb;

        [Header("Booleans")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool inRamp;
        [SerializeField] bool _onRail;
             
        [Header("Animation Params")]
        Animator anim;
        readonly string paramSpeed = "Velocity";
        readonly string paramJumpTrigger = "Jump";
        readonly string paramRailBool = "OnRail";
        
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

        private void FixedUpdate()
        {
            manageMovement();
            AlignToSurface();      
        }

        void ControlDrag() =>
            rb.drag = rbDrag;

        void manageMovement()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckArea, groundMask);
            if (_onRail || !isGrounded)
                return;

            float x = _horizontalInput.x;
            float y = _horizontalInput.y;

            Vector3 movement = new Vector3(x, 0f, y).normalized;

            if (movement.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                rb.AddForce(moveDir * playerSpeed * speedMultiplier);
                
                if(y>-1) //Allows stop the skateboard without rotate 
                transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);
                    
                
                Debug.DrawRay(transform.position, movement, Color.green);
                Debug.DrawRay(transform.position, moveDir, Color.yellow);
            }

            anim.SetFloat(paramSpeed, movement.magnitude);
        }

        public void MoveAlongRail(Vector3 destination, Vector3 contactPoint, Vector3 skateRotation)
        {
            _onRail = true;
            anim.SetBool(paramRailBool, true);
            rb.isKinematic = true;
            StartCoroutine(MoveOnRail(destination, skateRotation));
        }

        public IEnumerator MoveOnRail(Vector3 destination, Vector3 skateRotation)
        {

            m_skateboard.forward =  -skateRotation;

            while (Vector3.Distance(transform.position, destination) > 1f && _onRail)
            {
                m_skateboard.localPosition = Vector3.zero;
               transform.position = Vector3.MoveTowards(transform.position, destination, 0.1f);
               
                yield return new WaitForSeconds(0.01f);
            }
            _onRail = false;
            rb.isKinematic = false;
            anim.SetBool(paramRailBool, false);
        }

        void AlignToSurface()
        {
            RaycastHit hit;
            var onSurface = Physics.Raycast(transform.position, Vector3.down, out hit, m_rayDistance);
            if (onSurface && isGrounded)
            {
                var localRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

                var euler = localRot.eulerAngles;
                euler.y = 0;
                localRot.eulerAngles = euler;
                m_skateboard.localRotation = Quaternion.LerpUnclamped(m_skateboard.localRotation, localRot, 0.1f);
            }
        }

        public bool  GetPlayerIsGrounded() => isGrounded;
    }   
}

