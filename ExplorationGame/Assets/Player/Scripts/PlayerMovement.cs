using MyBox;
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
        [SerializeField, Range(1f, 20f)] float tricksSpeed =10f;

        [Header("GroundChecking")]
        [SerializeField] private LayerMask groundMask;
        [SerializeField] float groundCheckArea = 0.3f;
        [SerializeField] Transform groundCheck;
        [HideInInspector] public float m_rayDistance = 5f;

        [Header("Assignable components")]
        [SerializeField] Transform cam;
        public Transform m_skateboard;
        [HideInInspector] public Rigidbody rb;
      
        [SerializeField, ReadOnly] private bool isGrounded, inRamp, _onRail;
             
        [Header("Animation Params")]
        Animator anim;
        readonly string paramSpeed = "Velocity";
        readonly string paramJumpStartBool = "JumpStart";
        readonly string paramJumpTrigger = "Jump";
        readonly string paramRailBool = "OnRail";
        readonly string preJumpAnimName = "Pre jump";

        
        public void ReceiveInput(Vector2 moveInput)
        {
            _horizontalInput = moveInput;
        }

        public void ReceiveJumpInput()
        {
            if (isGrounded)
            {
                anim.SetBool(paramJumpStartBool, true);          
            }
        }

        public void ReleaseJumpInput()
        {
            if (anim.GetBool(paramJumpStartBool))
            {
                anim.SetBool(paramJumpTrigger, true);
                anim.SetBool(paramJumpStartBool, false);
                StartCoroutine(AddJumpForce());
            }
        }

        public IEnumerator AddJumpForce()
        {
            //yield return new WaitForSeconds(0.5f);
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);

            yield return null;
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
            if(_onRail)
                return;

            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckArea, groundMask);
            if (!isGrounded)
            {
                anim.SetBool(paramJumpStartBool, false);
                DoTricks();
                return;
            }   
            
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

        public void MoveAlongRail(Vector3 destination)
        {
            _onRail = true;
            anim.SetBool(paramRailBool, true);
            rb.isKinematic = true;
            StartCoroutine(MoveOnRail(destination));
        }

        public IEnumerator MoveOnRail(Vector3 destination)
        {
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

        void DoTricks()
        {
            float x = _horizontalInput.x;
            float y = _horizontalInput.y;

            m_skateboard.Rotate(m_skateboard.transform.up, x*tricksSpeed);
           //KickFlipRotation
            //m_skateboard.Rotate(0, 0,  y* tricksSpeed);
            
        }
    }   
}

