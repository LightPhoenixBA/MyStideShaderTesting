using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Extensions;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;

namespace ShaderTest
{
    public class MaterialStride : StartupScript
    {
        private Material material;
        private ModelComponent modelComponent;


        public override void Start()
        {
            Entity.Add(modelComponent = SetModel());//Entity.Get<ModelComponent>();
            //modelComponent.Model.Materials[0].Material.Passes[0].Parameters.Set(ComputeColorParameterKeys.ColorParameter, new ComputeColorParameterFloat4() { Value = Color.DarkGoldenrod.ToVector4() };// (ComputeColorParameterFloat4)( Color.DarkGoldenrod.ToVector4()));
            modelComponent.Model.Materials[0].Material.Passes[0].Parameters.Set(ComputeColorParameterKeys.ColorParameter, Color.DarkGoldenrod.ToColor4());// (ComputeColorParameterFloat4)( Color.DarkGoldenrod.ToVector4()));
            modelComponent.Materials[0].Passes[0].Parameters.Set(ComputeColorParameterKeys.ColorParameter, Color.DarkGoldenrod.ToColor4());
            modelComponent.Materials.SafeGet(0).Passes[0].Parameters.Set(ComputeColorParameterKeys.ColorParameter, Color.DarkGoldenrod.ToColor4());
        }
        private Material SetMaterial()
        {
            MaterialDescriptor materialDescriptor = new MaterialDescriptor()
            {
                Attributes = new MaterialAttributes
                {
                    Diffuse = new MaterialDiffuseMapFeature()
                    {
                        DiffuseMap = new ComputeShaderClassColor()
                        {
                            MixinReference = "ComputeColorParameter",
                            Members = new()
                            {
                                {ComputeColorParameterKeys.ColorParameter , Color.PaleGreen.ToColor4()}
                            },
                        }
                    },
                    DiffuseModel = new MaterialDiffuseLambertModelFeature()
                }
            };
            material = Material.New(GraphicsDevice, materialDescriptor);

            return material;
            //material.Passes[0].Parameters.Set(MaterialKeys.DiffuseValue, Color.PowderBlue.ToColor4());//ColorParameter
            // var layout = GenerateParameterLayout();
            //foreach (var item in material.Passes)
            //{
            //    //item.Parameters.UpdateLayout(layout );
            //    item.Parameters.Layout.LayoutParameterKeyInfos.Add(new ParameterKeyInfo(ComputeColorParameterKeys.ColorParameter, 0));
            //    item.Parameters.Set(ComputeColorParameterKeys.ColorParameter, Color.Orange.ToColor4());
            //}
        }
        ////private MaterialPass GenerateMaterialPass()
        ////{
        ////    ParameterCollection parameters = new ParameterCollection();
        ////    parameters.Layout.BufferSize = sizeof(float) * 3;
        ////    parameters.ParameterKeyInfos.Add(new ParameterKeyInfo(ComputeColorParameterKeys.ColorParameter, 0));
        ////    parameters.Set(ComputeColorParameterKeys.ColorParameter, Color.DarkGoldenrod.ToColor4());
        ////    parameters.UpdateLayout(parameters.Layout);
        ////    MaterialPass materialPass = new MaterialPass()
        ////    {
        ////        PassIndex = 0,
        ////        Parameters = new()
        ////        {
        ////            // { ComputeColorParameterKeys.ColorParameter, Color.Orange.ToColor4() 
        ////        }
        ////    };
        ////    materialPass.Parameters = parameters;
        ////    return materialPass;
        ////}

        //private ParameterCollectionLayout GenerateParameterLayout()
        //{
        //    ParameterCollectionLayout layout = new ParameterCollectionLayout()
        //    {
        //        BufferSize = sizeof(float) * 3,
        //        //LayoutParameterKeyInfos = new()
        //        //{
        //        //    new ParameterKeyInfo(ComputeColorParameterKeys.ColorParameter,0)
        //        //}
        //    };
        //    layout.LayoutParameterKeyInfos.Add(new ParameterKeyInfo(ComputeColorParameterKeys.ColorParameter, 0));
        //    return layout;
        //}
        private ModelComponent SetModel()
        {
            modelComponent = new ModelComponent()
            {
                Model = new()
                {
                    Meshes =
                    {
                        new Mesh()
                        {
                            Draw = GeometricPrimitive.Cone.New(GraphicsDevice).ToMeshDraw(),
                            MaterialIndex = 0
                        }
                    },
                    Materials =
                    {
                        { new MaterialInstance(SetMaterial())}
                    }
                }
            };
            //modelComponent.RenderGroup = RenderGroup.Group1;
            modelComponent.Materials.Add(0, material);
            return modelComponent;
        }
    }

}
