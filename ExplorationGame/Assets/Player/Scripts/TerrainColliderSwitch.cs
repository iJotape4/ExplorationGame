using UnityEngine;

namespace PlayerScripts
{
    public class TerrainColliderSwitch : MonoBehaviour
    {
        [SerializeField] Transform boardParent;
        PlayerMovement player;
        BoxCollider[] bc;

        private void Start()
        {
            player = GetComponentInParent<PlayerMovement>();
            bc = GetComponents<BoxCollider>();
        }

        private void Update()
        {
            if (player.GetPlayerIsGrounded() && boardParent.localRotation == Quaternion.identity)
                SwitchColliders(true);
            else
                SwitchColliders(false);
        }

        void SwitchColliders(bool enable)
        {
            foreach (var b in bc) b.enabled = enable;
        }
    }
}
