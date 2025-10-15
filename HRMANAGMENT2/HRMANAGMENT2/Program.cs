using System.Windows;

namespace HRMANAGMENT2
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            var app = new Application();
            app.Run(new MainWindow());
        }
    }
}