using System;
using System.Drawing;
namespace DynLights
{
	public class Light
	{
		public Point Position { get; private set; }
		public Color Colour { get; private set; }
		public int Radius { get; private set; }
		public int Depth { get; private set; }
		
		public Light ()
		{
		}
	}
}

