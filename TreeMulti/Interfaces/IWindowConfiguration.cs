using System.Windows;

namespace TreeMulti.Interfaces
{
    public interface IWindowConfiguration
    {
        Size GetWindowSize();
        string GetWindowTitle();
        ResizeMode GetResizeMode();
        SizeToContent GetSizeToContent();

    }
}
