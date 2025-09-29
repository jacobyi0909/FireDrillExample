using UnityEngine;
using DG.Tweening;

public class TweenTest : MonoBehaviour
{
    //public AnimationCurve curve;
    void Start()
    {
        DOTween.Init(false, false, LogBehaviour.Default);
        transform.DOMoveX(5, 1).SetEase(Ease.InBounce);
        //origin = transform.position;
    }

    //Vector3 origin;
    void Update()
    {
        //float y = curve.Evaluate(Time.time);

        //transform.position = origin + Vector3.right * y * 5;
        
    }
}
