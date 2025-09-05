using DG.Tweening;
using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [Header("Configuração do Pulso")]
    public float scaleUp = 1.2f;   
    public float duration = 0.5f;  
    public float delay = 0.3f;     

    private void Start()
    {
        Vector3 initialScale = transform.localScale;

        transform.DOScale(initialScale * scaleUp, duration)
            .SetLoops(-1, LoopType.Yoyo)   
            .SetEase(Ease.InOutSine)       
            .SetDelay(delay);              
    }
}
