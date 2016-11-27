﻿namespace Chloe
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChloeServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ChloeServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // ChloeServiceProcessInstaller
            // 
            this.ChloeServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ChloeServiceProcessInstaller.Password = null;
            this.ChloeServiceProcessInstaller.Username = null;
            // 
            // ChloeServiceInstaller
            // 
            this.ChloeServiceInstaller.Description = "Usługa sprawdzająca najtańsze połączenia między miastami";
            this.ChloeServiceInstaller.DisplayName = "Chloe";
            this.ChloeServiceInstaller.ServiceName = "Chloe";
            this.ChloeServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.ServiceName = "Service1";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ChloeServiceProcessInstaller,
            this.ChloeServiceInstaller,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ChloeServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ChloeServiceInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}