using System;
using System.Drawing;
namespace DynLights
{
	public class Light
	{
		public Point Position { get; set; }
		public Color Colour { get; private set; }
		public int Radius { get; private set; }
		public int Depth { get; private set; }
        int SourceRaidus = 5;
		
		public Light ()
		{
            Colour = Color.Green;
		}
	}
}

