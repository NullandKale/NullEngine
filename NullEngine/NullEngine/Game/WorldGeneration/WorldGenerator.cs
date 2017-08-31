using System;
using System.Collections.Generic;
using System.Drawing;
using NullEngine.Managers;
using NullEngine.Component;
using NullEngine;
using NullEngine.Entity;

namespace NullGame.WorldGen
{
    public class WorldGenerator
    {
        public WorldData wData;

        private int tileSize;
        private double scale;
        private OpenSimplexNoise noise;
        private CollisionManager cMan;
        private List<cCollider> colliders;

        public WorldGenerator(int seed, int worldSize, int chunkSize, double scale, int tileSize, CollisionManager collisionManager)
        {
            cMan = collisionManager;
            noise = new OpenSimplexNoise(seed);
            this.scale = scale;
            this.tileSize = tileSize;

            wData = new WorldData(seed, worldSize, chunkSize, "Game/Content/overworld.png");
            Game.rng = new Random(seed);
            GenerateWorldData();
        }

        public Chunk GenerateWorld(Point tempChunkPos)
        {
            Chunk tempChunk = new Chunk(wData.chunkSize, tempChunkPos.X, tempChunkPos.Y, wData);
            GenerateTerrain(tempChunk, tempChunkPos);

            return tempChunk;
        }

        public void GenerateColliders(worldTile[,] tempChunk)
        {
            if (colliders == null)
            {
                colliders = new List<cCollider>();
            }
            else
            {
                for (int i = 0; i < colliders.Count; i++)
                {
                    CollisionManager.removeCollider(colliders[i]);
                }
                colliders.Clear();
            }


            for (int x = 0; x < wData.chunkSize; x++)
            {
                for (int y = 0; y < wData.chunkSize; y++)
                {
                    if (tempChunk[x, y].isCollideable)
                    {
                        colliders.Add(new cCollider(getTileRect(x, y)));
                    }
                }
            }
        }

        public Rectangle getTileRect(Point tilePos)
        {
            tilePos.X = tilePos.X * tileSize;
            tilePos.Y = tilePos.Y * tileSize;
            return new Rectangle(tilePos.X, tilePos.Y, tileSize, tileSize);
        }

        public Rectangle getTileRect(int X, int Y)
        {
            X = X * tileSize;
            Y = Y * tileSize;
            return new Rectangle(X, Y, tileSize, tileSize);
        }

        ///////////////////////////////////////////////////////////////////////////
        //                  World Generation Functions Galore                    //
        //                         HERE BE DRAGONS                               //
        ///////////////////////////////////////////////////////////////////////////

