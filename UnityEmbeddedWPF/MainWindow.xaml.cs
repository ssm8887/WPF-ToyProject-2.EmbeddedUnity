﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Threading;
using TCPCommunication;
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

            //Loaded += UnityControl_Load;
            Loaded += TCPClient_Load;
            Activated += Form1_Activated;
            Deactivated += Form1_Deactivate;
            Closed += Form1_Closed;

            command_01.Click += Command_01_Click;
            command_02.Click += Command_02_Click;
            command_03.Click += Command_03_Click;
        }

        private void Command_01_Click(object sender, RoutedEventArgs e)
        {
            tcpClient.Send(this.Write(CommandId.Command_01));
        }

        private void Command_02_Click(object sender, RoutedEventArgs e)
        {
            tcpClient.Send(this.Write(CommandId.Command_02));
        }

        private void Command_03_Click(object sender, RoutedEventArgs e)
        {
            tcpClient.Send(this.Write(CommandId.Command_03));
        }
    }
}
