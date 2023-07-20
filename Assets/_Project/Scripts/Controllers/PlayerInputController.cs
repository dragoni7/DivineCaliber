﻿using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace dragoni7
{
    public class PlayerInputController : Singletone<PlayerInputController>
    {
        private bool isAttacking = false;
        private Vector2 currentMove;
        private PlayerController pController;
        public Camera MainCamera { get; private set; }

        private void Start()
        {
            pController = PlayerController.Instance;
            MainCamera = FindAnyObjectByType<Camera>();
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            currentMove = context.ReadValue<Vector2>();
        }
        public void OnFire(InputAction.CallbackContext context)
        {
            isAttacking = context.ReadValueAsButton();
        }

        public void OnAbility1(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                StartCoroutine(pController.CurrentPlayer.Abilities[0].Execute(pController.CurrentPlayer));
            }
        }

        public void OnAbility2(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                StartCoroutine(pController.CurrentPlayer.Abilities[1].Execute(pController.CurrentPlayer));
            }
        }

        public void OnAbility3(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                StartCoroutine(pController.CurrentPlayer.Abilities[2].Execute(pController.CurrentPlayer));
            }
        }

        public void FixedUpdate()
        {
            AbstractPlayer player = pController.CurrentPlayer;

            if (GameController.Instance.CurrentState == GameController.GameState.PlayingLevel)
            {
                if (!player.canAttack && !player.canMove)
                {
                    return;
                }

                if (isAttacking && player.canAttack)
                {
                    player.Weapon.PerformAttack();
                    player.CurrentSpeed = player.Stats.shootingSpeed;
                }
                else
                {
                    player.CurrentSpeed = player.Stats.speed;
                }

                if (player.canMove)
                {
                    // Move player
                    player.rb.velocity = currentMove * player.CurrentSpeed;

                    // Rotate player's weapon
                    Vector3 mousePosition = MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    Vector3 lookDirection = mousePosition - player.Weapon.transform.position;
                    float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

                    player.Weapon.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
    }
}

