using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller2D))]

public class Player1 : MonoBehaviour
{
    #region Variables
    #region Inspector_Values
    [Range(1, 50)] public float moveSpeed = 20;
    [Range(1, 100)] public float jumpPower = 50;
    [Range(1, 300)] public float fallSpeed = 100;
    [Range(1, 100)] public float groundedTraction = 50;
    [Range(1, 100)] public float airialTraction = 20;
    #endregion Inspector_Values

    #region Player_States
    bool doubleJumped = false;
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
    Vector2 input;
    Controller2D controller;
    #endregion Input

    #region Collisions
    [HideInInspector] public BoxCollider2D boxCollider;
    #endregion Collisions
    #endregion Variables

    void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    void Update()
    {
        DetectDirectionalInputs();

        ColPhysChecks();

        DetectJumping();

        MovePlayer();
    }

    void DetectDirectionalInputs()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    void ColPhysChecks()
    {
        gravity = -fallSpeed * 2f;
        accelerationTimeGrounded = 1f / groundedTraction;
        accelerationTimeAirborne = 1f / airialTraction;

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
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

        if (Input.GetKeyDown("space") &&
            !controller.collisions.below &&
            !doubleJumped)
        {
            velocity.y = jumpPower;
            doubleJumped = true;
        }

        if (controller.collisions.below) doubleJumped = false;
    }

    void MovePlayer()
    {
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
