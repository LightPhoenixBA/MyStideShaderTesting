using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Extensions;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;


namespace ShaderTest
{
    public class IdealEntityController : StartupScript
    {
        private Material myMaterial => GenerateMaterial();// { get => GenerateMaterial(); private set => myMaterial = value; }
        //public Model MyModel => GenerateModel();
        public Model MyModel;
        private ModelComponent modelComponent;
       // public ComputeShaderClassColor myShader => GenerateShader();

        // private MaterialInstance materialInstance => GenerateMaterialnstance();
        public Color3 myControllerColor = new(0.2f, 0.8f, 0.1f);
        //  public Color4 myControllerColor4 = new(0f, 1.0f, 0.2f);
        public override void Start()
        {
            if (myControllerColor == default) myControllerColor = Color.ForestGreen.ToColor3();
            MyModel.Materials.Clear();
            MyModel.Materials.Add(myMaterial);
            modelComponent = new(MyModel);
            //modelComponent.Enabled = true;

            //modelComponent.Materials.Add(0, myMaterial);
            //modelComponent.Model.Materials.Add(myMaterial);
            //modelComponent.Materials[0].Passes[0].Parameters.Set(IdealShaderKeys.myColor, myControllerColor);
            //// modelComponent.RenderGroup = RenderGroup.Group1;
            //foreach (var mat in MyModel.Materials)
            //{
            //    foreach (var pass in mat.Material.Passes)
            //    {
            //        pass.Parameters.Set(IdealShaderKeys.myColor, myControllerColor);

            //    }
            //    //if (item.Parameters.ContainsKey(IdealShaderKeys.myColor))
            //    //{
            //    //    Log.Warning($"found key at {item.ToString()}");
            //    //}
            //}
            //// modelComponent.Model.Materials[0].Material.Passes[0].Parameters.Set(IdealShaderKeys.myColor, new Color3(0.0f, 1.0f, 0.4f));
            //modelComponent.Materials.Add(0, materialInstance.Material);
            //modelComponent.Materials.Clear();
            //modelComponent.GetMaterial(0).Descriptor.Attributes.Diffuse = new MaterialDiffuseMapFeature(new ComputeShaderClassColor() { MixinReference = "IdealShader" });//(ComputeColor)new ComputeShaderClassColor() { MixinReference = "IdealShader"};
            // modelComponent.Materials[0].Material.Passes[0].Parameters.Set(IdealShaderKeys.myColor, new Color3(0f, 1f, 0.5f));
            Entity.Add(modelComponent);
            // Model.Materials.Clear();
            // Model.Materials.Add(NewMaterial());

        }
        private Model GenerateModel()
        {
            return new Model()
            {
                Meshes = { new Mesh()
                    {
                    Draw = GeometricPrimitive.Cylinder.New(GraphicsDevice).ToMeshDraw(),
                    MaterialIndex = 0
                    }
                },
                Materials = { GenerateMaterial() },
            };
        }
        private ComputeShaderClassColor GenerateShader()
        {
            return new ComputeShaderClassColor()
            {
                MixinReference = "IdealShader",
                Generics =
                        {
                             {
                                "myFallbackColor",  new ComputeColorParameterFloat3() {Value = Color.DarkRed.ToVector3() }
                             }
                        },
                Members = new()
                        {
                            { IdealShaderKeys.myColor,myControllerColor }
                        }
            };
        }
        private Material GenerateMaterial()
        {
            //var urr = new MaterialComputeColorKeys(MaterialKeys.DiffuseMap, MaterialKeys.DiffuseValue, Color.White);
            MaterialDescriptor materialDescriptor = new MaterialDescriptor()
            {
                Attributes = new MaterialAttributes
                {
                    Diffuse = new MaterialDiffuseMapFeature(GenerateShader()),
                    DiffuseModel = new MaterialDiffuseLambertModelFeature()
                },
            };

            Material material = Material.New(GraphicsDevice, materialDescriptor);
            //foreach (var pass in material.Passes)
            //{
            //    pass.Parameters.Set(IdealShaderKeys.myColor, myControllerColor);
            //    pass.Parameters.Set(MaterialKeys.GenericValueColor3, myControllerColor);
            //}
          //  material.Passes[0].Parameters.Set(MaterialKeys.DiffuseValue, myControllerColor.ToColor4() * 255f);
            material.Passes[0].Parameters.Set(IdealShaderKeys.myColor, myControllerColor);
            //material.Passes[0].Parameters.Set(MaterialKeys.GenericValueColor3, myControllerColor);
            //material.Passes[0].Parameters.Set(MaterialKeys.DiffuseValue, ref myControllerColor4);
            //Game.GraphicsContext.DrawTexture();
            //var colorVertexStream = new ComputeVertexStreamColor { Stream = new ColorVertexStreamDefinition() };
            // var computeColor = new ComputeBinaryColor(new ComputeColor(myControllerColor4), colorVertexStream, BinaryOperator.Color);


            return material;
        }
        //private MaterialInstance GenerateMaterialnstance()
        //{
        //    return new MaterialInstance
        //    {
        //        Material = Material.New(GraphicsDevice,
        //        new MaterialDescriptor()
        //        {
        //            Attributes = new MaterialAttributes
        //            {
        //                DiffuseModel = new MaterialDiffuseLambertModelFeature(),
        //                Diffuse = new MaterialDiffuseMapFeature(GenerateShader())
        //            }
        //        })
        //    };
        //}
    }
}
