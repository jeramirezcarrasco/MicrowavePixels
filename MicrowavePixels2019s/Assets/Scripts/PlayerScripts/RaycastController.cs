using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]

/*
 * The following is a modification of code from Sebastian Lague
 * 
 * The code was constructed as a result of following his tutorial here: https://www.youtube.com/watch?v=MbWK8bCAU2w
 */
public class RaycastController : MonoBehaviour {

	public LayerMask collisionMask, eggMask;

	public const float skinWidth = 0.015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	[HideInInspector] public float horizontalRaySpacing;
	[HideInInspector] public float verticalRaySpacing;

	[HideInInspector] public BoxCollider2D boxCollider;
	[HideInInspector] public PolygonCollider2D spriteCollider;

	Vector2 spriteBoundsSize;

	public RaycastOrigins raycastOrigins;

    [HideInInspector] public Alert alert;
    [HideInInspector] public GameObject[] patrols;
    [HideInInspector] public bool caughtEgg = false, canResetTurretRange = false, eggHit, resetEggCount = false;

    public virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		spriteCollider = GetComponent<PolygonCollider2D> ();
		CalculateRaySpacing ();
        spriteBoundsSize.x = boxCollider.bounds.size.x;
        spriteBoundsSize.y = boxCollider.bounds.size.y;
        boxCollider.size = spriteBoundsSize;
		CalculateRaySpacing ();
        alert = GetComponent<Alert>();
        patrols = GameObject.FindGameObjectsWithTag("Patrol");
	}

	public virtual void Update () { }

	public void UpdateRaycastOrigins() {
		Bounds bounds = boxCollider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	public void CalculateRaySpacing() {
		Bounds bounds = boxCollider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
		bool clipping;
	}
}
