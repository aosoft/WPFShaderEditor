using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace WPFShaderEditor
{
	class CustomShaderEffect : ShaderEffect
	{
		public CustomShaderEffect()
		{
			PixelShader = new PixelShader();
			UpdateShaderValue(InputProperty);
		}

		public Brush Input
		{
			get { return (Brush)GetValue(InputProperty); }
			set { SetValue(InputProperty, value); }
		}

		public static readonly DependencyProperty InputProperty =
			ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(CustomShaderEffect), 0);


		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Width.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty WidthProperty =
			DependencyProperty.Register("Width",
				typeof(double),
				typeof(CustomShaderEffect),
				new PropertyMetadata(0.0, PixelShaderConstantCallback(0)));

		public double Height
		{
			get { return (double)GetValue(HeightProperty); }
			set { SetValue(HeightProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Height.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HeightProperty =
			DependencyProperty.Register(
				"Height",
				typeof(double),
				typeof(CustomShaderEffect),
				new PropertyMetadata(0.0, PixelShaderConstantCallback(1)));


		public double Time
		{
			get { return (double)GetValue(TimeProperty); }
			set { SetValue(TimeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TimeProperty =
			DependencyProperty.Register(
				"Time",
				typeof(double),
				typeof(CustomShaderEffect),
				new PropertyMetadata(0.0, PixelShaderConstantCallback(2)));

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
	}
}
