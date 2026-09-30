using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HKTE.Content.Projectiles.Spells.Wraiths
{
    internal class HowlingWraithsProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 11;
        }
        public int killTime = 40;
        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 256;
            Projectile.height = 256;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(180, 180, 180, 255);
        }

        public override bool? CanHitNPC(NPC target)
        {
            Player player = Main.player[Projectile.owner];

            // Don't hit enemies that are level with or below the player
            if (target.Center.Y >= player.Center.Y)
                return false;

            return null;
        }

        public override void AI()
        {
            Timer++;
            float scale = MathHelper.Clamp((Timer / 3f) * 0.1f, 0f, 1.5f);
            Projectile.frame = Math.Min((int)(Timer / 3f), 10);

            if (Main.rand.NextBool(2))
            {
                Vector2 pos = Projectile.Center + new Vector2(
                    Main.rand.NextFloat(-60f, 60f),
                    Main.rand.NextFloat(50f, 100f)
                );

                Dust dust = Dust.NewDustPerfect(
                    pos,
                    DustID.WhiteTorch,
                    new Vector2(0, 1f),
                    100,
                    Color.White,
                    1f
                );

                dust.noGravity = true;
            }

            if (Timer == 1)
            {
                for (int i = 0; i < 25; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2CircularEdge(4f, 6f);

                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.WhiteTorch,
                        velocity,
                        100,
                        Color.White,
                        1.5f
                    );

                    dust.noGravity = true;
                }
            }

            if (Timer > 15 && Main.rand.NextBool(2))
            {
                Vector2 spawnPos = Projectile.Center + Main.rand.NextVector2Circular(250f, 250f);
                Vector2 velocity = Projectile.Center - spawnPos;

                velocity.Normalize();
                velocity *= 2f;

                Dust dust = Dust.NewDustPerfect(
                    spawnPos,
                    DustID.WhiteTorch,
                    velocity,
                    100,
                    Color.Red,
                    1.2f
                );

                dust.noGravity = true;
            }

            if (Timer == 30)
            {
                for (int i = 0; i < 40; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2CircularEdge(12f, 12f);

                    Dust dust = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.WhiteTorch,
                        velocity,
                        100,
                        Color.White,
                        2.5f
                    );

                    dust.noGravity = true;
                }
            }

            if (scale > 0.5f && Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(80f, 80f),
                    DustID.WhiteTorch,
                    Vector2.Zero,
                    100,
                    Color.White,
                    1.5f
                );

                dust.noGravity = true;
            }

            
            if (Timer >= killTime - 10)
            {
                Projectile.scale *= 0.8f;
            }
            else
            {
                Projectile.scale = scale;

                Player player = Main.player[Projectile.owner];

                // Keep the visual centered where we want it
                Projectile.Center = player.Center + new Vector2(0, -120 * scale);
            }
           

            if (Timer >= killTime)
            {
                Projectile.Kill();
                return;
            }

        }
    }
}