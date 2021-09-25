using System;
using System.Windows.Forms;


namespace PictView_Test0
{

        class ViewerMain
        {
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
}
