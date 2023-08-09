using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(PlayerMovement))]
    public class InputManager : MonoBehaviour
    {
        PlayerMovement movement;

        PlayerInput controls;
        PlayerInput.OnFootActions onFootActions;

        private void Awake()
        {
            //GET COMPONENTS
            movement = GetComponent<PlayerMovement>();

            //GET ACTION MAPS
            controls = new PlayerInput();
            onFootActions = controls.OnFoot;

            //SUSCRIBE INPUT ACTIONS
            onFootActions.Move.performed += ctx => movement.ReceiveInput(ctx.ReadValue<Vector2>());
            onFootActions.Jump.canceled += ctx => movement.ReceiveJumpInput();
        }

        private void OnEnable()=> controls.Enable();

        private void OnDestroy() => controls.Disable();  
    }
}
