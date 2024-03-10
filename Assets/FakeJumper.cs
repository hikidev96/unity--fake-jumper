using UnityEngine;
using UnityEngine.Events;

namespace FJ
{
    public class FakeJumper : MonoBehaviour
    {
        [SerializeField] private Transform bodyTrans;
        [SerializeField] private Transform shadowTrans;
        [SerializeField] private bool doJumpOnStart;
        [SerializeField] private float gravityValue = 9.8f;
        [SerializeField] private float jumpPower = 10.0f;
        [SerializeField] private float groundOffset = 0.0f;
        [SerializeField] private UnityEvent grounded = new UnityEvent();

        private float currentJumpPower;
        private bool isGrounded = false;

        public UnityEvent Grounded => grounded;
        public bool IsGrounded => isGrounded;
        public float GravityValue => gravityValue;

        private void Start()
        {
            if (this.doJumpOnStart == true)
            {
                Jump(jumpPower, groundOffset);
            }
        }

        private void Update()
        {
            if (isGrounded == true)
                return;

            bodyTrans.transform.position += new Vector3(0.0f, currentJumpPower * Time.deltaTime, 0.0f);

            currentJumpPower -= gravityValue * Time.deltaTime;

            if (bodyTrans.position.y <= shadowTrans.position.y + groundOffset &&
                currentJumpPower <= 0.0f)
            {
                isGrounded = true;
                bodyTrans.position = new Vector2(bodyTrans.position.x, shadowTrans.position.y + groundOffset);
                grounded?.Invoke();
            }
        }

        public void Jump(Vector2 targetPos, float speed, float groundOffset = 0.0f)
        {
            var distance = Vector2.Distance(targetPos, this.transform.position);

            this.currentJumpPower = (distance / speed) / 2.0f * GravityValue;
            this.groundOffset = groundOffset;
            isGrounded = false;
        }        

        public void Jump(float jumpPower, float groundOffset = 0.0f)
        {
            this.currentJumpPower = jumpPower;
            this.groundOffset = groundOffset;
            isGrounded = false;
        }
    }
}
