﻿using System.Collections.Generic;
using UnityEngine;

namespace dragoni7
{
    public class PlayerDetector : AbstractDetector
    {
        [SerializeField] private float targetDetectionRange;

        [SerializeField] private LayerMask obstacleLayerMask, playerLayerMask;

        [SerializeField] private bool showGizmos = true;

        private List<Transform> colliders;

        public override void Detect(AIData aIData)
        {
            // Find if player is near
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);

            if (playerCollider != null)
            {
                // Check if can see player
                Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetDetectionRange, obstacleLayerMask);
                aIData.attackDirection = direction;

                // Make sure that the collider we see is on Player layer
                if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
                    colliders = new List<Transform>() { playerCollider.transform };
                }
                else
                {
                    colliders = null;
                    aIData.attackDirection = Vector3.zero;
                }
            }
            else
            {
                // Enemy doesnt see player
                colliders = null;
                aIData.attackDirection = Vector3.zero;
            }
            aIData.targets = colliders;
        }

        private void OnDrawGizmosSelected()
        {
            if (showGizmos == false)
            {
                return;
            }

            Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

            if (colliders == null)
            {
                return;
            }

            Gizmos.color = Color.magenta;

            foreach (var item in colliders) 
            {
                Gizmos.DrawSphere(item.position, 0.3f);
            }
        }
    }
}