        private void GenerateTerrain(Chunk tempChunk, Point tempChunkPos)
        {
            double[,] height = new double[wData.chunkSize, wData.chunkSize];

            double maxHeight = 0;

            for (int x = 0; x < wData.chunkSize; x++)
            {
                for (int y = 0; y < wData.chunkSize; y++)
                {
                    double xLoc = (((double)x / (double)wData.chunkSize) + (tempChunkPos.X * wData.chunkSize)) * scale;
                    double yLoc = (((double)y / (double)wData.chunkSize) + (tempChunkPos.Y * wData.chunkSize)) * scale;
                    height[x, y] = noise.Evaluate(xLoc, yLoc);

                    if (maxHeight < height[x, y])
                    {
                        maxHeight = height[x, y];
                    }
                }
            }

            tempChunk.backgroundTiles = new worldTile[wData.chunkSize, wData.chunkSize];

            for (int x = 0; x < wData.chunkSize; x++)
            {
                for (int y = 0; y < wData.chunkSize; y++)
                {
                    tempChunk.backgroundTiles[x, y] = new worldTile();
                    tempChunk.backgroundTiles[x, y].graphics.tAtlas = wData.tAtlas;

                    double region0 = 0.25 * maxHeight;
                    double region1 = 0.50 * maxHeight;
                    double region2 = 0.75 * maxHeight;

                    if (height[x, y] < region0)
                    {
                        int tile = Game.rng.Next(0, 3);

                        if (tile == 0)
                        {
                            tempChunk.backgroundTiles[x, y].graphics.TexID = (int)WorldTexID.grass0;
                        }
                        else if (tile == 1)
                        {
                            tempChunk.backgroundTiles[x, y].graphics.TexID = (int)WorldTexID.grass1;
                        }
                        else
                        {
                            tempChunk.backgroundTiles[x, y].graphics.TexID = (int)WorldTexID.grass2;
                        }

                        tempChunk.backgroundTiles[x, y].isCollideable = false;
                        tempChunk.backgroundTiles[x, y].isRoad = false;
                        tempChunk.backgroundTiles[x, y].type = "grass";
                    }
                    else if (height[x, y] >= region0 && height[x, y] < region1)
                    {
                        int tile = Game.rng.Next(0, 4);

                        if (tile == 0)
                        {
                            tempChunk.backgroundTiles[x, y].graphics.TexID = (int)WorldTexID.sand0;
                        }
                        else if (tile == 1)
                        {
                            tempChunk.backgroundTiles[x, y].graphics.TexID = (int)WorldTexID.sand1;
                        }
                        else if (tile == 2)
                        {
                            tempChunk.backgroundTiles[x, y].graphics.TexID = (int)WorldTexID.sand2;
                        }
                        else
                        {
                            tempChunk.backgroundTiles[x, y].graphics.TexID = (int)WorldTexID.sand3;
                        }

                        tempChunk.backgroundTiles[x, y].isCollideable = false;
                        tempChunk.backgroundTiles[x, y].isRoad = false;
                        tempChunk.backgroundTiles[x, y].type = "sand";
                    }
                    else if (height[x, y] >= region1 && height[x, y] < region2)
                    {
                        int tile = Game.rng.Next(0, 2);

                        if (tile == 0)
                        {
                            tempChunk.backgroundTiles[x, y].graphics.TexID = (int)WorldTexID.water0;
                        }
                        else
                        {
                            tempChunk.backgroundTiles[x, y].graphics.TexID = (int)WorldTexID.water1;
                        }

                        tempChunk.backgroundTiles[x, y].isCollideable = true;
                        tempChunk.backgroundTiles[x, y].isRoad = false;
                        tempChunk.backgroundTiles[x, y].type = "water";
                    }
                    else
                    {
                        tempChunk.backgroundTiles[x, y].graphics.TexID = (int)WorldTexID.water2;

                        tempChunk.backgroundTiles[x, y].isCollideable = true;
                        tempChunk.backgroundTiles[x, y].isRoad = false;
                        tempChunk.backgroundTiles[x, y].type = "water";
                    }
                }
            }
            LayRoads(tempChunk);
            NineSplice(tempChunk);

            for(int i = 0; i < wData.Dungeons.Count; i++)
            {
                Console.WriteLine("Dungeon @ [" + wData.Dungeons[i].DoorChunkLoc.X + "," + wData.Dungeons[i].DoorChunkLoc.Y + "]");
                if(wData.Dungeons[i].DoorChunkLoc.X / wData.chunkSize == tempChunkPos.X && wData.Dungeons[i].DoorChunkLoc.Y / wData.chunkSize == tempChunkPos.Y)
                {
                    tempChunk.hasDungeon = true;
                    tempChunk.dungeon = GenerateDungeon(wData.Dungeons[i]);
                }
            }
        }

