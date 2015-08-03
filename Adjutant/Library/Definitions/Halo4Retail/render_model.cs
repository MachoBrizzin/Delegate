﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adjutant.Library;
using Adjutant.Library.Cache;
using Adjutant.Library.Endian;
using Adjutant.Library.DataTypes;
using mode = Adjutant.Library.Definitions.render_model;

namespace Adjutant.Library.Definitions.Halo4Retail
{
    public class render_model : mode
    {
        public render_model(CacheFile Cache, int Offset)
        {
            cache = Cache;
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Offset);

            Name = Cache.Strings.GetItemByID(Reader.ReadInt32());
            Flags = new Bitmask(Reader.ReadInt32());

            Reader.BaseStream.Position += 4; //12

            #region Regions Block
            long temp = Reader.BaseStream.Position;
            int iCount = Reader.ReadInt32();
            int iOffset = Reader.ReadInt32() - Cache.Magic;
            Regions = new List<mode.Region>();
            Reader.BaseStream.Position = iOffset;
            for (int i = 0; i < iCount; i++)
                Regions.Add(new Region(Cache));
            Reader.BaseStream.Position = temp + 12;
            #endregion

            Reader.ReadInt32();
            InstancedGeometryIndex = Reader.ReadInt32();

            #region Instanced Geometry Block
            temp = Reader.BaseStream.Position;
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            GeomInstances = new List<mode.InstancedGeometry>();
            Reader.BaseStream.Position = iOffset;
            for (int i = 0; i < iCount; i++)
                GeomInstances.Add(new InstancedGeometry(Cache));
            Reader.BaseStream.Position = temp + 12;
            #endregion
            
            Reader.BaseStream.Position += 4; //48

            #region Nodes Block
            temp = Reader.BaseStream.Position;
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            Nodes = new List<mode.Node>();
            Reader.BaseStream.Position = iOffset;
            for (int i = 0; i < iCount; i++)
                Nodes.Add(new Node(Cache));
            Reader.BaseStream.Position = temp + 12;
            #endregion

            #region MarkerGroups Block
            temp = Reader.BaseStream.Position;
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            MarkerGroups = new List<mode.MarkerGroup>();
            Reader.BaseStream.Position = iOffset;
            for (int i = 0; i < iCount; i++)
                MarkerGroups.Add(new MarkerGroup(Cache));
            Reader.BaseStream.Position = temp + 12;
            #endregion

            #region Shaders Block
            temp = Reader.BaseStream.Position;
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            Shaders = new List<mode.Shader>();
            Reader.BaseStream.Position = iOffset;
            for (int i = 0; i < iCount; i++)
                Shaders.Add(new Shader(Cache));
            Reader.BaseStream.Position = temp + 12;
            #endregion

            Reader.BaseStream.Position += 20; //104

            #region ModelParts Block
            temp = Reader.BaseStream.Position;
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            ModelSections = new List<mode.ModelSection>();
            Reader.BaseStream.Position = iOffset;
            for (int i = 0; i < iCount; i++)
                ModelSections.Add(new ModelSection(Cache, iOffset + 112 * i));
            Reader.BaseStream.Position = temp + 12;
            #endregion

            Reader.BaseStream.Position += 12; //128

            #region BoundingBox Block
            temp = Reader.BaseStream.Position;
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            BoundingBoxes = new List<mode.BoundingBox>();
            Reader.BaseStream.Position = iOffset;
            for (int i = 0; i < iCount; i++)
                BoundingBoxes.Add(new BoundingBox(Cache, iOffset + 52 * i));
            Reader.BaseStream.Position = temp + 12;
            #endregion

            Reader.BaseStream.Position += 48; //188

            #region NodeMapGroup Block
            temp = Reader.BaseStream.Position;
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            NodeIndexGroups = new List<mode.NodeIndexGroup>();
            Reader.BaseStream.Position = iOffset;
            for (int i = 0; i < iCount; i++)
                NodeIndexGroups.Add(new NodeIndexGroup(Cache));
            Reader.BaseStream.Position = temp + 12;
            #endregion

            Reader.BaseStream.Position += 48; //248
            
            RawID = Reader.ReadInt32();
            
            Reader.BaseStream.Position += 140; //392
        }

