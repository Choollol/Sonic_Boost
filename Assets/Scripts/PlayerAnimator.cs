using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public enum Action
    {
        Idle, Walk, Jump
    }

    private Action action;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private PlayerController playerController;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (GameManager.isGameActive)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                spriteRenderer.flipX = true;
                action = Action.Walk;
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                spriteRenderer.flipX = false;
                action = Action.Walk;
            }
            else
            {
                action = Action.Idle;
            }
            if (playerController.isJumping)
            {
                action = Action.Jump;
            }
        }
        else
        {
            action = Action.Idle;
        }

        animator.Play("Base Layer." + action + ".Player_" + action);
    }
}
