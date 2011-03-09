using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace DynLights
{
	public class FramebufferManager
	{
        public int fboHandle, fboTextureHandle, depthHandle; //Swithc back to priv
		int width, height;
		bool enabled;
		
        //WARNING!!!! DO NOT INITIALISE UNTILL THE OGL STATE IS LOADED!!
		public FramebufferManager(int width, int height)
		{
			width = 800;
			height = 600;
			//Init Texture
            try
            {
				GL.GenTextures(1, out fboTextureHandle);
            	
				
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Unable to access OpenGL. Have you initialised OGL before attempting to create this Framebuffer?");
            }
			
			
			GL.BindTexture(TextureTarget.Texture2D, fboTextureHandle);
        	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest); //Enable texture filtering
        	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        	GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp); //Do not wrap textures
       		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
        	GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
       		GL.BindTexture(TextureTarget.Texture2D, 0); //Unbind texturebuffer, to allow further texture actions.
			
			GL.GenRenderbuffers(1, out depthHandle);
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, width, height);
			
			GL.GenFramebuffers(1, out fboHandle); //Generate FBO and get pointer.
        	GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboHandle); //Load the FBO
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthHandle);
        	GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, fboTextureHandle, 0); //Attach colour texture to FBO
			GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
			if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
				throw new Exception("FBO failed to init, error: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		}
		
		public void Enable()
		{
            if (enabled)
            {
                return;
            }
            else
			{
               	GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboHandle);
                enabled = true;
				
				GL.PushAttrib(AttribMask.ViewportBit);
				GL.Viewport(0,0,800,600);
				
				GL.ClearColor(1,1,1,0);
				GL.ClearDepth(1.1);
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			}	
		}
		
		public void Disable()
		{
            if (!enabled)
            {
                return;
            }        
            else
			{
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                GL.PopAttrib();
				
				enabled = false;
			}
		}
		
		public void RenderImmediate()
		{
            if (!enabled)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.Blend);
				GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.BindTexture(TextureTarget.Texture2D, fboTextureHandle);
				//GL.Scale(1,-1,1);

                GL.Color4(Color.White);
                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0, 0);  GL.Vertex2(0, 0);
                GL.TexCoord2(0, 1);  GL.Vertex2(0, 600);
                GL.TexCoord2(1, 1);  GL.Vertex2(800, 600);
                GL.TexCoord2(1, 0);  GL.Vertex2(800, 0);
                GL.End();
                
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.Disable(EnableCap.Texture2D);
				GL.Disable(EnableCap.Blend);
            }
            else
            {
                throw (new Exception("Attempted to render FBO while rendering to it!"));
            }
		}
		
		public void ClearToAlpha()
		{
			
		}
	}
}

