using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble
{
    static class Program
    {
        public static Form1 form;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Form1 form1 = new Form1();
            //Application.Run(form1);
            Application.Run(form = new Form1());
            //Program.form.Execute(() => {Program.form});
        }
    }
}