        private void LayRoads(Chunk tempChunk)
        {
            worldTile roadTile = new worldTile();
            roadTile.graphics = new Tile();
            roadTile.graphics.tAtlas = wData.tAtlas;
            roadTile.graphics.TexID = (int)WorldTexID.sand4;

            roadTile.isCollideable = false;
            roadTile.isContainer = false;
            roadTile.type = "sand";
            roadTile.isRoad = true;

            for (int i = 0; i < wData.Villages.Count; i++)
            {
                Console.WriteLine("Village @ [" + wData.Villages[i].Loc.X + "," + wData.Villages[i].Loc.Y + "] [" + wData.Villages[i].ConnectedVillageLoc.X + "," + wData.Villages[i].ConnectedVillageLoc.Y + "]");
                if(wData.Villages[i].roadChunks.Contains(tempChunk.key))
                {
                    List<Point> roadPoints = Ray.pointsIntersectingRect(wData.Villages[i].VillageToVillage, tempChunk.chunkRect);
                    for(int j = 0; j < roadPoints.Count; j++)
                    {
                        Point realPoint = worldPosToChunkPos(roadPoints[j]);
                        tempChunk.backgroundTiles[realPoint.X, realPoint.Y] = roadTile;
                    }
                }
            }
        }



        private void GenerateWorldData()
        {
            int numberVillages = 5;
            int numberDungeons = 10;

            //Generate VillageLocations
            for (int i = 0; i < numberVillages; i++)
            {
                VillageData d = new VillageData();
                d.Loc = GenerateVillageChunkLocation();
                wData.Villages.Add(d);
            }

            //Connect each village to a random village
            for (int j = 0; j < wData.Villages.Count; j++)
            {
                wData.Villages[j].ConnectedVillageLoc = wData.Villages[Game.rng.Next(wData.Villages.Count)].Loc;
                wData.Villages[j].VillageToVillage = new Ray(wData.Villages[j].Loc, wData.Villages[j].ConnectedVillageLoc);

                for (int x = 0; x < wData.worldSize; x++)
                {
                    for (int y = 0; y < wData.worldSize; y++)
                    {
                        Rectangle chunkRect = new Rectangle(x * wData.chunkSize, y * wData.chunkSize, wData.chunkSize, wData.chunkSize);
                        if (Ray.isIntersectingRect(wData.Villages[j].VillageToVillage, chunkRect))
                        {
                            if (wData.Villages[j].roadChunks == null)
                            {
                                wData.Villages[j].roadChunks = new List<Point>();
                            }
                            wData.Villages[j].roadChunks.Add(new Point(x, y));
                        }
                    }
                }
            }

            for (int k = 0; k < numberDungeons; k++)
            {
                DungeonData d = new DungeonData();
                d.DoorChunkLoc = GenerateDungeonLocation();
                d.DoorLoc = new Point(d.DoorChunkLoc.X % 100, d.DoorChunkLoc.Y % 100);
                d.seed = Game.rng.Next();
                wData.Dungeons.Add(d);
            }
        }

        private void NineSplice(Chunk tempChunk)
        {
            List<nineSplice> templates = createNineSpliceTemplates();
            for (int x = 0; x < wData.chunkSize; x++)
            {
                for (int y = 0; y < wData.chunkSize; y++)
                {
                    for(int i = 0; i < templates.Count; i++)
                    {
                        nineSpliceTile(templates[i], x, y, tempChunk);
                    }
                }
            }
        }

