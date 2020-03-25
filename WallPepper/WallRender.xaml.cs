using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Timers;

namespace WallPepper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WallRender : Window
	{
		private static System.Timers.Timer aTimer;
		static IntPtr otherWindow = IntPtr.Zero, thisWindow = IntPtr.Zero;
		Process proc;

		public WallRender()
        {
            InitializeComponent();
		}
		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//This window's handle
			thisWindow = new WindowInteropHelper(this).Handle;

			//Open another program for embedding
			// TODO: Iconize the program into task bar, being able to select programs.
			proc = new Process();
			proc.StartInfo = new ProcessStartInfo("C:\\Users\\andre\\AppData\\Local\\osu!\\osu!.exe");
			proc.Start();

			aTimer = new Timer(100);
			aTimer.Elapsed += timedEvent;
			aTimer.AutoReset = true;
		}

		private static void timedEvent(Object source, ElapsedEventArgs e)
		{
			//WinHelper.SetForegroundWindow(otherWindow);
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// Unlikely to happen
			ArrangeWindows();
		}

		private void ArrangeWindows()
		{
			// Appropriate locationing
			Point topLeft = childPlaceholder.TransformToAncestor(this).Transform(new Point(0, 0));
			Point bottomRight = childPlaceholder.TransformToAncestor(this).Transform(new Point(childPlaceholder.ActualWidth, childPlaceholder.ActualHeight));
			WinHelper.MoveWindow(otherWindow, (int)topLeft.X, (int)topLeft.Y, (int)bottomRight.X - (int)topLeft.X, (int)bottomRight.Y - (int)topLeft.Y, true);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Hooker.Visibility = Visibility.Collapsed;
			otherWindow = proc.MainWindowHandle;
			//This bit removes the border of otherWindow and sets thisWindow as parent
			//I actually don't know what flags should be set, but simply setting the WS_VISIBLE flag seems to make window work, however borderless.
			//WinHelper.SetWindowLong(otherWindow, WinHelper.GWL_STYLE, WinHelper.winStyle.WS_VISIBLE);
			WinHelper.SetParent(otherWindow, thisWindow);
			IntPtr handle = IntPtr.Zero;
			WallpaperWorker.getWorkerHandle(out handle);
			WinHelper.SetParent(thisWindow, handle);
			Application.Current.MainWindow.WindowState = WindowState.Maximized;
			ArrangeWindows();
			WallpaperWorker.registerMouseHandler(WinHelper.GetProcessId(handle));
			aTimer.Enabled = true;
		}
	}
}
