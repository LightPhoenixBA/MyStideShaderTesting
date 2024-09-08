//using LegionForge.Globals;
using Stride.Engine;
using Stride.Graphics;
using System;
using System.Reflection;
using System.Windows;

namespace ShaderTest
{
    class PathTest : StartupScript
    {
        public PathTest()
        {
        }

        public override void Start()
        {
            throw new NotImplementedException();
            //Log.Info($"Starting {this}");
            //Log.Warning(Assembly.GetExecutingAssembly().Location);
            //Log.Warning(AppDomain.CurrentDomain.RelativeSearchPath);
            //var TexRef = Block_IDs_Index.texture_IDs;
            ////Image[] images = Image.New2D();
            //foreach (var item in TexRef)
            //{
            //Log.Warning(item.GetRootPath());
            //}
        }
    }

}