        private List<nineSplice> createNineSpliceTemplates()
        {
            List<nineSplice> temp = new List<nineSplice>();

            worldTile waterTile = new worldTile();
            waterTile.graphics.tAtlas = wData.tAtlas;
            waterTile.graphics.TexID = (int)WorldTexID.water0;
            waterTile.type = "water";
            waterTile.isCollideable = true;
            waterTile.isContainer = false;

            worldTile grassTile = new worldTile();
            grassTile.graphics.tAtlas = wData.tAtlas;
            grassTile.graphics.TexID = (int)WorldTexID.grass0;
            grassTile.type = "grass";
            grassTile.isCollideable = false;
            grassTile.isContainer = false;

            worldTile sandTile = new worldTile();
            sandTile.graphics.tAtlas = wData.tAtlas;
            sandTile.graphics.TexID = (int)WorldTexID.sand0;
            sandTile.type = "sand";
            sandTile.isCollideable = false;
            sandTile.isContainer = false;

            nineSplice n0 = new nineSplice();
            n0.Active_1 = true;
            n0.tile_1 = waterTile;

            n0.Active_4 = true;
            n0.tile_4 = grassTile;

            n0.CenterTile = grassTile;
            n0.CenterTile.graphics.TexID = (int)WorldTexID.oceanToGrassTopMid;

            temp.Add(n0);

            nineSplice n1 = new nineSplice();
            n1.Active_1 = true;
            n1.tile_1 = waterTile;

            n1.Active_4 = true;
            n1.tile_4 = sandTile;

            n1.CenterTile = sandTile;
            n1.CenterTile.graphics.TexID = (int)WorldTexID.beachTopMid;

            temp.Add(n1);

            nineSplice n5 = new nineSplice();
            n5.Active_3 = true;
            n5.tile_3 = waterTile;

            n5.Active_4 = true;
            n5.tile_4 = sandTile;

            n5.CenterTile = sandTile;
            n5.CenterTile.graphics.TexID = (int)WorldTexID.beachMidLeft;

            temp.Add(n5);

            nineSplice n7 = new nineSplice();
            n7.Active_5 = true;
            n7.tile_5 = waterTile;

            n7.Active_4 = true;
            n7.tile_4 = sandTile;

            n7.CenterTile = sandTile;
            n7.CenterTile.graphics.TexID = (int)WorldTexID.beachMidRight;

            temp.Add(n7);

            nineSplice n8 = new nineSplice();
            n8.Active_4 = true;
            n8.tile_4 = sandTile;

            n8.Active_7 = true;
            n8.tile_7 = waterTile;

            n8.CenterTile = sandTile;
            n8.CenterTile.graphics.TexID = (int)WorldTexID.beachBottomMid;

            temp.Add(n8);

            nineSplice n2 = new nineSplice();
            n2.Active_1 = true;
            n2.tile_1 = waterTile;

            n2.Active_3 = true;
            n2.tile_3 = waterTile;

            n2.Active_4 = true;
            n2.tile_4 = sandTile;

            n2.CenterTile = sandTile;
            n2.CenterTile.graphics.TexID = (int)WorldTexID.beachTopLeft;

            temp.Add(n2);

            nineSplice n3 = new nineSplice();
            n3.Active_1 = true;
            n3.tile_1 = waterTile;

            n3.Active_4 = true;
            n3.tile_4 = sandTile;

            n3.Active_5 = true;
            n3.tile_5 = waterTile;

            n3.CenterTile = sandTile;
            n3.CenterTile.graphics.TexID = (int)WorldTexID.beachTopRight;

            temp.Add(n3);

            nineSplice n9 = new nineSplice();
            n9.Active_3 = true;
            n9.tile_3 = waterTile;

            n9.Active_4 = true;
            n9.tile_4 = sandTile;

            n9.Active_7 = true;
            n9.tile_7 = waterTile;

            n9.CenterTile = sandTile;
            n9.CenterTile.graphics.TexID = (int)WorldTexID.beachBottomLeft;

            temp.Add(n9);

            nineSplice n10 = new nineSplice();
            n10.Active_5 = true;
            n10.tile_5 = waterTile;

            n10.Active_4 = true;
            n10.tile_4 = sandTile;

            n10.Active_7 = true;
            n10.tile_7 = waterTile;

            n10.CenterTile = sandTile;
            n10.CenterTile.graphics.TexID = (int)WorldTexID.beachBottomRight;

            temp.Add(n10);

            nineSplice n4 = new nineSplice();
            n4.Active_0 = true;
            n4.tile_0 = waterTile;

            n4.Active_1 = true;
            n4.tile_1 = sandTile;

            n4.Active_3 = true;
            n4.tile_3 = sandTile;

            n4.Active_4 = true;
            n4.tile_4 = sandTile;

            n4.CenterTile = sandTile;
            n4.CenterTile.graphics.TexID = (int)WorldTexID.beachInteriorBottomRight;

            temp.Add(n4);

            nineSplice n6 = new nineSplice();
            n6.Active_1 = true;
            n6.tile_1 = sandTile;

            n6.Active_5 = true;
            n6.tile_5 = sandTile;

            n6.Active_4 = true;
            n6.tile_4 = sandTile;

            n6.Active_2 = true;
            n6.tile_2 = waterTile;

            n6.CenterTile = sandTile;
            n6.CenterTile.graphics.TexID = (int)WorldTexID.beachInteriorBottomLeft;

            temp.Add(n6);

            nineSplice n11 = new nineSplice();
            n11.Active_5 = true;
            n11.tile_5 = sandTile;

            n11.Active_7 = true;
            n11.tile_7 = sandTile;

            n11.Active_4 = true;
            n11.tile_4 = sandTile;

            n11.Active_8 = true;
            n11.tile_8 = waterTile;

            n11.CenterTile = sandTile;
            n11.CenterTile.graphics.TexID = (int)WorldTexID.beachInteriorTopLeft;

            temp.Add(n11);

            nineSplice n12 = new nineSplice();
            n12.Active_3 = true;
            n12.tile_3 = sandTile;

            n12.Active_7 = true;
            n12.tile_7 = sandTile;

            n12.Active_4 = true;
            n12.tile_4 = sandTile;

            n12.Active_6 = true;
            n12.tile_6 = waterTile;

            n12.CenterTile = sandTile;
            n12.CenterTile.graphics.TexID = (int)WorldTexID.beachInteriorTopRight;
            
            temp.Add(n12);

            nineSplice n13 = new nineSplice();
            n13.Active_4 = true;
            n13.tile_4 = grassTile;

            n13.Active_7 = true;
            n13.tile_7 = sandTile;

            n13.CenterTile = grassTile;
            n13.CenterTile.graphics.TexID = (int)WorldTexID.grassToSandTopMid;

            temp.Add(n13);

            nineSplice n14 = new nineSplice();
            n14.Active_4 = true;
            n14.tile_4 = grassTile;

            n14.Active_5 = true;
            n14.tile_5 = sandTile;

            n14.CenterTile = grassTile;
            n14.CenterTile.graphics.TexID = (int)WorldTexID.grassToSandMidLeft;

            temp.Add(n14);

            nineSplice n15 = new nineSplice();
            n15.Active_4 = true;
            n15.tile_4 = grassTile;

            n15.Active_3 = true;
            n15.tile_3 = sandTile;

            n15.CenterTile = grassTile;
            n15.CenterTile.graphics.TexID = (int)WorldTexID.grassToSandMidRight;

            temp.Add(n15);

            nineSplice n16 = new nineSplice();
            n16.Active_4 = true;
            n16.tile_4 = grassTile;

            n16.Active_1 = true;
            n16.tile_1 = sandTile;

            n16.CenterTile = grassTile;
            n16.CenterTile.graphics.TexID = (int)WorldTexID.grassToSandBottomMid;

            temp.Add(n16);

            nineSplice n17 = new nineSplice();
            n17.Active_4 = true;
            n17.tile_4 = grassTile;

            n17.Active_5 = true;
            n17.tile_5 = grassTile;

            n17.Active_7 = true;
            n17.tile_7 = grassTile;

            n17.Active_8 = true;
            n17.tile_8 = sandTile;

            n17.CenterTile = grassTile;
            n17.CenterTile.graphics.TexID = (int)WorldTexID.grassToSandTopLeft;

            temp.Add(n17);

            nineSplice n18 = new nineSplice();
            n18.Active_4 = true;
            n18.tile_4 = grassTile;

            n18.Active_3 = true;
            n18.tile_3 = grassTile;

            n18.Active_7 = true;
            n18.tile_7 = grassTile;

            n18.Active_6 = true;
            n18.tile_6 = sandTile;

            n18.CenterTile = grassTile;
            n18.CenterTile.graphics.TexID = (int)WorldTexID.grassToSandTopRight;

            temp.Add(n18);

            nineSplice n19 = new nineSplice();
            n19.Active_4 = true;
            n19.tile_4 = grassTile;

            n19.Active_1 = true;
            n19.tile_1 = grassTile;

            n19.Active_5 = true;
            n19.tile_5 = grassTile;

            n19.Active_2 = true;
            n19.tile_2 = sandTile;

            n19.CenterTile = grassTile;
            n19.CenterTile.graphics.TexID = (int)WorldTexID.grassToSandBottomLeft;

            temp.Add(n19);

            nineSplice n20 = new nineSplice();
            n20.Active_4 = true;
            n20.tile_4 = grassTile;

            n20.Active_1 = true;
            n20.tile_1 = grassTile;

            n20.Active_3 = true;
            n20.tile_3 = grassTile;

            n20.Active_0 = true;
            n20.tile_0 = sandTile;

            n20.CenterTile = grassTile;
            n20.CenterTile.graphics.TexID = (int)WorldTexID.grassToSandBottomRight;

            temp.Add(n20);

            nineSplice n21 = new nineSplice();
            n21.Active_4 = true;
            n21.tile_4 = grassTile;

            n21.Active_5 = true;
            n21.tile_5 = grassTile;

            n21.Active_7 = true;
            n21.tile_7 = grassTile;

            n21.Active_1 = true;
            n21.tile_1 = sandTile;

            n21.Active_3 = true;
            n21.tile_3 = sandTile;

            n21.CenterTile = grassTile;
            n21.CenterTile.graphics.TexID = (int)WorldTexID.beachToGrassTopLeft;

            temp.Add(n21);

            nineSplice n22 = new nineSplice();
            n22.Active_4 = true;
            n22.tile_4 = grassTile;

            n22.Active_3 = true;
            n22.tile_3 = grassTile;

            n22.Active_7 = true;
            n22.tile_7 = grassTile;

            n22.Active_1 = true;
            n22.tile_1 = sandTile;

            n22.Active_5 = true;
            n22.tile_5 = sandTile;

            n22.CenterTile = grassTile;
            n22.CenterTile.graphics.TexID = (int)WorldTexID.beachToGrassTopRight;

            temp.Add(n22);

            nineSplice n23 = new nineSplice();
            n23.Active_4 = true;
            n23.tile_4 = grassTile;

            n23.Active_1 = true;
            n23.tile_1 = grassTile;

            n23.Active_5 = true;
            n23.tile_5 = grassTile;

            n23.Active_3 = true;
            n23.tile_3 = sandTile;

            n23.Active_7 = true;
            n23.tile_7 = sandTile;

            n23.CenterTile = grassTile;
            n23.CenterTile.graphics.TexID = (int)WorldTexID.beachToGrassBottomLeft;

            temp.Add(n23);

            nineSplice n24 = new nineSplice();
            n24.Active_4 = true;
            n24.tile_4 = grassTile;

            n24.Active_1 = true;
            n24.tile_1 = grassTile;

            n24.Active_3 = true;
            n24.tile_3 = grassTile;

            n24.Active_5 = true;
            n24.tile_5 = sandTile;

            n24.Active_7 = true;
            n24.tile_7 = sandTile;

            n24.CenterTile = grassTile;
            n24.CenterTile.graphics.TexID = (int)WorldTexID.beachToGrassBottomRight;

            temp.Add(n24);

            return temp;
        }