        new public class Region : mode.Region
        {
            public Region(CacheFile Cache)
            {
                EndianReader Reader = Cache.Reader;

                Name = Cache.Strings.GetItemByID(Reader.ReadInt32());

                long temp = Reader.BaseStream.Position;
                int pCount = Reader.ReadInt32();
                int pOffset = Reader.ReadInt32() - Cache.Magic;
                Reader.BaseStream.Position = pOffset;
                Permutations = new List<mode.Region.Permutation>();
                for (int i = 0; i < pCount; i++)
                    Permutations.Add(new Permutation(Cache));
                Reader.BaseStream.Position = temp + 12;
            }

            new public class Permutation : mode.Region.Permutation
            {
                public Permutation(CacheFile Cache)
                {
                    EndianReader Reader = Cache.Reader;

                    Name = Cache.Strings.GetItemByID(Reader.ReadInt32());
                    PieceIndex = Reader.ReadInt16();
                    PieceCount = Reader.ReadInt16();
                    
                    Reader.BaseStream.Position += 20; //28
                }
            }
        }

        new public class InstancedGeometry : mode.InstancedGeometry
        {
            public InstancedGeometry(CacheFile Cache)
            {
                EndianReader Reader = Cache.Reader;

                Name = Cache.Strings.GetItemByID(Reader.ReadInt32());
                NodeIndex = Reader.ReadInt32();

                TransformScale = Reader.ReadSingle();

                TransformMatrix = new Matrix();

                TransformMatrix.m11 = Reader.ReadSingle();
                TransformMatrix.m12 = Reader.ReadSingle();
                TransformMatrix.m13 = Reader.ReadSingle();

                TransformMatrix.m21 = Reader.ReadSingle();
                TransformMatrix.m22 = Reader.ReadSingle();
                TransformMatrix.m23 = Reader.ReadSingle();

                TransformMatrix.m31 = Reader.ReadSingle();
                TransformMatrix.m32 = Reader.ReadSingle();
                TransformMatrix.m33 = Reader.ReadSingle();

                TransformMatrix.m41 = Reader.ReadSingle();
                TransformMatrix.m42 = Reader.ReadSingle();
                TransformMatrix.m43 = Reader.ReadSingle();
            }
        }

        new public class Node : mode.Node
        {
            public Node(CacheFile Cache)
            {
                EndianReader Reader = Cache.Reader;

                Name = Cache.Strings.GetItemByID(Reader.ReadInt32());
                ParentIndex = Reader.ReadInt16();
                FirstChildIndex = Reader.ReadInt16();
                NextSiblingIndex = Reader.ReadInt16();
                Reader.ReadInt16();
                Position = new RealQuat(
                    Reader.ReadSingle(), 
                    Reader.ReadSingle(),
                    Reader.ReadSingle());
                Rotation = new RealQuat(
                    Reader.ReadSingle(),
                    Reader.ReadSingle(),
                    Reader.ReadSingle(),
                    Reader.ReadSingle());

                TransformScale = Reader.ReadSingle();

                TransformMatrix = new Matrix();

                TransformMatrix.m11 = Reader.ReadSingle();
                TransformMatrix.m12 = Reader.ReadSingle();
                TransformMatrix.m13 = Reader.ReadSingle();

                TransformMatrix.m21 = Reader.ReadSingle();
                TransformMatrix.m22 = Reader.ReadSingle();
                TransformMatrix.m23 = Reader.ReadSingle();

                TransformMatrix.m31 = Reader.ReadSingle();
                TransformMatrix.m32 = Reader.ReadSingle();
                TransformMatrix.m33 = Reader.ReadSingle();

                TransformMatrix.m41 = Reader.ReadSingle();
                TransformMatrix.m42 = Reader.ReadSingle();
                TransformMatrix.m43 = Reader.ReadSingle();

                DistanceFromParent = Reader.ReadSingle();

                Reader.BaseStream.Position += 16; //112
            }
        }

        new public class MarkerGroup : mode.MarkerGroup
        {
            public MarkerGroup(CacheFile Cache)
            {
                EndianReader Reader = Cache.Reader;

                Name = Cache.Strings.GetItemByID(Reader.ReadInt32());

                long temp = Reader.BaseStream.Position;
                int mCount = Reader.ReadInt32();
                int mOffset = Reader.ReadInt32() - Cache.Magic;
                Reader.BaseStream.Position = mOffset;
                Markers = new List<mode.MarkerGroup.Marker>();
                for (int i = 0; i < mCount; i++)
                    Markers.Add(new Marker(Cache));
                Reader.BaseStream.Position = temp + 12;
            }

            new public class Marker : mode.MarkerGroup.Marker
            {
                public Marker(CacheFile Cache)
                {
                    EndianReader Reader = Cache.Reader;

                    RegionIndex = Reader.ReadByte();
                    PermutationIndex = Reader.ReadByte();
                    NodeIndex = Reader.ReadByte();
                    Reader.ReadByte();
                    Position = new RealQuat(
                        Reader.ReadSingle(),
                        Reader.ReadSingle(),
                        Reader.ReadSingle());
                    Rotation = new RealQuat(
                        Reader.ReadSingle(),
                        Reader.ReadSingle(),
                        Reader.ReadSingle(),
                        Reader.ReadSingle());
                    Scale = Reader.ReadSingle();

                    Reader.BaseStream.Position += 12; //48
                }
            }
        }

