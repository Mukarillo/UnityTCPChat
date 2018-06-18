using System;
namespace chatserver
{
	public class Color
	{
		public static Color ServerColor{ get { return new Color(1f, 1f, 1f, 1f); } }

		public float r, g, b, a;
              
		public Color(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
	}
}
