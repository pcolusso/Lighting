using System;
namespace DynLights
{
	public class Bootstrap
	{
		public static void Main()
		{
			using (View v = new View())
			{
				v.Run();
			}
		}
	}
}

