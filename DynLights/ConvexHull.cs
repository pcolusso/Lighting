using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DynLights
{
	public class ConvexHull
	{
		List<Vector2> Points = new List<Vector2>();
		Point pos;
		Color colour;
		float depth;
		
		public ConvexHull(float depth, Vector2[] points, Color color, Point pos)
		{
			this.depth = depth;
            colour = Color.Purple;
			this.pos = pos;
			Points.AddRange(points);
		}
		
		public void DrawShadowGeometry(Light l)
		{
			
		}

        public void Render()
        {
            GL.Color4(colour);
            GL.PushMatrix();
            GL.Begin(BeginMode.Polygon);
            //GL.Translate(pos.X, pos.Y, depth);
            foreach (Vector2 vertex in Points)
                GL.Vertex2(vertex);
            GL.End();
            GL.PopMatrix();
        }
	}
}

