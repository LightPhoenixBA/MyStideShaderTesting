using Stride.Core.Mathematics;
using Stride.Graphics;
using System;
using System.Runtime.InteropServices;

namespace ShaderTest
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct MyVertexDefiniton : IVertex, IEquatable<MyVertexDefiniton>
    {
        public static readonly VertexDeclaration Layout = new
            (
             VertexElement.Position<Vector3>(),
             new VertexElement("MYTEXCOORD", PixelFormat.R32G32B32_Float),
            VertexElement.Normal<Vector3>()
             //VertexElement.TextureCoordinate<Vector3>()
            // new VertexElement("MyTexcoord", PixelFormat.R32G32B32_Float),//VertexElement.TextureCoordinate<Vector3>(),
            // new VertexElement("MyTexcoord", PixelFormat.R32_Float)

            );

        public Vector3 Position;
        public Vector3 TextureCoordinate;
        public Vector3 Normal;
        public MyVertexDefiniton(Vector3 position, Vector3 textureCoordinate, Vector3 normal)
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
            Normal = normal;
        }
        public bool Equals(MyVertexDefiniton other)
        {
            return Position.Equals(other.Position) && Normal.Equals(other.Normal) && TextureCoordinate.Equals(other.TextureCoordinate);
        }

        public void FlipWinding()
        {
            TextureCoordinate.X = (1.0f - TextureCoordinate.X);
        }

        public VertexDeclaration GetLayout()
        {
            return Layout;
        }
    }
}
