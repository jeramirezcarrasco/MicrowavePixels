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
    //[Range(1, 100)] public float divekickSpeed = 40;
    //[Range(0, 100)] public float slideAttackSpeed = 20;
    //[Range(1, 100)] public float dashSpeed = 40;
    //[Range(0, 20)] public float crouchSpeed = 5;
    //[Range(1, 30)] public float dashActiveFrames = 15;
    //[Range(1, 120)] public float clingActiveFrames = 30;
    //[Range(1, 30)] public float slideAttackActiveFrames = 6;
    //[Range(1, 50)] public float slideAttackCooldownFrames = 5;
    //[Range(1, 50)] public float projectileCooldownFrames = 5;
    //[Range(1, 30)] public float guardActiveFrames = 15;
    //[Range(1, 100)] public float pushBackSpeed = 50;
    //[Range(1, 100)] public float pushBackActiveFrames = 10;
    //[Range(1, 100)] public float stunActiveFrames = 60;
    #endregion Inspector_Values

    #region Frame_Counts
    float dashFrameCount = 0;
    float clingFrameCount = 0;
    float slideAttackFrameCount = 0;
    float projectileFrameCount = 0;
    float guardFrameCount = 0;
    float pushBackFrameCount = 0;
    float stunFrameCount = 0;
    #endregion Frame_Counts

    #region Player_States
    bool doubleJumped = false;
    [HideInInspector] public bool divekicked = false;
    [HideInInspector] public bool slideAttacked = false;
    [HideInInspector] public bool guarded = false;
    [HideInInspector] public bool shotProjectile = false;
    [HideInInspector] public bool inPushBack = false;
    [HideInInspector] public bool wallSplat = false;
    [HideInInspector] public bool stunned = false;
    [HideInInspector] public bool continueStun = false;
    bool canWallSplat = true;
    bool canSlideAttack = true;
    [HideInInspector] public bool facingLeft = false;
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool hitWithProjectile = false;
    bool dashed = false;
    bool dashedLeft = false;
    bool dashedRight = false;
    bool moving = false;
    public static bool crouching = false;
    bool movingLeft = false;
    bool movingRight = false;
    bool rotated = false;
    [HideInInspector] public bool inRespawn = false;

    bool hitOnLeftSide = false;
    bool hitOnRightSide = false;

    bool splattedLeft = false, splattedRight = false;
    bool stunnedOnLeftWall = false, stunnedOnRightWall = false;

    bool grounded;
    bool climbingSlope;
    bool descendingSlope;
    bool clinging = false;
    [HideInInspector] public bool dead = false;

    bool lockFacing = false;

    [HideInInspector] public bool invincible = false;
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

    //#region Sprites
    //SpriteRenderer playerSprite;
    //SpriteRenderer playerIcon;
    //SpriteRenderer opponentSprite;
    //SpriteRenderer opponentIcon;

    //public Sprite runLeftSprite, runRightSprite, upClingLeftSprite, upClingRightSprite, upLeftClingSprite,
    //upRightClingSprite, leftClingSprite, rightClingSprite, jumpFallLeft, jumpFallRight, crouchLeftSprite,
    //crouchRightSprite, divekickRightSprite, divekickLeftSprite, slideAttackLeftSprite, slideAttackRightSprite,
    //guardRightSprite, guardLeftSprite, pushBackLeftSprite, pushBackRightSprite, wallSplatLeftSprite,
    //    wallSplatRightSprite, stunnedLeftSprite, stunnedRightSprite, dashLeftSprite, dashRightSprite;
    //#endregion Sprites

    #region Collisions
    Collider2D collidedObject;

    [HideInInspector] public BoxCollider2D boxCollider;

    bool clipping;
    float clipAmount;

    bool leftCollision, rightCollision, bottomCollision, topCollision;

    PolygonCollider2D p1Hurtbox, p2Hurtbox;
    #endregion Collisions

    #region Respawn
    GameObject respawnUp, respawnDown, respawnRight, respawnLeft, opponent;

    BoxCollider2D activeAreaUp, activeAreaDown, activeAreaRight, activeAreaLeft;

    [HideInInspector] public bool inUpArea, inDownArea, inLeftArea, inRightArea;
    #endregion Respawn

    #region External_Scripts
    Player1 p1Script;
    #endregion External_Scripts

    #region Audio
    [HideInInspector] public AudioSource killSound, clingSound;

    bool canPlayClingSound = true;
    #endregion Audio

    #region Win
    public static bool p1Win = false;
    public static bool p2Win = false;

    //Canvas p1WinCanvas, p2WinCanvas;
    #endregion Win
    #endregion Variables

    void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    void Update()
    {
        DetectDirectionalInputs();

        ColPhysChecks();

        DetectMovement();

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
        if (input.x == 0 && input.y == 0)
            moving = false;
        else
            moving = true;

        if (input.x > 0)
        {
            movingRight = true;
        }

        if (input.x < 0)
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
