using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private float moveSpeed = 4f;
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;

    private float gravity = 9.81f;

    PlayerInteraction playerInteract;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerInteract = GetComponentInChildren<PlayerInteraction>();
    }

    void Update()
    {
        Move();
        Interact();

        if(Input.GetKeyDown(KeyCode.R))
        {
           UIManager.instance.ToggleRelationshipPanel();
        }
    }

    public void Interact()
    {
        if ((Input.GetButtonDown("Fire1")))
        {
            //Interact
            playerInteract.Interact();
        }
        if ((Input.GetButtonDown("Fire2")))
        {
            //Interact
            playerInteract.ItemInteract();
        }
        if ((Input.GetButtonDown("Fire3")))
        {
            //Interact
            playerInteract.ItemKeep();
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3 (horizontal,0,vertical).normalized;
        Vector3 velocity = moveSpeed * Time.deltaTime * direction;

        if(controller.isGrounded)
        {
            velocity.y = 0;
        }
        velocity.y -= Time.deltaTime * gravity;

        if(Input.GetButton("Sprint"))
        {
            moveSpeed = runSpeed;
            animator.SetBool("Running",true);
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("Running", false);

        }

        if (direction.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(direction);

            controller.Move(velocity);

        }

        animator.SetFloat("Speed",direction.magnitude);
    }
}
