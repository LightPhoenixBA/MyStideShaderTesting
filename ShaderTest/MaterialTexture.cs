using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;

namespace ShaderTest
{
    public class MaterialTexture : StartupScript
    {
        private ModelComponent model => Entity.Get<ModelComponent>();
        private Texture texture;
        Texture defaultTexture => Content.Load<Texture>("Uv_Map");

        public override void Start()
        {
            texture = defaultTexture;
            texture = Texture.New2D(GraphicsDevice, defaultTexture.Width, defaultTexture.Height, defaultTexture.Format, TextureFlags.ShaderResource, 2);
            var toLoad = defaultTexture.GetDataAsImage(Game.GraphicsContext.CommandList);
            //texture.SetData(Game.GraphicsContext.CommandList, new DataPointer(toLoad.DataPointer, toLoad.TotalSizeInBytes), 0, 0);
            texture.SetData(Game.GraphicsContext.CommandList, new DataPointer(toLoad.DataPointer, toLoad.TotalSizeInBytes), 1, 0);
            SetNewMaterial();

            //model.Materials[0].Passes[0].Parameters.Set(MaterialKeys.DiffuseValue, Color.DarkRed);
            //model.Materials[0].Passes[0].Parameters.Set(MaterialKeys.DiffuseMap, texture);
        }
        private void SetNewMaterial()
        {
            ComputeColorParameterTexture computeTexure = new ComputeColorParameterTexture();
            computeTexure.Texture.Texture = defaultTexture;
            MaterialDescriptor materialDescriptor = new MaterialDescriptor()
            {
                Attributes = new MaterialAttributes
                {
                    Diffuse = new MaterialDiffuseMapFeature()
                    {
                        //    DiffuseMap = new ComputeTextureColor(texture,TextureCoordinate.Texcoord0,Vector2.One,Vector2.Zero)
                        DiffuseMap = new ComputeShaderClassColor()
                        {
                            MixinReference = "MyTextureShader",
                            //  MixinReference = "ComputeColorTexture",
                            //Generics = new()
                            //{
                            //    {"MyTex", computeTexure },
                            //    {"UVSematic", new ComputeColorStringParameter() { Value = "TexCoord0"}},
                            //  //  {TexturingKeys.Texture0, texture }
                            //},
                            Members = new()
                            {
                                // {MaterialKeys.DiffuseMap, computeTexure }
                                { MyTextureShaderKeys.myTex,texture}
                            }
                        }
                    },
                    DiffuseModel = new MaterialDiffuseLambertModelFeature()
                }
            };
            Material material = Material.New(GraphicsDevice, materialDescriptor);
            material.Passes[0].Parameters.Set(MyTextureShaderKeys.myTex, texture);
            model.Materials.Clear();
            model.Materials.Add(0, material);
        }
    }
}
