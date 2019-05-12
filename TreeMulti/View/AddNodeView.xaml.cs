using System.Windows;
using TreeMulti.Interfaces;

namespace TreeMulti.View
{
    public partial class AddNodeView : IWindowConfiguration
    {

        public AddNodeView()
        {
            InitializeComponent();
        }

        public Size GetWindowSize()
        {
           return new Size(300,400);
        }

        public string GetWindowTitle()
        {
            return "Add/Edit node";
        }

        public ResizeMode GetResizeMode()
        {
            return ResizeMode.NoResize;
        }

        public SizeToContent GetSizeToContent()
        {
            return SizeToContent.Height;
        }

    }
}
