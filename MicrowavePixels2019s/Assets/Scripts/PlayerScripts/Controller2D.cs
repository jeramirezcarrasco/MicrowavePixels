﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The following is a modification of code from Sebastian Lague
 * 
 * The code was constructed as a result of following his tutorial here: https://www.youtube.com/watch?v=MbWK8bCAU2w
 */
public class Controller2D : RaycastController {

    float maxClimbAngle = 80;
    float maxDescendAngle = 75;

    public CollisionInfo collisions;

    public override void Start() {
		base.Start ();
    }

	public override void Update() {
		base.Update ();
	}

	public void Move(Vector3 velocity, bool standingOnPlatform = false) {
		UpdateRaycastOrigins ();

		collisions.Reset ();

		collisions.velocityOld = velocity;

		if (velocity.y < 0) {
			DescendSlope (ref velocity);
		}

		if (velocity.x != 0)
			HorizontalCollisions (ref velocity);

		if (velocity.y != 0)
			VerticalCollisions (ref velocity);

		transform.Translate (velocity);

		if (standingOnPlatform) {
			collisions.below = true;
		}
	}

	void HorizontalCollisions(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D obstacleHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            RaycastHit2D eggHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, eggMask);
            this.eggHit = eggHit;

            Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (obstacleHit) {
				if (obstacleHit.distance == 0) {
					continue;
				}

				float slopeAngle = Vector2.Angle (obstacleHit.normal, Vector2.up);

				collisions.normal = obstacleHit.normal;

				if (i == 0 && slopeAngle <= maxClimbAngle) {
					if (collisions.descendingSlope) {
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStart = obstacleHit.distance - skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope (ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}

				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (obstacleHit.distance - skinWidth) * directionX;
					rayLength = obstacleHit.distance;

					if (collisions.climbingSlope) {
						velocity.y = Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x);
					}

					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}
			}

            TriggerEgg(eggHit);
        }
	}

	void VerticalCollisions(ref Vector3 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D obstacleHit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			RaycastHit2D eggHit = Physics2D.Raycast (rayOrigin, Vector2.right * directionY, rayLength, eggMask);
            this.eggHit = eggHit;

            Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (obstacleHit) {
                velocity.y = (obstacleHit.distance - skinWidth) * directionY;
				rayLength = obstacleHit.distance;

				collisions.normal = obstacleHit.normal;

				if (collisions.climbingSlope) {
					velocity.x = velocity.y / Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign (velocity.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}

            TriggerEgg(eggHit);
        }

		if (collisions.climbingSlope) {
			float directionX = Mathf.Sign (velocity.x);
			rayLength = Mathf.Abs (velocity.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
				if (slopeAngle != collisions.slopeAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) {
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

		if (hit) {
			float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				if (Mathf.Sign (hit.normal.x) == directionX) {
					if (hit.distance - skinWidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x)) {
						float moveDistance = Mathf.Abs (velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;

						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
	}

    private void TriggerEgg(RaycastHit2D hit)
    {
        if (hit) Destroy(hit.collider.gameObject);

        if (hit && !caughtEgg)
        {
            alert.IncreaseTurretRange();
            foreach (GameObject patrol in patrols)
                patrol.GetComponent<EnemyAI_1>().onAlert = true;
            caughtEgg = true;
            canResetTurretRange = true;
        }

        if (hit && caughtEgg) resetEggCount = true;
    }

    public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public bool climbingSlope;
		public float slopeAngle, slopeAngleOld;

		public bool descendingSlope;

		public Vector3 velocityOld;

		public Vector2 normal;

		public bool isAirborne() {
			if (!above && !below && !left && !right)
				return true;
			else
				return false;
		}

		public void Reset() {
			above = below = false;
			left = right = false;
			climbingSlope = false;
			descendingSlope = false;

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}
}
