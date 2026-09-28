using HKTE.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace HKTE.Content.Items.Weapons.Melee
{
    public class Nail : ModItem
    {
        private bool canSpawnDust = false;
        private int dmgBonus = 0;
        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.autoReuse = false;
            Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
            Item.noMelee = true; // The projectile will do the damage and not the item
            Item.scale = 1.5f;

            Item.shoot = ModContent.ProjectileType<NailProjectile>(); // The projectile is what makes a shortsword work
            Item.shootSpeed = 1f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
        }

        // Since this weapon is a projectile (uses noUseGraphic), it isn't naturally considered a melee weapon for the purposes of prefixes. This allows the expected prefixes to be applied.
        public override bool MeleePrefix() => true;

        public override bool Shoot(
        Player player,
        EntitySource_ItemUse_WithAmmo source,
        Vector2 position,
        Vector2 velocity,
        int type,
        int damage,
        float knockback)
        {
            Vector2 direction = Main.MouseWorld - player.Center;

            if (direction.LengthSquared() > 0f)
                direction.Normalize();

            Projectile.NewProjectile(
                source,
                player.Center,
                direction,
                type,
                damage,
                knockback,
                player.whoAmI
            );

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronShortsword, 1)
                .AddIngredient(ItemID.Geode, 1)
                .AddIngredient(ItemID.SilverOre, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (canSpawnDust)
            {
                if (Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Stone);

                    Dust dust = Main.dust[d];

                    dust.noGravity = true;
                }
            }
        }
    }
}
