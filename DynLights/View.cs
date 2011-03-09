using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace DynLights
{
	public class View : GameWindow
	{
        List<ConvexHull> hulls = new List<ConvexHull>();
        List<Light> lights = new List<Light>();
        FramebufferManager fbo, testFbo;
        int fboHandle, fboTextureHandle;

        public View ()
			: base (800, 600, new GraphicsMode(32,22,0,0))
		{
			lights.Add(new Light(720,20,400,0.02f));
			lights.Add(new Light(20,20,400,0.01f));
            hulls.Add(new ConvexHull(0.0f, new Vector2[] { new Vector2(12,12), new Vector2(15,67), new Vector2(53, 35), new Vector2(22,34) }, Color.Violet, new Point(12,32)));
			hulls.Add(new ConvexHull(0.0f, new Vector2[] { new Vector2(34,22), new Vector2(55,67), new Vector2(20, 30)}, Color.Gold, new Point(500,500)));
		}
		
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			GL.Enable(EnableCap.Blend);
            fbo = new FramebufferManager(800,600);
			
			#region testfbo
			GL.GenTextures(1, out fboTextureHandle);
            GL.BindTexture(TextureTarget.Texture2D, fboTextureHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest); //Enable texture filtering
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp); //Do not wrap textures
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, 800, 600, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D, 0); //Unbind texturebuffer, to allow further texture actions.

            GL.GenFramebuffers(1, out fboHandle); //Generate FBO and get pointer.
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboHandle); //Load the FBO
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, fboTextureHandle, 0); //Attach colour texture to FBO
			
			GL.PushAttrib(AttribMask.ViewportBit);
			GL.Viewport(0,0,800,600);
			GL.ClearColor(Color.Blue);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			Matrix4 baseMatrix = Matrix4.CreateOrthographicOffCenter(0, 800, 600, 0, -1, 1);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref baseMatrix);
			GL.Color4(Color.Red);
			GL.Begin(BeginMode.Triangles);
			GL.Vertex2(20,20);
			GL.Vertex2(20,300);
			GL.Vertex2(300,300);
			GL.End();
			GL.PopAttrib();
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			#endregion testfbo
			
			/*
			testFbo = new FramebufferManager(new Rectangle(0,0,800,600));
			
			testFbo.Enable();
			GL.PushAttrib(AttribMask.ViewportBit);
			GL.Viewport(0,0,800,600);
			GL.ClearColor(Color.Blue);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			Matrix4 baseMatrix2 = Matrix4.CreateOrthographicOffCenter(0, 800, 600, 0, -1, 1);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref baseMatrix2);
			GL.Color4(Color.Red);
			GL.Begin(BeginMode.Triangles);
			GL.Vertex2(20,20);
			GL.Vertex2(20,300);
			GL.Vertex2(300,300);
			GL.End();
			GL.PopAttrib();
			testFbo.Disable();
			                                 
			*/
            Matrix4 mat = Matrix4.CreateOrthographicOffCenter(0, 800, 600, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref mat);
        }
		
		protected override void OnResize (EventArgs e)
		{
            GL.Viewport(ClientRectangle);
			base.OnResize (e);
		}
		
		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);
           	//lights[0].Position = new Point(Mouse.X, Mouse.Y);
			//if (Keyboard[Key.U])
				//lights.Add(new Light(Mouse.X, Mouse.Y, 100, 0));
			if (Keyboard[Key.Escape])
                Exit();
		}
		
		protected override void OnRenderFrame (FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.ClearColor(Color.Purple);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			Matrix4 baseMatrix = Matrix4.CreateOrthographicOffCenter(0, 800, 600, 0, -1, 1);
		
            fbo.Enable(); 
            {
				
				Matrix4 fboMatrix = Matrix4.CreateOrthographicOffCenter(0, 800, 0, 600, -1, 1);
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadMatrix(ref fboMatrix);
				
                GL.Enable(EnableCap.DepthTest);
				GL.DepthMask(true);
				foreach(ConvexHull h in hulls)
                    	h.Render();
				
                GL.DepthMask(false);
                GL.Disable(EnableCap.DepthTest);

                foreach (Light l in lights)
                {
                    GL.ColorMask(false, false, false, true);
					GL.ClearColor(1,1,1,0);
                    GL.Clear(ClearBufferMask.ColorBufferBit);
					
                    GL.Disable(EnableCap.Blend);
                    GL.Enable(EnableCap.DepthTest);
                    GL.ColorMask(false, false, false, true);
                    l.RenderAlpha(1.0f);

                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.DstAlpha, BlendingFactorDest.One);
                    GL.ColorMask(true, true, true, false);
					
					GL.Disable(EnableCap.DepthTest);
                }
            }
			
            fbo.Disable();
			GL.LoadMatrix(ref baseMatrix);
			
			fbo.RenderImmediate();
            
            SwapBuffers();
        }

        Light[] GetVisibleLights()
        {
            List<Light> LightList = new List<Light>();

            return LightList.ToArray();
        }
	}
}

