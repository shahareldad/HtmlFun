﻿using System.ServiceProcess;

namespace WindowsServiceRunningHttpCommands
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SelfHostWebApiService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}