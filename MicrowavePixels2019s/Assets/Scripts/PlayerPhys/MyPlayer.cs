using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller2D))]

public class MyPlayer : MonoBehaviour
{
    public Animator animator;

    #region Vars

    #region Inspector_Values
    [Range(1, 50)] public float moveSpeed = 10;
    [Range(1, 100)] public float jumpPower = 15;
    [Range(1, 300)] public float fallSpeed = 30;
    [Range(1, 100)] public float groundedTraction = 50;
    [Range(1, 100)] public float airialTraction = 20;
    #endregion Inspector_Values

    #region Player_States
    bool doubleJumped = false;

    [HideInInspector] public bool facingLeft = false;
    [HideInInspector] public bool facingRight = true;

    bool moving = false;
    bool movingLeft = false;
    bool movingRight = false;

    bool grounded;
    bool climbingSlope;
    bool descendingSlope;

    bool lockFacing = false;
    #endregion Player_States

    #region Physics
    float accelerationTimeGrounded;
    float accelerationTimeAirborne;

    float timeToJumpApex;
    float gravity;

    float velocityXSmoothing;

    Vector3 velocity;
    Vector3 standingPos;
    #endregion Physics

    #region Input
    Vector2 playerInput;
    Controller2D controller;
    //P1GameManager gm;
    #endregion Input

    #region Collisions
    Collider2D collidedObject;

    [HideInInspector] public BoxCollider2D boxCollider;

    bool clipping;
    float clipAmount;

    bool leftCollision, rightCollision, bottomCollision, topCollision;

    PolygonCollider2D p1Hurtbox, p2Hurtbox;
    #endregion Collisions

    #endregion Vars
    float horizontalMove = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        animator.SetFloat("Speed", Math.Abs(horizontalMove));

        DetectDirectionalInputs();

        ColPhysChecks();

        DetectMovement();

        DetectJumping();

        MovePlayer();

        if (controller.collisions.below) print("grounded");
        else print("airborne");

        //if (boxCollider.bounds.min.y )
    }

    void DetectDirectionalInputs()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
    }

    void ColPhysChecks()
    {
        grounded = controller.collisions.below;
        climbingSlope = controller.collisions.climbingSlope;
        descendingSlope = controller.collisions.descendingSlope;

        gravity = -fallSpeed * 2f;
        accelerationTimeGrounded = 1f / groundedTraction;
        accelerationTimeAirborne = 1f / airialTraction;

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    void DetectMovement()
    {
        if (playerInput.x == 0 && playerInput.y == 0)
            moving = false;
        else
            moving = true;

        if (playerInput.x > 0)
        {
            movingRight = true;
        }

        if (playerInput.x < 0)
        {
            movingLeft = true;
        }
    }

    void DetectJumping()
    {
        if (Input.GetKeyDown("space"))
        {
            if (controller.collisions.below)
            {
                velocity.y = jumpPower;
            }
        }

        if (Input.GetKeyDown("space") && !doubleJumped)
        {
            velocity.y = jumpPower;
            doubleJumped = true;
        }

        if (controller.collisions.below)
        {
            doubleJumped = false;
        }
    }

    void MovePlayer()
    {
        float targetVelocityX = playerInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
