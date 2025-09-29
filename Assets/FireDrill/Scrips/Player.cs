using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public partial class Player : MonoBehaviour
{
    PlayerInput input = null;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction runAction;
    InputAction eventAction;
    InputAction putAction;
    InputAction actionAction;
    InputAction throwAction;
    CharacterController cc;
    Animator anim;
    bool bRun;
    public GameObject ballFactory;
    public Transform ballPoint;

    float curHP;
    public float maxHP = 10f;
    public Slider sliderHP;
    public Image imageDamage;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("FireObject"))
        {
            curHP -= 1;
            sliderHP.value = curHP;

            // 만약 curHP가 0이면 
            if (curHP <= 0)
            {
                ResultManager.Instance.ShowSuccessUI();
            }

            if (false == bDamageEffect)
            {
                bDamageEffect = true;
                StartCoroutine(IEDamage());
            }
        }
    }
    bool bDamageEffect;

    IEnumerator IEDamage()
    {
        Color c = imageDamage.color;
        for (float t = 0.75f; t > 0; t -= Time.deltaTime * 2)
        {
            c.a = t;
            imageDamage.color = c;
            yield return new WaitForSeconds(Time.deltaTime * 2);
        }
        c.a = 0;
        imageDamage.color = c;
        bDamageEffect = false;
    }

    private void Awake()
    {
        sliderHP.minValue = 0;
        sliderHP.maxValue = maxHP;
        sliderHP.value = maxHP;
        curHP = maxHP;



        anim = GetComponentInChildren<Animator>();

        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        runAction = input.actions["Run"];
        eventAction = input.actions["Event"];
        putAction = input.actions["Put"];
        actionAction = input.actions["Action"];
        throwAction = input.actions["ThrowBall"];

        throwAction.performed += c =>
        {
            Instantiate(ballFactory, ballPoint.position, ballPoint.rotation);
        };

        actionAction.performed += c =>
        {
            if (grabObject)
            {
                grabObject.StartPowder();
            }
        };

        actionAction.canceled += c =>
        {
            if (grabObject)
            {
                grabObject.StopPowder();
            }
        };

        putAction.performed += c => Put();

        eventAction.performed += OnMyEvent;

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

    private void OnMyEvent(CallbackContext context)
    {
        print("OnMyEvent");
        // 앞에 있는 충돌체를 검사하고 싶다.
        Ray ray = new Ray(Camera.main.transform.position,
                            Camera.main.transform.forward);

        //bool bHit = Physics.Raycast(ray, out RaycastHit hitInfo, 10);
        bool bHit = Physics.SphereCast(ray, 1, out RaycastHit hitInfo, 15);
        if (bHit)
        {
            // tag가 Door라면
            if (hitInfo.transform.tag.Equals("Door"))
            {
                // Animator컴포넌트를 가져와서 문을 열고싶다.
                var anim = hitInfo.transform.GetComponentInChildren<Animator>();
                if (anim)
                {
                    anim.Play("Opening");
                }
            }
            // 그렇지 않고 소화기라면
            else if (hitInfo.transform.tag.Equals("FireExtinguisher"))
            {
                // 소화기를 잡고싶다.
                print("잡았다.");
                Grab(ref hitInfo);
            }
            else if (hitInfo.transform.root.tag.Equals("NPC"))
            {
                var npc = hitInfo.transform.GetComponent<NPC>();
                if (npc)
                {
                    npc.Getup();
                }
            }
        }
    }

    FireExtinguisher grabObject;
    public Transform fireExtTarget;

    void Grab(ref RaycastHit hitInfo)
    {
        hitInfo.rigidbody.isKinematic = true;
        hitInfo.transform.parent = fireExtTarget;
        hitInfo.transform.localPosition = Vector3.zero;
        hitInfo.transform.localRotation = Quaternion.identity;

        grabObject = hitInfo.transform.GetComponent<FireExtinguisher>();
    }

    void Put()
    {
        if (grabObject)
        {
            grabObject.GetComponent<Rigidbody>().isKinematic = false;
            grabObject.transform.parent = null;
            grabObject = null;
        }
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
