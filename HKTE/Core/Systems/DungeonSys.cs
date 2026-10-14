using HKTE.Content.Items.Weapons.Melee;
using HKTE.Content.Tiles.Interactables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HKTE.Core.Systems
{
    internal class DungeonSys : ModSystem
    {
        public override void PostWorldGen()
        {
            ushort chestType =
                (ushort)ModContent.TileType<HallownestChest>();

            int nailType = ModContent.ItemType<Nail>();

            List<Point> candidates = new();

            // Scan for tiles backed by Dungeon walls.
            for (int x = 100; x < Main.maxTilesX - 100; x += 2)
            {
                for (int y = (int)Main.worldSurface;
                     y < Main.maxTilesY - 200;
                     y += 2)
                {
                    if (!WorldGen.InWorld(x + 1, y + 1, 10))
                        continue;

                    Tile tile = Main.tile[x, y];

                    if (tile.WallType == 0 ||
                        !Main.wallDungeon[tile.WallType])
                        continue;

                    // Check whether the chest's two columns are clear.
                    if (Main.tile[x, y].HasTile ||
                        Main.tile[x + 1, y].HasTile ||
                        Main.tile[x, y - 1].HasTile ||
                        Main.tile[x + 1, y - 1].HasTile)
                        continue;

                    // Both columns need solid ground beneath them.
                    if (!WorldGen.SolidTile(x, y + 1) ||
                        !WorldGen.SolidTile(x + 1, y + 1))
                        continue;

                    candidates.Add(new Point(x, y));
                }
            }

            Mod.Logger.Info(
                $"Hallownest chest: found {candidates.Count} candidate locations."
            );

            // Shuffle candidate order so the chest isn't always in one place.
            for (int i = candidates.Count - 1; i > 0; i--)
            {
                int j = WorldGen.genRand.Next(i + 1);
                (candidates[i], candidates[j]) =
                    (candidates[j], candidates[i]);
            }

            foreach (Point pos in candidates)
            {
                int chestIndex = WorldGen.PlaceChest(
                    pos.X,
                    pos.Y,
                    chestType
                );

                if (chestIndex < 0)
                    continue;

                Main.chest[chestIndex].item[0].SetDefaults(nailType);

                int westEastFeet =
                    (pos.X - Main.maxTilesX / 2) * 2;

                string direction = westEastFeet < 0 ? "West" : "East";

                Mod.Logger.Info(
                    $"Hallownest Chest: X={pos.X}, Y={pos.Y} | " +
                    $"{Math.Abs(westEastFeet)} feet {direction}"
                );

                return;
            }

            Mod.Logger.Warn(
                "Hallownest chest failed: no candidate location could accept it."
            );
        }
    }
}
