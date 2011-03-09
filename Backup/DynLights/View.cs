using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace DynLights
{
	public class View : GameWindow
	{
		
		public View ()
			: base (800, 600, new GraphicsMode(32,22,0,0))
		{
		}
		
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			GL.ClearColor(Color.CornflowerBlue);
		}
		
		protected override void OnResize (EventArgs e)
		{
			base.OnResize (e);
		}
		
		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);
		}
		
		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);
			//Clear screen
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.DepthMask(true);
			//Init camera matrix
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.MatrixMode(MatrixMode.Texture);
			GL.LoadIdentity();
			
			GL.Disable(EnableCap.CullFace);
			//Fill z-buffer
			
			//for every light
			SwapBuffers();
		}
		
	}
}

