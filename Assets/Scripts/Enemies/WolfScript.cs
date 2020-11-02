﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Enemies
{
    class WolfScript : BasicEnemyController
    {
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {

            base.Update();
        }

        private void FixedUpdate()
        {
            Behaviour();

            base.FixedUpdate();
        }

        private void Behaviour()
        {
            if (playerDetected)
            {
                if (Vector2.Distance(transform.position, player.position) > stoppingDistance && !(gfxAnim.GetBool("Knockback")))
                {
                    direction = player.position - transform.position;
                    Vector2 facingDirection = player.position - transform.position;
                    FaceDirection(facingDirection);

                }
            }
            else
            {
                Wander();
            }
            if (gfxAnim.GetBool("Knockback"))
            {
                knockbackTimer += 1;
                direction = -1 * (player.position - transform.position);
                print(knockbackIntensity);
                if (knockbackTimer > knockbackIntensity - knockbackResistance)
                {
                    gfxAnim.SetBool("Knockback", false);
                }
            }
        }
    }
}
