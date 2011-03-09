using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace DynLights
{
	public class ShaderManager
	{
		int v_shader, f_shader, g_shader, program;
		bool enabled;
		Dictionary<string, int> Uniforms = new Dictionary<string, int>();
		
		public ShaderManager(string VertexShader, string FragmentShader)
		{
			GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(v_shader, VertexShader);
			GL.CompileShader(v_shader);
			
			GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(f_shader, FragmentShader);
			GL.CompileShader(f_shader);
			
			program = GL.CreateProgram();
			GL.AttachShader(program, v_shader);
			GL.AttachShader(program, f_shader);
			
			Disable();
		}
		
		public void Enable()
		{
			if (!enabled) //Skip an OGL call if we can
				GL.UseProgram(program);
			enabled = true;
		}
		
		public void Disable()
		{
			if (enabled)
				GL.UseProgram(0);
			enabled = false;
		}
		
		public void AddUniform(string name)
		{
			int loc = GL.GetUniformLocation(program, name);
			Uniforms.Add(name, loc);
		}
		
		public void SetUniform1(string name, float val)
		{
			GL.Uniform1(Uniforms[name], val);
		}
	}
}

