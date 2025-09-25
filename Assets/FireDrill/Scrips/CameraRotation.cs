using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    
    void Start()
    {
        
    }

    float rx;
    float ry;
    public float rotSpeed = 60f;
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rx -= my * rotSpeed * Time.deltaTime;
        ry += mx * rotSpeed * Time.deltaTime;

        rx = Mathf.Clamp(rx, -60f, 60f);

        transform.eulerAngles = new Vector3(rx, ry, 0);
    }
}
