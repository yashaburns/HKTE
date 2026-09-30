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
        private const int FrameCount = 4;
        private const int FrameSpeed = 2;

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;

            Projectile.DamageType = DamageClass.Magic;
        }

        /*
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        */

        public override void AI()
        {
            Timer++;

            // Loop through the 4 animation frames
            Projectile.frame = ((int)(Timer / FrameSpeed)) % FrameCount;

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
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Destroy the projectile when it hits terrain
            Projectile.Kill();
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Rectangle sourceRectangle = new Rectangle(
                0,
                Projectile.frame * 64,
                64,
                64
            );

            Vector2 origin = sourceRectangle.Size() / 2f;

            SpriteEffects effects = SpriteEffects.None;

            if (Projectile.velocity.X > 0)
                effects = SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                sourceRectangle,
                new Color(180, 180, 180, 255),
                0f,
                origin,
                Projectile.scale,
                effects,
                0
            );

            return false;
        }
    }
}