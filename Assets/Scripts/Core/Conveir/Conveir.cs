using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveir : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _textureScrollSpeed = .5f;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        float offset = Time.time * _textureScrollSpeed;
        _renderer.material.mainTextureOffset = new Vector2(0, -offset);
    }

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 movement = Vector3.right * _speed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
}
