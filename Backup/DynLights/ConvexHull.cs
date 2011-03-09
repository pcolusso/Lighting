using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;

namespace DynLights
{
	public class ConvexHull
	{
		List<Vector2> Points = new List<Vector2>();
		Point pos;
		Color colour;
		float depth;
		
		public ConvexHull(float depth, List<Vector2> points, Color color, Point pos)
		{
			this.depth = depth;
			this.colour = colour;
			this.pos = pos;
			Points.AddRange(points);
		}
		
		public void DrawShadowGeometry(Light l)
		{
			foreach(Vector2 p in Points)
			{
				var CurrentPoint = p;
				
				var nv = new Vector2(CurrentPoint.X - Light.Position.X, CurrentPoint.Y - Light.Position.Y);
				var lv = new Vector2(CurrentPoint.Y - Light.Position.Y, CurrentPoint.X - Light.Position.X);
				if ( (nv.X * -1 * lv.X) + (lv.Y * nv.Y) > 0 )
				{
					
				}
				
				var LastPoint = p;
			}
		}
	}
}

