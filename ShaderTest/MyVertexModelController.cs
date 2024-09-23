using LegionForge;
using LegionForge.Blocks;
using LegionForge.Globals;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;
using System;
using System.Linq;
using Buffer = Stride.Graphics.Buffer;
//using System;

namespace ShaderTest
{
    public class MyVertexModelController : StartupScript
    {
        public Model model => GenerateModel();
        ModelComponent modelComponent;
        Texture Texture;
        static readonly int blockID = 1;
        int blockCount = 8;
        public readonly TextureDescription defaultTexDesc = TextureDescription.New2D(64, 64, PixelFormat.R32G32B32_Float, TextureFlags.ShaderResource);
        public override void Start()
        {
            //  Texture = LF_GenerateTexture2dArray.GenerateTexture2DArray(GraphicsDevice, Game.GraphicsContext, Content, Log);
            Texture = GenerateTextureSequence();
            // Texture.SetData(Game.GraphicsContext.CommandList, LF_GenerateTexture2dArray.GenerateDefaultImage(LF_GenerateTexture2dArray.imageDescription).ToDataBox(),0,0);
            Entity.Add(modelComponent = new ModelComponent(model));
            modelComponent.Model.Meshes.Clear();
            modelComponent.Model.Meshes.Add(GenerateMesh());
        }
        private Texture GenerateTextureArray()
        {
            TextureDescription textureDescription = TextureDescription.New2D(defaultTexDesc.Width, defaultTexDesc.Height, defaultTexDesc.Format, defaultTexDesc.Flags, arraySize: blockCount);
            Image image = Image.New(textureDescription);
            for (int i = 0; i < blockCount; i++)
            {
                for (int x = 0; x < image.Description.Width; x++)
                {
                    for (int y = 0; y < image.Description.Height; y++)
                    {
                        //Vector3 color = new Vector3
                        //    (
                        //    x: x % 1,
                        //    y: y * 2 % 1,
                        //    z: x * 3 % 1
                        //    );
                        // image.PixelBuffer[i, 1].SetPixel(x, y, color);
                        image.PixelBuffer[i, 1].SetPixel(x, y, new Vector3(i / blockCount, i / blockCount, i / blockCount));
                    }
                }
            }
            return Stride.Graphics.Texture.New2D(GraphicsDevice, textureDescription.Width, textureDescription.Height, textureDescription.Format, image.ToDataBox());
        }

        private Texture GenerateTextureSequence()
        {
            TextureDescription textureDescription = defaultTexDesc;
            Texture texture = Texture.New2D(GraphicsDevice, textureDescription.Width, textureDescription.Height, 1, textureDescription.Format, textureDescription.Flags,blockCount);//, textureDescription.Flags, textureDescription.ArraySize, textureDescription.Usage);

            if ((texture.Width * texture.Height * texture.ArraySize * texture.ViewFormat.SizeInBytes()) != (textureDescription.Width * textureDescription.Height * textureDescription.ArraySize * blockCount * textureDescription.Format.SizeInBytes()))
            {
                throw new InvalidOperationException("image size doesnt match texture size");
            }
            for (int i = 0; i < blockCount; i++)
            {
                Image image = Image.New(textureDescription);
                Vector3 color = new Vector3(Random.Shared.Next(0, 255)/255f, Random.Shared.Next(0, 255) / 255f, Random.Shared.Next(0, 255) / 255f);
                for (int x = 0; x < image.Description.Width; x++)
                {
                    for (int y = 0; y < image.Description.Height; y++)
                    {
                         image.GetPixelBuffer(0, 0).SetPixel(x, y, color);
                    }
                }
                DataPointer pointer = new DataPointer(image.DataPointer,image.TotalSizeInBytes);
                texture.SetData(Game.GraphicsContext.CommandList, pointer, i,0);
            }
            return texture;
        }