        private void nineSpliceTile(nineSplice n, int xPos, int yPos, Chunk tempChunk)
        {
            bool nMatch0 = false;
            bool nMatch1 = false;
            bool nMatch2 = false;
            bool nMatch3 = false;
            bool nMatch4 = false;
            bool nMatch5 = false;
            bool nMatch6 = false;
            bool nMatch7 = false;
            bool nMatch8 = false;

            if (n.Active_0)
            {
                if(isInChunk(xPos - 1, yPos - 1))
                {
                    if (n.tile_0.type == tempChunk.backgroundTiles[xPos - 1, yPos - 1].type)
                    {
                        nMatch0 = true;
                    }
                }
            }
            else
            {
                nMatch0 = true;
            }

            if (n.Active_1)
            {
                if (isInChunk(xPos, yPos - 1))
                {
                    if (n.tile_1.type == tempChunk.backgroundTiles[xPos, yPos - 1].type)
                    {
                        nMatch1 = true;
                    }
                }
            }
            else
            {
                nMatch1 = true;
            }

            if (n.Active_2)
            {
                if (isInChunk(xPos + 1, yPos - 1))
                {
                    if (n.tile_2.type == tempChunk.backgroundTiles[xPos + 1, yPos - 1].type)
                    {
                        nMatch2 = true;
                    }
                }
            }
            else
            {
                nMatch2 = true;
            }

            if (n.Active_3)
            {
                if (isInChunk(xPos - 1, yPos))
                {
                    if (n.tile_3.type == tempChunk.backgroundTiles[xPos - 1, yPos].type)
                    {
                        nMatch3 = true;
                    }
                }
            }
            else
            {
                nMatch3 = true;
            }

            if (n.Active_4)
            {
                if (isInChunk(xPos, yPos))
                {
                    if (n.tile_4.type == tempChunk.backgroundTiles[xPos, yPos].type)
                    {
                        nMatch4 = true;
                    }
                }
            }
            else
            {
                nMatch4 = true;
            }

            if (n.Active_5)
            {
                if (isInChunk(xPos + 1, yPos))
                {
                    if (n.tile_5.type == tempChunk.backgroundTiles[xPos + 1, yPos].type)
                    {
                        nMatch5 = true;
                    }
                }
            }
            else
            {
                nMatch5 = true;
            }

            if (n.Active_6)
            {
                if (isInChunk(xPos - 1, yPos + 1))
                {
                    if (n.tile_6.type == tempChunk.backgroundTiles[xPos - 1, yPos + 1].type)
                    {
                        nMatch6 = true;
                    }
                }
            }
            else
            {
                nMatch6 = true;
            }

            if (n.Active_7)
            {
                if (isInChunk(xPos, yPos + 1))
                {
                    if (n.tile_7.type == tempChunk.backgroundTiles[xPos, yPos + 1].type)
                    {
                        nMatch7 = true;
                    }
                }
            }
            else
            {
                nMatch7 = true;
            }

            if (n.Active_8)
            {
                if (isInChunk(xPos + 1, yPos + 1))
                {
                    if (n.tile_8.type == tempChunk.backgroundTiles[xPos + 1, yPos + 1].type)
                    {
                        nMatch8 = true;
                    }
                }
            }
            else
            {
                nMatch8 = true;
            }

            if(nMatch0 && nMatch1 && nMatch2 && nMatch3 && nMatch4 && nMatch5 && nMatch6 && nMatch7 && nMatch8 && isInChunk(xPos,yPos))
            {
                tempChunk.backgroundTiles[xPos, yPos] = n.CenterTile;
            }
        }

