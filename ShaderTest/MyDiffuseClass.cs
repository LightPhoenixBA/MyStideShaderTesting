using Stride.Core.Mathematics;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;
using Stride.Shaders;

namespace ShaderTest
{
    class MyDiffuseClass : ComputeShaderClassColor , IMaterialDiffuseFeature//ComputeValueBase<Color4>, IComputeColor
    {

        public MyDiffuseClass()
        {
            MixinReference = "MyDiffuse";
           // Members = MaterialNew.parameters;
        }
        //public override ShaderSource GenerateShaderSource(ShaderGeneratorContext context, MaterialComputeColorKeys baseKeys)
        //{
        //    return new ShaderClassSource("MyDiffuse");
        //}
        //public static readonly ParameterCollectionLayout Layout = new() { LayoutParameterKeyInfos = new() {(MyDiffuseKeys.myColor,Color.Green.ToColor3()),(MyDiffuseKeys.myTex,MaterialNew.Texture) } };
        //public  ParameterCollection Layout()
        //{
        //    ParameterCollection collection = new();
        //    collection.Layout.LayoutParameterKeyInfos.Add(new(MyDiffuseKeys.myColor, 0));
        //    // collection.Layout.LayoutParameterKeyInfos.Add();
        //    collection.Set(MyDiffuseKeys.myColor, Color.HotPink.ToColor3());
        //    ////  collection.HasLayout
        //    return collection;
        //}
        public void Visit(MaterialGeneratorContext context)
        {
            throw new System.NotImplementedException();
        }

        public bool HasChanged { get; }
        public bool Enabled { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    }
}
