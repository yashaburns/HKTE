using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace HKTE.Content.Projectiles.Spells.Spirit
{
    internal class VengefulSpiritCastProjectile : ModProjectile
    {
        private const int FrameSpeed = 3;
        private const int FrameCount = 7;

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = FrameCount;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            // 4 frames × 2 ticks = 8 ticks
            Projectile.timeLeft = FrameSpeed * FrameCount;

            Projectile.scale = 2f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Rectangle sourceRectangle = new Rectangle(
                0,
                Projectile.frame * 128,
                128,
                128
            );

            Vector2 origin;

            if (Projectile.spriteDirection == 1)
                origin = new Vector2(112f, 63f);
            else
                origin = new Vector2(20f, 63f);

            
            SpriteEffects effects = Projectile.spriteDirection == 1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;
            

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                sourceRectangle,
                lightColor,
                0f,
                origin,
                Projectile.scale,
                effects,
                0
            );

            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.spriteDirection = player.direction;

            Timer++;

            // Follow the player
            Projectile.Center = player.Center + new Vector2(256f * player.direction, 0f);

            // Don't move
            Projectile.velocity = Vector2.Zero;

            // Animate
            Projectile.frame = (int)(Timer / FrameSpeed);

            // Kill after animation finishes
            if (Projectile.frame >= FrameCount)
            {
                Projectile.Kill();
            }
        }
    }
}