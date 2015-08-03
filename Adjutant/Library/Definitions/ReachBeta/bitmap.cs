﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adjutant.Library;
using Adjutant.Library.Cache;
using Adjutant.Library.Endian;
using Adjutant.Library.DataTypes;
using bitm = Adjutant.Library.Definitions.bitmap;

namespace Adjutant.Library.Definitions.ReachBeta
{
    public class bitmap : bitm
    {
        public bitmap(CacheFile Cache, int Offset)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Offset);

            Reader.BaseStream.Position += 96; //96

            #region Sequence Chunk
            long temp = Reader.BaseStream.Position;
            int sCount = Reader.ReadInt32();
            int sOffset = Reader.ReadInt32() - Cache.Magic;
            Sequences = new List<bitm.Sequence>();
            Reader.BaseStream.Position = sOffset;
            for (int i = 0; i < sCount; i++)
                Sequences.Add(new Sequence(Cache));
            Reader.BaseStream.Position = temp + 12;
            #endregion

            #region BitmapData Chunk
            temp = Reader.BaseStream.Position;
            sCount = Reader.ReadInt32();
            sOffset = Reader.ReadInt32() - Cache.Magic;
            Bitmaps = new List<bitm.BitmapData>();
            Reader.BaseStream.Position = sOffset;
            for (int i = 0; i < sCount; i++)
                Bitmaps.Add(new BitmapData(Cache));
            Reader.BaseStream.Position = temp + 12;
            #endregion

            Reader.BaseStream.Position += 32; //152

            #region Raw Chunk A
            temp = Reader.BaseStream.Position;
            int rCount = Reader.ReadInt32();
            int rOffset = Reader.ReadInt32() - Cache.Magic;
            RawChunkAs = new List<bitm.RawChunkA>();
            Reader.BaseStream.Position = rOffset;
            for (int i = 0; i < rCount; i++)
                RawChunkAs.Add(new RawChunkA(Cache));
            Reader.BaseStream.Position = temp + 12;
            #endregion

            #region Raw Chunk B
            temp = Reader.BaseStream.Position;
            rCount = Reader.ReadInt32();
            rOffset = Reader.ReadInt32() - Cache.Magic;
            RawChunkBs = new List<bitm.RawChunkB>();
            Reader.BaseStream.Position = rOffset;
            for (int i = 0; i < rCount; i++)
                RawChunkBs.Add(new RawChunkB(Cache));
            Reader.BaseStream.Position = temp + 12;
            #endregion
        }

        new public class Sequence : bitm.Sequence
        {
            public Sequence(CacheFile Cache)
            {
                EndianReader Reader = Cache.Reader;
                Name = Reader.ReadNullTerminatedString(32);
                FirstSubmapIndex = Reader.ReadInt16();
                BitmapCount = Reader.ReadInt16();

                Reader.BaseStream.Position += 16; //52

                #region Sprite Chunk
                long temp = Reader.BaseStream.Position;
                int sCount = Reader.ReadInt32();
                int sOffset = Reader.ReadInt32() - Cache.Magic;
                Sprites = new List<bitm.Sequence.Sprite>();
                Reader.BaseStream.Position = sOffset;
                for (int i = 0; i < sCount; i++)
                    Sprites.Add(new Sprite(Cache));
                Reader.BaseStream.Position = temp + 12;
                #endregion
            }

            new public class Sprite : bitm.Sequence.Sprite
            {
                public Sprite(CacheFile Cache)
                {
                    EndianReader Reader = Cache.Reader;

                    SubmapIndex = Reader.ReadInt32();
                    Reader.ReadInt32();
                    Left = Reader.ReadSingle();
                    Right = Reader.ReadSingle();
                    Top = Reader.ReadSingle();
                    Bottom = Reader.ReadSingle();
                    RegPoint = new RealQuat(
                        Reader.ReadSingle(),
                        Reader.ReadSingle());
                }
            }
        }

        new public class BitmapData : bitm.BitmapData
        {
            public BitmapData(CacheFile Cache)
            {
                EndianReader Reader = Cache.Reader;

                Class = Reader.ReadString(4);
                Width = Reader.ReadInt16();
                Height = Reader.ReadInt16();
                Depth = Reader.ReadByte();
                Flags = new Bitmask(Reader.ReadByte());
                Type = (TextureType)Reader.ReadByte();
                Reader.ReadByte(); //dunno what this is
                Format = (TextureFormat)Reader.ReadInt16();

                if ((int)Format > 31) //change to match defined format list
                    Format -= 5;

                MoreFlags = new Bitmask(Reader.ReadInt16());
                RegX = Reader.ReadInt16();
                RegY = Reader.ReadInt16();
                MipmapCount = Reader.ReadByte();
                Reader.ReadByte();
                InterleavedIndex = Reader.ReadByte();
                Index2 = Reader.ReadByte();
                PixelsOffset = Reader.ReadInt32();
                PixelsSize = Reader.ReadInt32();

                Reader.BaseStream.Position += 16; //48
            }
        }

        new public class RawChunkA : bitm.RawChunkA
        {
            public RawChunkA(CacheFile Cache)
            {
                RawID = Cache.Reader.ReadInt32();
                Cache.Reader.ReadInt32();
            }
        }

        new public class RawChunkB : bitm.RawChunkB
        {
            public RawChunkB(CacheFile Cache)
            {
                RawID = Cache.Reader.ReadInt32();
                Cache.Reader.ReadInt32();
            }
        }
    }
}
