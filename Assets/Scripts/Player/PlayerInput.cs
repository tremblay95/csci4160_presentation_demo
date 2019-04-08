using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour
{
    public static Transform pos;

    public GunControl gun;
    private CharacterController cc;
    public Transform camTrans;

    public float useDistance = 2.0f;
    int intEnvMask;

    public bool isMovingBox = false;
    public Rigidbody moveBox = null;
    Vector3 boxMoveDir = Vector3.zero;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float boxMoveSpeed = 2.0f;
    [SerializeField] private float boxMoveForce = 20.0f;

    Vector3 moveDir;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        intEnvMask = LayerMask.GetMask("InteractiveEnvironment");

        if (pos == null) pos = transform;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        

        gun.gunDown = isMovingBox;

        moveDir = transform.forward * vertical + transform.right * horizontal;


        cc.SimpleMove(moveDir * (isMovingBox ? boxMoveSpeed : moveSpeed));

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            StartCoroutine(Jump());
        }


    }

    private IEnumerator ResetBox()
    {
        
        yield return new WaitForSeconds(0.1f);
        isMovingBox = false;
    }

    [SerializeField] private AnimationCurve jumpCurve; // used to control upward force
    [SerializeField] private float jumpFactor = 5.0f;

    private bool isJumping;


    private IEnumerator Jump()
    {
        // avoid crawling up slopes during the jump
        cc.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        // if we are grounded and our head is not colliding with the ceiling, move upward
        do
        {
            // at some point, the jump amount will be less than the effects of gravity
            float jumpAmount = jumpCurve.Evaluate(timeInAir) * jumpFactor * Time.deltaTime;
            cc.Move(Vector3.up * jumpAmount);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!cc.isGrounded && cc.collisionFlags != CollisionFlags.Above);

        // reset slope limit so we can go up slopes when not jumping
        cc.slopeLimit = 45.0f;
        isJumping = false;
    }

}
