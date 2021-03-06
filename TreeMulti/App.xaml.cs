﻿using System;
using System.Windows;
using TreeMulti.Interfaces;
using TreeMulti.Model;
using TreeMulti.View;
using TreeMulti.ViewModel;

namespace TreeMulti
{
    public partial class App : Application
    {
        public App()
        {
            DialogService = new DialogService();

            DialogService.RegisterView(new MainView(), typeof(MainViewModel));
            DialogService.RegisterView(new AddNodeView(), typeof(AddViewModel));
        }
        public IDialogService DialogService { get; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var addEditVmFunc = new Func<Node, AddViewModel>(c => new AddViewModel(c));
            var mainViewModel = new MainViewModel(new JsonRepository("file.json"), ((App)Application.Current).DialogService, addEditVmFunc);
            DialogService.ShowDialog(mainViewModel);
        }
    }
}
