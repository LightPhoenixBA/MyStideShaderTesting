//using LegionForge.Globals;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Materials;

namespace ShaderTest
{
    public class MaterialSwap : StartupScript
    {
        ModelComponent thisModel;
        public Material material;
        public Texture texture;
        public Color3 myColor = Color.Green.ToColor3();
        public override void Start()
        {
            if (texture == null) Log.Error("texture is not assigned on " + Entity.Name);
            thisModel = Entity.Get<ModelComponent>();
            material = thisModel.GetMaterial(0);
            if (material == null) Log.Error("material not found on " + Entity.Name);
       // var myDiffuse =   material.Passes[0].Parameters;
            foreach (var pass in material.Passes)
            {
                pass.Parameters.Set(MyDiffuseKeys.myColor, Color.Purple.ToColor3());
                // pass.Parameters.Set(MyDiffuseKeys.myTex, texture);
                Log.Info($"{pass.PassIndex} setting on {pass} ");
            }
            
        }
    }
}
