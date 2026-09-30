using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using HKTE.Content.Projectiles.Spells.Wraiths;
using HKTE.Content.Projectiles.Spells.Spirit;

namespace HKTE.Content.Items.Weapons.Staffs
{
    internal class Spellcaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true; // This makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.useStyle = 5;
            Item.useTime = 25;
            Item.useAnimation = 25;

            Item.UseSound = SoundID.Item20;

            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;

            Item.scale = 1.75f;
            Item.SetWeaponValues(20, 5);

            Item.noMelee = true;
        }

        public override bool? UseItem(Player player)
        {
            Vector2 mouse = Main.MouseWorld;
            Vector2 direction = mouse - player.Center;

            bool castUp = direction.Y < -Math.Abs(direction.X);

            if (castUp)
            {
                Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    Vector2.UnitY * -1,
                    ModContent.ProjectileType<HowlingWraithsProjectile>(),
                    Item.damage,
                    Item.knockBack,
                    player.whoAmI
                );
            }
            else
            {
                int facing = direction.X >= 0 ? 1 : -1;

                Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    Vector2.UnitX * facing * 10f,
                    ModContent.ProjectileType<VengefulSpiritProjectile>(),
                    Item.damage,
                    Item.knockBack,
                    player.whoAmI
                );
            }

            return true;
        }

        public override void HoldItem(Player player)
        {
            // Player is currently casting
            if (player.itemAnimation > 0)
            {
                player.velocity = Vector2.Zero;

                player.controlLeft = false;
                player.controlRight = false;
                player.controlUp = false;
                player.controlDown = false;
                player.controlJump = false;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CobaltBar, 60)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
