using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


public class Player : MonoBehaviour
{
    PlayerInput input = null;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction runAction;
    CharacterController cc;
    Animator anim;
    bool bRun;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();

        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        runAction = input.actions["Run"];

        bRun = false;
        
        runAction.performed += (context) =>
        {
            bRun = true;
        };
        runAction.canceled += (context) =>
        {
            bRun = false;
        };

        jumpAction.performed += (context) =>
        {
            // 만약 jumpCount가 maxJumpCount보다 작다 그리고 스페이스바를 눌렀다면
            if (jumpCount < maxJumpCount)
            {
                // 점프를 뛰고싶다.
                yVelocity = jumpPower;
                jumpCount++;
            }
        };



        cc = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        
    }

    public float gravity = -9.81f;
    public float jumpPower = 20f;
    float yVelocity;
    int jumpCount = 0;
    public int maxJumpCount = 1;

    float walkSpeed = 3f;
    float runSpeed = 6f;
    float speed;

    private void FixedUpdate()
    {
        // 만약 땅에 닿았다면 jumpCount를 0으로 초기화 하고싶다.
        if (cc.isGrounded)
        {
            jumpCount = 0;
        }
    }

    void Update()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        var move = moveAction.ReadValue<Vector2>();
        float h = move.x;
        float v = move.y;

        if (bRun)
        {
            // 뛸때는 h, v의 크기가 2
            anim.SetFloat("h", h * 2);
            anim.SetFloat("v", v * 2);
        }
        else
        {
            // 걸을때는 h, v의 크기가 1
            anim.SetFloat("h", h);
            anim.SetFloat("v", v);
        }

        yVelocity += gravity * Time.deltaTime;


        Vector3 dir = new Vector3(h, 0, v);
        // 카메라를 기준으로 방향을 재설정 하고싶다.
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();

        speed = walkSpeed;
        if (bRun)
        {
            speed = runSpeed;
        }

        Vector3 velocity = dir * speed;
        velocity.y = yVelocity;

        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
}
