using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace WPFShaderEditor
{
	class CustomShaderEffect : ShaderEffect
	{
		public CustomShaderEffect()
		{
			PixelShader = new PixelShader();
		}

		public byte[] Bytecode
		{
			get { return (byte[])GetValue(BytecodeProperty); }
			set { SetValue(BytecodeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Bytecode.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BytecodeProperty =
			DependencyProperty.Register(
				"Bytecode",
				typeof(byte[]),
				typeof(CustomShaderEffect),
				new PropertyMetadata(
					null,
					(d, e) =>
					{
						var t = d as CustomShaderEffect;
						if (t != null)
						{
							t.OnBytecodePropertyChanged(e);
						}
					}));


		private void OnBytecodePropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			var v = e.NewValue as byte[];
			if (v != null)
			{
				using (var ms = new MemoryStream(v))
				{
					PixelShader.SetStreamSource(ms);
				}
			}
		}

		/*
		private static Bytecode Compile(string code)
		{
			return Bytecode.Compile(code, "main", "ps3_0");
		}
		*/
	}
}
