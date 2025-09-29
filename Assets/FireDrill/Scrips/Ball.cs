using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 불이라면 바로 파괴하고싶다.
        if (collision.collider.CompareTag("FireObject"))
        {
            Destroy(collision.gameObject);
        }
        // 어딘가 닿았다면 나자신(Ball)을 파괴하고싶다.
        Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
