using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
   
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public ParticleSystem ps;

    public void StartPowder()
    {
        ps.Stop();
        ps.Play();
    }

    public void StopPowder()
    {
        ps.Stop();
    }
}
