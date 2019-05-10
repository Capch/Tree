using System;
using System.ComponentModel;
using System.Windows;
using TreeMulti.Interfaces;

namespace TreeMulti
{
    public partial class DialogBase
    {

        public DialogBase(FrameworkElement viewElement)
        {
            ContentElement = viewElement;
            InitializeComponent();
            DataContext = this;
        }
        public FrameworkElement ContentElement { get; set; }

    }
}
