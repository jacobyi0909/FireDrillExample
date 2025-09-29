using UnityEngine;

public class PowderCollision : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        FireObject obj = other.GetComponent<FireObject>();
        if (obj)
        {
            obj.TakeDamage();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
