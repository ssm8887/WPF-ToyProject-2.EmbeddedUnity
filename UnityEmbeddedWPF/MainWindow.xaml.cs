using System.Windows;
using static TCPCommunication.CommandEnum;

namespace UnityEmbeddedWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += UnityControl_Load;
            Loaded += Server_Load;
            Activated += Form1_Activated;
            Deactivated += Form1_Deactivate;
            Closed += Form1_Closed;

            Dispatcher.UnhandledExceptionFilter += Dispatcher_UnhandledExceptionFilter;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            command_01.Click += Command_01_Click;
            command_02.Click += Command_02_Click;
            command_03.Click += Command_03_Click;
        }

        private void Dispatcher_UnhandledExceptionFilter(object sender, System.Windows.Threading.DispatcherUnhandledExceptionFilterEventArgs e)
        {
            e.RequestCatch = true;
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(e.Exception.ToString());
            e.Handled = true;
        }

        private void Command_01_Click(object sender, RoutedEventArgs e)
        {
            this.Write(CommandId.Command_01);
        }

        private void Command_02_Click(object sender, RoutedEventArgs e)
        {
            this.Write(CommandId.Command_02);
        }

        private void Command_03_Click(object sender, RoutedEventArgs e)
        {
            this.Write(CommandId.Command_03);
        }
    }
}
