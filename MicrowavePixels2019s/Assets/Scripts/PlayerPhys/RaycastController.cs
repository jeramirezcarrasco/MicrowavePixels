using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*
 * The following is a modification of code from Sebastian Lague
 * 
 * The code was constructed as a result of following his tutorial here: https://www.youtube.com/watch?v=MbWK8bCAU2w
 */

public class RaycastController : MonoBehaviour
{

    public LayerMask collisionMask;

    public const float skinWidth = 0.015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    [HideInInspector] public float horizontalRaySpacing;
    [HideInInspector] public float verticalRaySpacing;

    [HideInInspector] public BoxCollider2D boxCollider;

    Vector2 standardColliderSize;

    Vector2 spriteBoundsSize;

    public RaycastOrigins raycastOrigins;

    Collider2D collidedObject;

    public virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        //Time.timeScale = 0.2f;
        spriteBoundsSize.x = boxCollider.bounds.size.x;
        spriteBoundsSize.y = boxCollider.bounds.size.y;
        boxCollider.size = spriteBoundsSize;
        standardColliderSize = boxCollider.size;
        CalculateRaySpacing();
    }

    public virtual void Update()
    {
        /*if (Player.crouching) {
			spriteBoundsSize.x = spriteCollider.bounds.size.x;
			spriteBoundsSize.y = spriteCollider.bounds.size.y;
			boxCollider.size = spriteBoundsSize;
		} else
			boxCollider.size = standardColliderSize;*/
        //CalculateRaySpacing ();
    }

    public void UpdateRaycastOrigins()
    {
        /*spriteBoundsSize.x = spriteCollider.bounds.size.x;
		spriteBoundsSize.y = spriteCollider.bounds.size.y;
		boxCollider.size = spriteBoundsSize;*/
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -1);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        /*spriteBoundsSize.x = spriteCollider.bounds.size.x;
		spriteBoundsSize.y = spriteCollider.bounds.size.y;
		boxCollider.size = spriteBoundsSize;*/
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        collidedObject = col.collider;
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
        bool clipping;
    }
}