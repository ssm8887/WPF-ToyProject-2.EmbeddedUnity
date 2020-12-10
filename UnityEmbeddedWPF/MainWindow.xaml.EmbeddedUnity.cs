using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace UnityEmbeddedWPF
{
    public partial class MainWindow
    {
        [DllImport("User32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private Process process;
        private IntPtr unityHWND = IntPtr.Zero;

        private const int WM_ACTIVATE = 0x0006;
        private readonly IntPtr WA_ACTIVE = new IntPtr(1);
        private readonly IntPtr WA_INACTIVE = new IntPtr(0);

        private void UnityControl_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    ProcessStartInfo info = new ProcessStartInfo("ToyProject201127.exe");
            //    info.UseShellExecute = true;
            //    info.WindowStyle = ProcessWindowStyle.Minimized;
            //    process = System.Diagnostics.Process.Start(info);
            //    process.WaitForInputIdle();
            //    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, appIdleEvent, this, null);
            //    // Application Idle
            //    WindowInteropHelper helper = new WindowInteropHelper(Window.GetWindow(this));
            //    IntPtr ptr = helper.Handle;
            //    SetParent(app.MainWindowHandle, helper.Handle);
            //    SetWindowLong(new HandleRef(this, app.MainWindowHandle), GWL_STYLE, WS_VISIBLE);
            //    MoveWindow(app.MainWindowHandle, (int)control.Margin.Left, (int)control.Margin.Top, (int)control.Width, (int)control.Height, true);
            //}
            //catch
            //{

            //}

            try
            {
                process = new Process();
                process.StartInfo.FileName = "ToyProject201127.exe";
                process.StartInfo.Arguments = "-parentHWND " + this.UnityPanel.Handle.ToInt32() + " " + Environment.CommandLine;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                process.WaitForInputIdle();

                EnumChildWindows(this.UnityPanel.Handle, WindowEnum, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + ".");

            }
        }

        private void ActivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }

        private void DeactivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_INACTIVE, IntPtr.Zero);
        }

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHWND = hwnd;
            ActivateUnityWindow();
            return 0;
        }

        private void Form1_Closed(object sender, EventArgs e)
        {
            try
            {
                process.CloseMainWindow();

                Thread.Sleep(1000);
                while (process.HasExited == false)
                    process.Kill();
            }
            catch (Exception)
            {

            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            ActivateUnityWindow();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            DeactivateUnityWindow();
        }
    }
}
