using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private static float rotationSpeed = 20;

    private SpriteRenderer spriteRenderer;
    private ParticleSystem particles;

    private bool doActivate = true;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && doActivate)
        {
            GameManager.Instance.NextLevel();
            spriteRenderer.enabled = false;
            particles.Play();
            doActivate = false;
            AudioManager.PlaySound("Star Touched");
        }
    }
}
