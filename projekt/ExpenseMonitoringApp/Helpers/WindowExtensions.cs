using System.Windows;

namespace ExpenseMonitoringApp.Helpers
{
    public static class WindowExtensions
    {
        public static void CopySizeAndPosition(this Window window, Window anotherWindow)
        {
            window.Left = anotherWindow.Left;
            window.Top = anotherWindow.Top;
            window.Width = anotherWindow.Width;
            window.Height = anotherWindow.Height;
            if (anotherWindow.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Maximized;
            }
        }
    }
}