        new public class Shader : mode.Shader
        {
            public Shader(CacheFile Cache)
            {
                EndianReader Reader = Cache.Reader;

                Reader.BaseStream.Position += 12; 

                tagID = Reader.ReadInt32();

                Reader.BaseStream.Position += 28; //44
            }
        }

        new public class ModelSection : mode.ModelSection
        {
            public ModelSection(CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                #region Submesh Block
                long temp = Reader.BaseStream.Position;
                int sCount = Reader.ReadInt32();
                int sOffset = Reader.ReadInt32() - Cache.Magic;
                Reader.BaseStream.Position = sOffset;
                Submeshes = new List<mode.ModelSection.Submesh>();
                for (int i = 0; i < sCount; i++)
                    Submeshes.Add(new Submesh(Cache));
                Reader.BaseStream.Position = temp + 12;
                #endregion

                #region Subset Block
                temp = Reader.BaseStream.Position;
                sCount = Reader.ReadInt32();
                sOffset = Reader.ReadInt32() - Cache.Magic;
                Reader.BaseStream.Position = sOffset;
                Subsets = new List<mode.ModelSection.Subset>();
                for (int i = 0; i < sCount; i++)
                    Subsets.Add(new Subset(Cache));
                Reader.BaseStream.Position = temp + 12;
                #endregion

                #region Other
                VertsIndex = Reader.ReadInt16();
                Reader.BaseStream.Position += 6; //32

                Reader.ReadInt32();

                Reader.BaseStream.Position += 6; //42

                FacesIndex = Reader.ReadInt16();

                Reader.BaseStream.Position += 3; //47

                TransparentNodesPerVertex = Reader.ReadByte();
                NodeIndex = Reader.ReadByte();
                VertexFormat = Reader.ReadByte();
                OpaqueNodesPerVertex = Reader.ReadByte();

                Reader.BaseStream.Position += 61; //112
                #endregion
            }

            new public class Submesh : mode.ModelSection.Submesh
            {
                public Submesh(CacheFile Cache)
                {
                    EndianReader Reader = Cache.Reader;

                    ShaderIndex = Reader.ReadInt16();
                    Reader.ReadInt16();
                    FaceIndex = Reader.ReadInt32();
                    FaceCount = Reader.ReadInt32();
                    SubsetIndex = Reader.ReadUInt16();
                    SubsetCount = Reader.ReadUInt16();
                    Reader.ReadInt16();
                    Reader.ReadInt16();
                    VertexCount = Reader.ReadUInt16();
                    Reader.ReadInt16();
                }
            }

            new public class Subset : mode.ModelSection.Subset
            {
                public Subset(CacheFile Cache)
                {
                    EndianReader Reader = Cache.Reader;

                    FaceIndex = Reader.ReadInt32();
                    FaceCount = Reader.ReadInt32();
                    SubmeshIndex = Reader.ReadUInt16();
                    VertexCount = Reader.ReadUInt16();
                    Reader.ReadSingle();
                }
            }
        }

        new public class BoundingBox : mode.BoundingBox
        {
            public BoundingBox(CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                Reader.ReadInt32();
                XBounds = new RealBounds(Reader.ReadSingle(), Reader.ReadSingle());
                YBounds = new RealBounds(Reader.ReadSingle(), Reader.ReadSingle());
                ZBounds = new RealBounds(Reader.ReadSingle(), Reader.ReadSingle());
                UBounds = new RealBounds(Reader.ReadSingle(), Reader.ReadSingle());
                VBounds = new RealBounds(Reader.ReadSingle(), Reader.ReadSingle());

                Reader.BaseStream.Position += 8; //54
            }
        }

        new public class NodeIndexGroup : mode.NodeIndexGroup
        {
            public NodeIndexGroup(CacheFile Cache)
            {
                EndianReader Reader = Cache.Reader;

                long temp = Reader.BaseStream.Position;
                int nmCount = Reader.ReadInt32();
                int nmOffset = Reader.ReadInt32() - Cache.Magic;
                Reader.BaseStream.Position = nmOffset;
                NodeIndices = new List<mode.NodeIndexGroup.NodeIndex>();
                for (int i = 0; i < nmCount; i++)
                    NodeIndices.Add(new NodeIndex(Cache));
                Reader.BaseStream.Position = temp + 12;
            }

            new public class NodeIndex : mode.NodeIndexGroup.NodeIndex
            {
                public NodeIndex(CacheFile Cache)
                {
                    EndianReader Reader = Cache.Reader;

                    Index = Reader.ReadByte();
                }
            }
        }

    }
}