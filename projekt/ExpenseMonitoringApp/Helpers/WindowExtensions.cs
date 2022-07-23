using System.Windows;

namespace ExpenseMonitoringApp.Helpers
{
    public static class WindowExtensions
    {
        /// <summary>
        /// Copies size, position of <paramref name="window"/> to <paramref name="anotherWindow"/>. 
        /// If <paramref name="window"/> is maximized, <paramref name="anotherWindow"/> 
        /// alse will be maximized.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="anotherWindow"></param>
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
