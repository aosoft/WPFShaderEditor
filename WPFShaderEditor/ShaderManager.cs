using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive;
using Reactive.Bindings;
using SharpDX.D3DCompiler;

namespace WPFShaderEditor
{
	class ShaderManager
	{
		private ReactivePropertySlim<byte[]> _bytecode = new ReactivePropertySlim<byte[]>();
		private ReactivePropertySlim<string> _errorMessage = new ReactivePropertySlim<string>();

		static ShaderManager()
		{
			SharpDX.Configuration.ThrowOnShaderCompileError = false;
		}

		public ShaderManager()
		{
			Source = new ReactivePropertySlim<string>(@"
sampler2D implicitInput : register(s0);
float4 main(float2 uv : TEXCOORD) : COLOR
{
    return float4(uv.x, uv.y, 0.0, 1.0);
}
");
			Bytecode = _bytecode.ToReadOnlyReactivePropertySlim();
			ErrorMessage = _errorMessage.ToReadOnlyReactivePropertySlim();

			Source
				.Delay(TimeSpan.FromMilliseconds(500))
				.Select(value =>
				{
					if (!string.IsNullOrEmpty(value))
					{
						try
						{
							return ShaderBytecode.Compile(value, "main", "ps_3_0");
						}
						catch (Exception e)
						{
							return new CompilationResult(null, SharpDX.Result.Fail, e.StackTrace);
						}
					}
					return new CompilationResult(null, SharpDX.Result.Ok, string.Empty);
				})
				.ObserveOnDispatcher()
				.Subscribe(value =>
				{
					if (value != null)
					{
						_bytecode.Value = value.Bytecode?.Data;
						_errorMessage.Value = value.Message;
					}
					else
					{
						_bytecode.Value = null;
						_errorMessage.Value = string.Empty;
					}
				});
		}

		public ReactivePropertySlim<string> Source
		{
			get;
		}

		public ReadOnlyReactivePropertySlim<byte[]> Bytecode
		{
			get;
		}

		public ReadOnlyReactivePropertySlim<string> ErrorMessage
		{
			get;
		}
	}
}
