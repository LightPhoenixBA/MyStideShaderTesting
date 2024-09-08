using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Extensions;
using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;
using System;
using System.Collections.Generic;
namespace ShaderTest
{
    public class MaterialNew : StartupScript
    {
        private static Model SharedModel;
        private ModelComponent thisModel;
        private Material material;
        private Texture texture;
        public Texture Texture { get => texture; }
        public Color3 myColor = Color.Green.ToColor3();
        public static readonly Color3 myFallbackColor = Color.YellowGreen.ToColor3();
        //public static readonly Dictionary<ParameterKey, object> parameters = new()
        //{
        //        { MyDiffuseKeys.myColor, Color.HotPink.ToVector3() },
        //        { MyDiffuseKeys.myTex,texture }
        //};

        public MaterialNew()
        {
        }

        public override void Start()
        {
            // GraphicsDevice hey = Program.Game.GraphicsDevice;
            if (texture == null) texture = Content.Load<Texture>("Uv_Map");
            //Game.GraphicsContext.DrawTexture(texture, BlendStateDescription.Default);
            //Game.GraphicsDevice.Presenter.BeginDraw(Game.GraphicsContext.CommandList);
            //texture = Texture.New2D(GraphicsDevice, texture.Width, texture.Height, texture.Format, TextureFlags.ShaderResource, 2, GraphicsResourceUsage.Default, TextureOptions.Shared);
            //texture.SetData();

            if (SharedModel == null)
            {
                SharedModel = new()
                {
                  new Mesh()
                    {
                    Draw = GeometricPrimitive.Capsule.New(GraphicsDevice).ToMeshDraw(),
                    MaterialIndex = 0
                    }
                };
                SharedModel.Materials.Add(material = NewMaterial());
            }
            thisModel = new(SharedModel) { RenderGroup = RenderGroup.Group1 };
            thisModel.Materials.Add(0, NewMaterial());
            Entity.Add(thisModel);
            // MaterialPass renderPass = SharedModel.Materials.Where(x => x.Material.Descriptor.Attributes.Diffuse == true);//.First(x.Parameters.ContainsKey(MyDiffuseKeys.myColor)) )  //.Passes.First(x => x.Parameters.ContainsKey(MyDiffuseKeys.myColor));
            //var renderPass = SharedModel.Materials[0].Material.Passes.First(x => x.Material.Descriptor.Attributes.Diffuse != null);//.First(x.Parameters.ContainsKey(MyDiffuseKeys.myColor)) )  //.Passes.First(x => x.Parameters.ContainsKey(MyDiffuseKeys.myColor));

            // model = Entity.Get<ModelComponent>();
            // texture = LF_GenerateTexture2dArray.GenerateImageToTexture2DArray(GraphicsDevice, Game.Services.GetService<GraphicsContext>(), Content, Log);
            //texture = LF_GenerateTexture2dArray.GenerateImageTexture2DArray(GraphicsDevice, Game.Services.GetService<GraphicsContext>(), Content, Log);
            //Log.Info(model.Model.Materials[0].Material.Passes.Count.ToString());
            // material = model.Materials.First(x => x == MyDiffuseKeys.tex);
            //MaterialInstance materialInstance = model.Model.Materials.First(x => x.Material.Passes != null);//.GetMaterial(0);
            // materialInstance.Parameters.Set(MyDiffuseKeys.tex, texture);
            //MaterialPass renderPass = material.Passes.First();
            // MaterialGeneratorContext generatorContext = new MaterialGeneratorContext(thisModel.Materials[0], GraphicsDevice);


            // MaterialDescriptor materialDescriptor = new MaterialDescriptor();
            //renderPass.Material.Descriptor = materialDescriptor;
            //renderPass.Material.Descriptor.Visit(generatorContext);
            //generatorContext.Visit();
            //materialDescriptor.Attributes.Diffuse = (new MaterialGeneratorContext(model.Materials[0], GraphicsDevice));// = new MaterialDiffuseLambertModelFeature();
            //   material.Descriptor = new MaterialDescriptor();
            // material.Descriptor.Attributes.Diffuse = Color.White;
            //material.Descriptor.Attributes.Diffuse.Visit(new MaterialGeneratorContext(material,GraphicsDevice));

            //     Log.Info(renderPass.Parameters.ObjectValues.ToString());
            // Log.Warning(renderPass.ToString());
        }
        private Material NewMaterial()
        {
            MaterialDescriptor materialDescriptor = new MaterialDescriptor()
            {
                Attributes = new MaterialAttributes
                {
                    //Diffuse = new MaterialDiffuseMapFeature(new MyDiffuseClass()),
                    Diffuse = new MaterialDiffuseMapFeature(GenerateShader()),
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                }
            };
            Material material = Material.New(GraphicsDevice, materialDescriptor);
            //material.Passes[0].Parameters.Set(MyDiffuseKeys.myTex, texture);
            //material.Passes[0].Parameters.Set(MyDiffuseKeys.myColor, Color.Lime.ToColor3());
            //  material.Passes[0].Parameters.UpdateLayout(material.Passes[0].Parameters.Layout);
            // material.Passes[0].Parameters.UpdateLayout(SharedModel.Materials[0].Material.Passes[0].Parameters.Layout);
            //   Services.GetService<GraphicsContext>().DrawTexture();

            return material;
        }
        // private ComputeEffectShader GenerateShader() =>  new(RenderContext.GetShared(Services)) { Name = "MyDiffuse" };
        private ComputeShaderClassColor GenerateShader()
        {
            if (texture == null) throw new ArgumentNullException($"{this} does not have a texture set");

            return new ComputeShaderClassColor()
            {
                MixinReference = "MyDiffuse",
                Members = new Dictionary<ParameterKey, object>()
                {
                    { MyDiffuseKeys.myColor, Color.ForestGreen.ToColor3() },
                    { MyDiffuseKeys.myTex,texture },
                    { MyDiffuseKeys.myFallbackColor, new Color3(0.7f,0.33f,0.01f) },
                    { ComputeColorParameterKeys.ColorParameter, new Color3(0.9f,0.1f,0.01f) }
                },
                // Members = parameters
            };
        }
        private EffectInstance GenerateEffect()
        {
            // Create a EffectInstance and use it to set up the pipeline
            // Effect effect = new Effect(GraphicsDevice,EffectSystem.LoadEffect("MyDiffuse").WaitForResult().Bytecode);
            var effectInstance = new EffectInstance(EffectSystem.LoadEffect("MyDiffuse").WaitForResult());

            //pipelineStateDescription.EffectBytecode = effectInstance.Effect.Bytecode;
            //pipelineStateDescription.RootSignature = effectInstance.RootSignature;
            effectInstance.Parameters.Set(MyDiffuseKeys.myColor,new Color3(0.2f,0.4f,0.7f));
            //foreach (var item in parameters)
            //{
            //    effectInstance.Parameters.SetObject(item.Key, item.Value);
            //}

            // Update constant buffers and bind resources
            effectInstance.UpdateEffect(GraphicsDevice);
            effectInstance.Apply(Game.GraphicsContext);
            return effectInstance;
        }
        private ComputeShaderClassColor GenerateShaderWithGenerics()
        {
            var computeTexure = new ComputeColorParameterTexture();
            computeTexure.Texture.Texture = texture;

            return new ComputeShaderClassColor()
            {
                MixinReference = "MyDiffuse",
                Generics = new ComputeColorParameters()
                {
                    {"myTex", computeTexure },
                    {"myColor",new ComputeColorParameterFloat3() { Value = Color.DarkSeaGreen.ToVector3() } }
                    //{"mySampler",SamplerState.New(GraphicsDevice,SamplerStateDescription.Default) }
                }
            };
        }
        //private Texture GenTexture()
        //{
        //    Texture tex = Texture.New2D(GraphicsDevice,texture.Width,texture.Height,PixelFormat.BC1_UNorm_SRgb,TextureFlags.ShaderResource,1);
        //    //tex.SetData(Game.GraphicsContext.CommandList,texture.GetDataAsImage(Game.GraphicsContext.CommandList).ToDataBox());
        //    return tex;
        //}
    }
}