        private void DecorateChunk(Chunk tempChunk)
        {

        }

        private Chunk GenerateDungeon(DungeonData dData)
        {
            Random rng = new Random(dData.seed);
            Point chunk = new Point(dData.DoorChunkLoc.X / 100, dData.DoorChunkLoc.Y / 100);
            int randomFillPercent = 50;
            int iterations = 5;
            int xSize = rng.Next(20, 80);

            Chunk tempChunk = new Chunk(xSize, chunk.X, chunk.Y);

            worldTile wall = new worldTile();
            wall.graphics = new Tile();
            wall.graphics.tAtlas = wData.tAtlas;
            wall.graphics.TexID = (int)WorldTexID.wall;
            wall.isCollideable = true;
            wall.isContainer = false;
            wall.isRoad = false;
            wall.type = "wall";

            worldTile air = new worldTile();
            air.graphics = new Tile();
            air.graphics.tAtlas = wData.tAtlas;
            air.graphics.TexID = (int)WorldTexID.air;
            air.isCollideable = false;
            air.isContainer = false;
            air.isRoad = false;
            air.type = "air";

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < xSize; y++)
                {
                    if(rng.Next(100) > randomFillPercent - 1)
                    {
                        tempChunk.backgroundTiles[x, y] = wall;
                    }
                    else
                    {
                        tempChunk.backgroundTiles[x, y] = air;
                    }
                }
            }

