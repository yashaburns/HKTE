using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HKTE.Content.NPCS.Enemies
{
    public class Mosquito : ModNPC
    {

        private int wanderTimer;
        private Vector2 wanderDirection;
        private float homeY;
        private bool isWandering;

        private const int chargeDuration = 70;
        private Vector2 dashTarget;

        public override void SetDefaults()
        {
            NPC.width = 128;
            NPC.height = 128;
            NPC.scale = 1f;

            NPC.damage = 20;
            NPC.defense = 5;
            NPC.lifeMax = 40;

            NPC.knockBackResist = 0.5f;

            NPC.noGravity = true;
            NPC.noTileCollide = false;

            NPC.aiStyle = -1; // Custom AI
        }

        public override void OnSpawn(IEntitySource source)
        {
            homeY = NPC.Center.Y;
        }

        public override void AI()
        {
            // Custom AI logic for the Mosquito NPC
            Player target = Main.player[NPC.target];

            NPC.TargetClosest();

            switch ((int)NPC.ai[0])
            {
                case 0:
                    Wander(target);
                    break;
                case 1:
                    Charge(target);
                    break;
                case 2:
                    Dash();
                    break;
                case 3:
                    Recover();
                    break;
            }

            if (NPC.velocity.X > 0.05f)
                NPC.spriteDirection = 1;
            else if (NPC.velocity.X < -0.05f)
                NPC.spriteDirection = -1;

        }

        private void Wander(Player target)
        {
            float distance = Vector2.Distance(NPC.Center, target.Center);

            // Player is close enough → start charging
            if (distance <= 500f)
            {
                NPC.ai[0] = 1;
                NPC.ai[1] = 0;
                NPC.velocity = Vector2.Zero;
                return;
            }

            wanderTimer--;

            if (wanderTimer <= 0)
            {
                if (isWandering)
                {
                    // Finished moving → start slowing down
                    isWandering = false;

                    // Stay in the resting state for 30–45 frames
                    wanderTimer = Main.rand.Next(45, 65);
                }
                else
                {
                    // Finished resting → choose a new direction
                    isWandering = true;

                    wanderTimer = Main.rand.Next(60, 120);

                    float x = Main.rand.NextFloat(-1f, 1f);
                    float y = Main.rand.NextFloat(-0.5f, 0.5f);

                    float yDifference = NPC.Center.Y - homeY;

                    if (yDifference > 80f)
                        y = -Math.Abs(y);

                    else if (yDifference < -80f)
                        y = Math.Abs(y);

                    wanderDirection = new Vector2(x, y)
                        .SafeNormalize(Vector2.UnitX);
                }
            }

            if (isWandering)
            {
                // Slowly accelerate toward the chosen direction
                Vector2 desiredVelocity = wanderDirection * 1.5f;

                NPC.velocity = Vector2.Lerp(
                    NPC.velocity,
                    desiredVelocity,
                    0.02f
                );
            }
            else
            {
                // Slowly decelerate to a stop
                NPC.velocity = Vector2.Lerp(
                    NPC.velocity,
                    Vector2.Zero,
                    0.05f
                );
            }
        }

        private void Charge(Player target)
        {
            NPC.velocity = Vector2.Lerp(
                NPC.velocity,
                Vector2.Zero,
                0.05f
            );

            Vector2 direction = target.Center - NPC.Center;

            if (direction.X != 0)
                NPC.spriteDirection = direction.X > 0 ? 1 : -1;

            NPC.ai[1]++;

            if (NPC.ai[1] >= chargeDuration)
            {
                dashTarget = target.Center;

                NPC.ai[0] = 2;
                NPC.ai[1] = 0;

                Vector2 dashDirection = dashTarget - NPC.Center;

                if (dashDirection != Vector2.Zero)
                    dashDirection.Normalize();

                NPC.velocity = dashDirection * 8f;
            }
        }

        private void Dash()
        {
            NPC.ai[1]++;

            if (NPC.ai[1] >= 40)
            {
                NPC.ai[0] = 3;
                NPC.ai[1] = 0;
            }
        }

        private void Recover()
        {
            NPC.velocity = Vector2.Lerp(
                NPC.velocity,
                Vector2.Zero,
                0.1f
            );

            NPC.ai[1]++;

            if (NPC.ai[1] >= 30)
            {
                NPC.ai[0] = 0;
                NPC.ai[1] = 0;
            }
        }
    }
}
