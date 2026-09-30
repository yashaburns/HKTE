using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace HKTE.Content.Projectiles.Spells.Spirit
{
    internal class VengefulSpiritImpactProjectile : ModProjectile
    {
        private const int FrameSpeed = 4;
        private const int FrameCount = 5;

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
            Projectile.scale = 1.5f;

            Projectile.timeLeft = FrameSpeed * FrameCount;
        }

        public override void AI()
        {
            Timer++;

            Projectile.frame = (int)(Timer / FrameSpeed);

            if (Projectile.frame >= FrameCount)
                Projectile.Kill();
        }
    }
}