        private Model GenerateModel()
        {
            return new Model()
            {
                //Meshes = { GenerateMesh() },
                Materials = { NewMaterial() }
            };
        }
        private Mesh GenerateMesh()
        {
            // BufferDescription bufferDesc = new BufferDescription(MyVertexDefiniton.Layout.CalculateSize(), BufferFlags.VertexBuffer,GraphicsResourceUsage.Default, MyVertexDefiniton.Layout.VertexStride);//, MyVertexDefiniton.Layout.VertexStride);
            // Buffer vertexBuffer = Buffer.New(GraphicsDevice, bufferDesc);//Buffer.New(GraphicsDevice, bufferDesc, Vertices, GraphicsResourceUsage.Default);//Stride.Graphics.Buffer.Structured.New(GraphicsDevice, VertexData);
            //Buffer vertexBuffer = Buffer.New(GraphicsDevice,new DataPointer(Vertices.,));
            //vertexBuffer.SetData(Game.GraphicsContext.CommandList, Vertices);
            Buffer<MyVertexDefiniton> vertexBuffer = Buffer.Vertex.New(GraphicsDevice, Vertices, GraphicsResourceUsage.Default);//Stride.Graphics.Buffer.Structured.New(GraphicsDevice, VertexData);
            //vertexBuffer.ToStaging();
            Buffer<int> indexBuffer = Buffer.Index.New(GraphicsDevice, Indices);
            Mesh mesh = new Mesh()
            {
                Draw = new MeshDraw()
                {
                    DrawCount = Indices.Length,
                    IndexBuffer = new IndexBufferBinding(indexBuffer, true, indexBuffer.ElementCount, 0),
                    VertexBuffers =
                     [
                        new(vertexBuffer, MyVertexDefiniton.Layout,vertexBuffer.ElementCount)
                     ],
                    PrimitiveType = PrimitiveType.TriangleList,
                },
            };
            //mesh.Parameters.Set(myVertexShaderKeys.myTex, Content.Load<Texture>("Uv_Map"));
            //mesh.Parameters.Set(myVertexShaderKeys.verts,vertexBuffer);
            return mesh;
        }
        private Material NewMaterial()
        {
            //  delegate cmd => { Content.Load<Texture>("Uv_Map"); };
            //  ComputeTextureColor computeTextureColor = new(Content.Load<Texture>("Uv_Map"));
            // ComputeTextureColor computeTextureColor = new ComputeTextureColor(Content.Load<Texture>("Uv_Map"));
            // var computeTexure = new ComputeColorParameterTexture();
            //  var uvSematic = new ComputeColorStringParameter() { Value = "TexCoord" };
            //computeTexure.Texture.Texture = computeTextureColor.Texture;//Content.Load<Texture>("Uv_Map");
            // computeTexure.Texture.Texture = Texture.New2D(GraphicsDevice, 64, 64, Content.Load<Texture>("Uv_Map").Format);//Content.Load<Texture>("Uv_Map");
            // computeTexure.Texture.Texture.SetData(Game.GraphicsContext.CommandList, Content.Load<Texture>("Uv_Map").GetDataAsImage(Game.GraphicsContext.CommandList).ToDataBox());// = Content.Load<Texture>("Uv_Map");
            MaterialDescriptor materialDescriptor = new MaterialDescriptor()
            {
                Attributes = new MaterialAttributes
                {
                    Diffuse = new MaterialDiffuseMapFeature()
                    {
                        DiffuseMap = new ComputeShaderClassColor()
                        {
                            MixinReference = "myVertexShader",
                            //Generics =
                            //{
                            // //   {"mySampler",new ComputeColorParameterSampler() } //{ Filtering = TextureFilter.Linear} }
                            ////  {"myTex", computeTexure },
                            ////  {"TStream", uvSematic}
                            // },
                            //Members = new()
                            //{
                            //   { myVertexShaderKeys.myTex,Texture },
                            //   { myVertexShaderKeys.myDrawColor, Color.HotPink }
                            //}
                        }
                    },
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                }
            };
            Material material = Material.New(GraphicsDevice, materialDescriptor);
            material.Passes[0].Parameters.Set(myVertexShaderKeys.myTex, Texture);
            return material;
        }
        private static readonly int[] Indices = LF_TrianglesDatabase.LoadTriTable_Convex()[byte.MaxValue];
        private static MyVertexDefiniton[] Vertices => GenVerts();
        private static MyVertexDefiniton[] GenVerts()
        {
            BlockStruct wholeBlock = new BlockStruct(0, byte.MaxValue, blockID);
            MyVertexDefiniton[] verts = new MyVertexDefiniton[wholeBlock.points.Length];
            for (int i = 0; i < verts.Length; i++)
            {
                Vector2 uv = new(LF_PointGenerics.GenericBlockPoints[i].X + LF_PointGenerics.GenericBlockPoints[i].Z, LF_PointGenerics.GenericBlockPoints[i].Y);
                verts[i] = new MyVertexDefiniton((Vector3)LF_PointGenerics.GenericBlockPoints[i], new(uv, i % Block_IDs_Index.texture_IDs.Length), (Vector3)LF_PointGenerics.GenericBlockPoints[i]);
            }
            return verts;
        }
    };
}
