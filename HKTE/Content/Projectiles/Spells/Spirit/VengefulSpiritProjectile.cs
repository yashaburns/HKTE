using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace HKTE.Content.Projectiles.Spells.Spirit
{
    internal class VengefulSpiritProjectile : ModProjectile
    {
        private const int FrameSpeed = 2;

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 20;

            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.scale = 1.5f;
            Projectile.spriteDirection = 0;

            Projectile.DamageType = DamageClass.Magic;
        }

        
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        

        public override void AI()
        {
            Timer++;

            Projectile.spriteDirection = Projectile.velocity.X < 0 ? 1 : -1;

            // Loop through the 4 animation frames
            Projectile.frame = ((int)(Timer / FrameSpeed)) % 4;

            // Optional: some small trailing particles
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.WhiteTorch,
                    -Projectile.velocity * 0.1f,
                    100,
                    Color.White,
                    0.8f
                );

                dust.noGravity = true;
            }
            if (Main.rand.NextBool(2))
            {
                Vector2 trailPosition = (Projectile.Center - new Vector2(0, -25f)) - Projectile.velocity.SafeNormalize(Vector2.Zero) * 25f;

                Dust dust = Dust.NewDustPerfect(
                    trailPosition + Main.rand.NextVector2Circular(25f, 25f),
                    DustID.WhiteTorch,
                    -Projectile.velocity * 0.05f,
                    100,
                    Color.White,
                    Main.rand.NextFloat(1f, 2f)
                );

                dust.noGravity = false;
            }
        }

        public override bool TileCollideStyle(
            ref int width,
            ref int height,
            ref bool fallThrough,
            ref Vector2 hitboxCenterFrac)
            {
                hitboxCenterFrac.Y = -0.1f;
                return true;
            }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Inflate(20, 20);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Destroy the projectile when it hits terrain
            Projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center + new Vector2(0f, -25f),
                Vector2.Zero,
                ModContent.ProjectileType<VengefulSpiritImpactProjectile>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner
            );
        }
    }
}