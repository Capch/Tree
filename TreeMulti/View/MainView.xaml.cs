using System.Windows;
using TreeMulti.Interfaces;

namespace TreeMulti.View
{
    public partial class MainView : IWindowConfiguration
    {

        public MainView()
        {
            InitializeComponent();         
        }

        public Size GetWindowSize()
        {
            return new Size(800,600);
        }

        public string GetWindowTitle()
        {
            return "Tree test";
        }

        public ResizeMode GetResizeMode()
        {
            return ResizeMode.CanResize;
        }

        public SizeToContent GetSizeToContent()
        {
            return SizeToContent.Manual;
        }
    }
}
