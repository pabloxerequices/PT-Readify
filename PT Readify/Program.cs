using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;

namespace PT_Readify
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                new DAL().GarantirEsquema();
            }
            catch
            {
                // Se a BD ainda não existir, o esquema será aplicado na próxima execução.
            }

            Application.Run(new Form1());
        }
    }
}
