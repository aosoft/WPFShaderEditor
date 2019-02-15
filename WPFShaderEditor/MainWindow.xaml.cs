#define FILE_WATCHER

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpDX.D3DCompiler;

using IOPath = System.IO.Path;

namespace WPFShaderEditor
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{

		public MainWindow()
		{
			InitializeComponent();
			var sm = new ShaderManager();
			DataContext = sm;

#if FILE_WATCHER
			var dir = IOPath.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var fname = "test.hlsl";

			var fw = new FileSystemWatcher();

			Action<string> fl = (path) =>
			{
				if (File.Exists(path))
				{
					for (int i = 0; i < 10; i++)
					{
						try
						{
							using (var sr = new StreamReader(path))
							{
								var s = sr.ReadToEnd();
								Dispatcher.BeginInvoke((Action)(() =>
									{
										sm.Source.Value = s;
									}),
									null);
								return;
							}
						}
						catch
						{
						}
						System.Threading.Thread.Sleep(100);
					}
				}
			};
			fl(IOPath.Combine(dir, fname));

			fw.Path = dir;

			FileSystemEventHandler h = (s, e) =>
			{
				if (fname.Equals(e.Name))
				{
					fl(e.FullPath);
				}
			};
			fw.Changed += h;
			fw.EnableRaisingEvents = true;

			Closed += (s, e) =>
			{
				fw.Dispose();
			};
#endif 
		}
	}
}
