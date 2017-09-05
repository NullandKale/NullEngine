using System;
using System.Collections.Generic;
using System.Drawing;
using NullEngine.Entity;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NullEngine;
using NullEngine.Managers;
using NullGame.WorldGen;

namespace NullGame.Managers
{
    public class WorldManager
    {
        public static WorldManager man;
        public static Texture2D worldTex
        {
            get
            {
                return man.currentChunk.getBackgroundTexture();
            }
        }

        public string worldFileLoc = "Game/Content/World/";

        public Point currentChunkPos;
        public Chunk currentChunk;

        public int worldMaxX
        {
            get
            {
                return chunkSizeX * tileSize;
            }
        }

        public int worldMaxY
        {
            get
            {
                return chunkSizeY * tileSize;
            }
        }

        private int worldSizeX;
        private int worldSizeY;
        private int chunkSizeX;
        private int chunkSizeY;
        private int tileSize;
        private double scale;

        public WorldGenerator wGen;
        private Dictionary<Point, Chunk> worldCache;

        public WorldManager(int seed, int worldSize, int chunkSize, double scale, int tileSize, CollisionManager collisionManager, Point curretChunk, NullEngine.StateMachine.iState parent)
        {
            if(man == null)
            {
                man = this;
            }
            else
            {
                throw new SingletonException(this);
            }

            worldSizeX = worldSize;
            worldSizeY = worldSize;
            chunkSizeX = chunkSize;
            chunkSizeY = chunkSize;
            currentChunkPos = curretChunk;
            currentChunk = new Chunk(chunkSize, currentChunkPos.X, currentChunkPos.Y);

            this.tileSize = tileSize;
            this.scale = scale;

            wGen = new WorldGenerator(seed, worldSize, chunkSize, scale, tileSize, collisionManager, parent);
            worldFileLoc += seed.ToString() + "/";
            worldCache = new Dictionary<Point, Chunk>();
            LoadChunk();
        }

        public void LoadChunk()
        {
            if (worldCache.ContainsKey(currentChunkPos))
            {
                currentChunk = worldCache[currentChunkPos];
            }
            else
            {
                if (ChunkOnDisk(currentChunkPos))
                {
                    currentChunk = LoadChunkFromDisk(currentChunkPos);
                    worldCache.Add(currentChunk.key, currentChunk);
                }
                else
                {
                    currentChunk = wGen.GenerateWorld(currentChunkPos);
                    worldCache.Add(currentChunk.key, currentChunk);
                    SaveChunkToDisk(currentChunk);
                }
            }

            wGen.GenerateColliders(currentChunk.backgroundTiles);
        }

        public static Tile worldTileToTile(worldTile tile)
        {
            return tile.graphics;
        }

        public static Point worldToTile(Point worldPos)
        {
            if(worldPos.X >= man.chunkSizeX * man.tileSize || worldPos.Y >= man.chunkSizeY * man.tileSize)
            {
                return new Point(0, 0);
            }
            return new Point(worldPos.X / 16 * transform.masterScale, worldPos.Y / 16 * transform.masterScale);
        }

        public static Point worldToTile(int X, int Y)
        {
            if (X >= man.chunkSizeX * man.tileSize || Y >= man.chunkSizeY * man.tileSize)
            {
                return new Point(0, 0);
            }
            return new Point(X / man.tileSize, Y / man.tileSize);
        }

        public void ChangeCurrentChunk(int X, int Y)
        {
            if(X == 0 && Y == 0)
            {
                return;
            }

            if(currentChunkPos.X + X < 0)
            {
                currentChunkPos.X = worldSizeX - 1;
            }
            else if(currentChunkPos.X + X >= worldSizeX)
            {
                currentChunkPos.X = 0;
            }
            else
            {
                currentChunkPos.X += X;
            }

            if (currentChunkPos.Y + Y < 0)
            {
                currentChunkPos.Y = worldSizeY - 1;
            }
            else if (currentChunkPos.Y + Y >= worldSizeY)
            {
                currentChunkPos.Y = 0;
            }
            else
            {
                currentChunkPos.Y += Y;
            }
            LoadChunk();
        }

        public string GenChunkFileName(Point key)
        {
            return worldFileLoc + key.X + "-" + key.Y + ".wor";
        }

        public void SaveChunkToDisk(Chunk c)
        {
            if(!ChunkOnDisk(c.key))
            {
                IFormatter formatter = new BinaryFormatter();
                string chunkFile = GenChunkFileName(c.key);
                GenFileStructure();
                Stream stream = new FileStream(chunkFile, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, c);
                stream.Close();
            }
        }

        public void GenFileStructure()
        {
            Directory.CreateDirectory(worldFileLoc);
        }

        public Chunk LoadChunkFromDisk(Point loc)
        {
            Console.WriteLine("Loading Chunk @ [" + loc.X + "," + loc.Y + "]");
            string chunkFile = GenChunkFileName(loc);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(chunkFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            Chunk c;

            if (stream.Length == 0)
            {
                Console.WriteLine("Error in chunk saved to disk. Regnerating Chunk and Saving to Disk");
                stream.Close();
                c = wGen.GenerateWorld(loc);
                SaveChunkToDisk(c);
            }
            else
            {
                c = (Chunk)formatter.Deserialize(stream);
                stream.Close();
                c.AfterDiskLoad(wGen.wData.tAtlas);
            }

            return c;
        }

        public bool ChunkOnDisk(Point loc)
        {
            if(!Game.DEBUG_doNotLoad_SETTOFALSE)
            {
                string chunkFile = GenChunkFileName(loc);
                if (File.Exists(chunkFile))
                {
                    Stream stream = new FileStream(chunkFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (stream.Length == 0)
                    {
                        stream.Close();
                        return false;
                    }
                    else
                    {
                        stream.Close();
                        Console.WriteLine("Chunk on disk");
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Not loading chunks from disk");
                return false;
            }
        }
    }
}
