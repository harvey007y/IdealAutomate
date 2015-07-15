﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IdealAutomate.Core;

namespace TestAgentRansack {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      this.Hide();
      IdealAutomate.Core.Methods myActions = new Methods();
      myActions.Run(@"c:\Program Files\Mythicsoft\Agent Ransack\AgentRansack.exe","");
      Application.Current.Shutdown();
    }
  }
}