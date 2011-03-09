using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace DynLights
{
	public class FramebufferManager
	{
		int fboHandle;
		int texHandle;
		Rectangle rect;
		bool enabled;
		
		public FramebufferManager(Rectangle size)
		{
			rect = size;
			//Init Texture
			texHandle = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, texHandle);
			//Texture params
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest); //Enable texture filtering
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp); //Do not wrap textures
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
			
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, rect.Width, rect.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
			GL.BindTexture(TextureTarget.Texture2D, 0);
			
			//Init FBO
			GL.GenFramebuffers(1, out fboHandle);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboHandle);
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texHandle, 0);
		}
		
		public void Enable()
		{
			if(!enabled)
			{
				GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboHandle);
				GL.PushAttrib(AttribMask.ViewportBit);
				GL.Viewport(rect);
				enabled = true;
			}	
		}
		
		public void Disable()
		{
			if(enabled)
			{
				GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
				GL.PopAttrib();
				enabled = false;
			}
		}
		
		public void RenderImmediate()
		{
			if(!enabled)
			{
				GL.Enable(EnableCap.Texture2D);
				GL.BindTexture(TextureTarget.Texture2D, texHandle);
				GL.Begin(BeginMode.Quads)
				
			}
		}
	}
}

