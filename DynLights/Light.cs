using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
namespace DynLights
{
	public class Light
	{
		public Point Position { get; set; }
		public Color Colour { get; private set; }
		public int Radius { get; private set; }
		public float Depth { get; private set; }
        int SourceRaidus = 5;
		
		public Light(int x, int y, int radius, float depth)
		{
            Colour = Color.Green;
			Position = new Point(x, y);
			Radius = radius;
			Depth = depth;
		}
		
		public void Render()
		{
			
		}
		
		public void RenderAlpha(float intensity)
		{
			if (intensity < 0 || intensity > 1)
				throw new Exception("Light intensity too great, keep between 0-1");
			
			int NumberOfSubdivisions = 32;
			GL.Begin(BeginMode.TriangleFan);
			GL.Color4(0, 0, 0, intensity);
			GL.Vertex3(Position.X, Position.Y, Depth);
			GL.Color4(0, 0, 0, 0);
			
			for(float angle = 0.0f; angle <= Math.PI * 2; angle += (float) ((Math.PI / 2) / NumberOfSubdivisions))
			{
				GL.Vertex3(Radius * (float) Math.Cos(angle) + Position.X,
				           Radius * (float) Math.Sin(angle) + Position.Y,
				           Depth);
			}
			GL.Vertex3(Position.X + Radius, Position.Y, Depth);
			GL.End();
			
		}
	}
}

