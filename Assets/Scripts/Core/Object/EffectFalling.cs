using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectFalling : MonoBehaviour
{
    [SerializeField] private float _squashScale = 0.8f; 
    [SerializeField] private float _squashTime = 0.15f; 
    [SerializeField] private float _returnTime = 0.25f;

    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void PlaySquashEffect()
    {
        DOTween.Kill(transform);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(_originalScale * _squashScale, _squashTime));
        seq.Append(transform.DOScale(_originalScale, _returnTime));
    }

    public void PlayDisappearEffect(System.Action onComplete = null)
    {
        DOTween.Kill(transform);
        transform.DOScale(Vector3.zero, 0.3f)
            .OnComplete(() => Destroy(gameObject));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            PlaySquashEffect();
        }
    }
}
