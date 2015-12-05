using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace CopyVSExecutableToIdealAutomate {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    protected override void OnStartup(StartupEventArgs e) {
      base.OnStartup(e);
      DispatcherUnhandledException += app_DispatcherUnhandledException;
    }
    static void app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
      MessageBox.Show(e.Exception.InnerException.ToString());
      e.Handled = true;
      // Log/inspect the inspection here
    }
  }
}