            for(int i = 0; i < iterations; i++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    for (int y = 0; y < xSize; y++)
                    {
                        int neighbors = getNumberNeighbors(tempChunk, x, y, "wall", xSize);

                        if (tempChunk.backgroundTiles[x, y].type == "wall")
                        {
                            if (neighbors >= 4)
                            {
                                tempChunk.backgroundTiles[x, y] = wall;
                            }
                        }
                        else
                        {
                            if (neighbors >= 5)
                            {
                                tempChunk.backgroundTiles[x, y] = wall;
                            }
                        }
                    }
                }

            }

            return tempChunk;
        }

        private int getNumberNeighbors(Chunk tempChunk, int X, int Y, string type, int size)
        {
            int count = 0;

            for(int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        if(isinChunk(size, X + i, Y + j))
                        {
                            if (tempChunk.backgroundTiles[X + i, Y + j].type == type)
                            {
                                count++;
                            }
                        }
                    }
                }
            }

            return count;
        }

        private bool isinChunk(int max, int x, int y)
        {
            return !(x < 0 || x >= max || y < 0 || y >= max);
        }

        private Point GenerateDungeonLocation()
        {
            Point p = new Point(Game.rng.Next(wData.worldSize * wData.chunkSize), Game.rng.Next(wData.worldSize * wData.chunkSize));
            for(int i = 0; i < wData.Dungeons.Count; i++)
            {
                if(wData.Dungeons[i].DoorChunkLoc == p)
                {
                    return GenerateDungeonLocation();
                }
            }

            return p;
        }

        private Point GenerateVillageChunkLocation()
        {
            Point p = new Point(Game.rng.Next(wData.worldSize * wData.chunkSize), Game.rng.Next(wData.worldSize * wData.chunkSize));
            for(int i = 0; i < wData.Villages.Count; i++)
            {
                if(wData.Villages[i].Loc == p)
                {
                    return GenerateVillageChunkLocation();
                }
            }
            return p;
        }

        private bool isInChunk(int x, int y)
        {
            return !((x < 0 || x >= wData.chunkSize) || (y < 0 || y >= wData.chunkSize));
        }

        private bool isInWorld(int x, int y)
        {
            return !((x < 0 || x >= wData.worldSize) || (y < 0 || y >= wData.worldSize));
        }

        private Rectangle getChunkRect(Chunk c)
        {
            Point TopChunkPos = inChunkPosToWorldPos(Point.Empty, c);
            return new Rectangle(TopChunkPos.X, TopChunkPos.Y, wData.chunkSize, wData.chunkSize);
        }

        private Point inChunkPosToWorldPos(Point p, Chunk c)
        {
            return new Point(c.key.X * wData.chunkSize + p.X, c.key.Y * wData.chunkSize + p.Y);
        }

        private Point worldPosToChunkPos(Point p)
        {
            return new Point(p.X % wData.chunkSize, p.Y % wData.chunkSize);
        }

        private Point inChunkPosToWorldPos(Point ChunkPos, Point WorldPos)
        {
            return new Point(WorldPos.X * wData.chunkSize + ChunkPos.X, WorldPos.Y * wData.chunkSize + ChunkPos.Y);
        }
    }
}
