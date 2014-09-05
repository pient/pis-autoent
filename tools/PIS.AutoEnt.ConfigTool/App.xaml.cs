﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace PIS.AutoEnt.ConfigTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Contains("-ui"))
            {
                AppStarter.StartGUI();
            }
            else
            {
                AppStarter.StartConsole();
            }
        }
    }
}
