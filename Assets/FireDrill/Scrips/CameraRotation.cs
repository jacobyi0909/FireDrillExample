using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    // 부모 게임오브젝트에 붙어있는 PlayerInput 컴포넌트를 가져오고싶다.
    InputAction mouseAction;
    void Awake()
    {
        var input = GetComponentInParent<PlayerInput>();
        mouseAction = input.actions["Mouse"];
    }

    float rx;
    float ry;
    public float rotSpeed = 30f;
    void Update()
    {
        //float mx = Input.GetAxis("Mouse X");
        //float my = Input.GetAxis("Mouse Y");
        var mouseDelta = mouseAction.ReadValue<Vector2>();

        rx -= mouseDelta.y * rotSpeed * Time.deltaTime;
        ry += mouseDelta.x * rotSpeed * Time.deltaTime;

        rx = Mathf.Clamp(rx, -60f, 60f);

        transform.eulerAngles = new Vector3(rx, ry, 0);
    }
}
