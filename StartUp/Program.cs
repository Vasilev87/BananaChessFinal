using System;
using System.Windows.Forms;

namespace Banana_Chess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 viewForm = new Form1();
            Application.Run(viewForm);  //aparently this is stopping the execution flow
        }
    }
}
