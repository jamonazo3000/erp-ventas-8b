﻿using System;
using System.Collections.Generic;
using System.Linq;
using ERP_ventas.Formularios;
using System.Windows.Forms;
using ERP_ventas.Formularios.Clientes;
using ERP_ventas.Clientes;
using ERP_ventas.Formularios.Ofertas;

namespace ERP_ventas
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new Login());
            //Login loginForm = new Login();
            OfertasForm ofertasForm = new OfertasForm();
            ofertasForm.FormClosed += MainForm_Closed;
            ofertasForm.Show();
            Application.Run();
        }

        /*
       * Método encargado de cerrar las interfaces mostradas
       */
        private static void MainForm_Closed(object sender, FormClosedEventArgs e)
        {
            ((Form)sender).FormClosed -= MainForm_Closed;

            if (Application.OpenForms.Count == 0)
            {
                Application.ExitThread();
            }
            else
            {
                Application.OpenForms[0].FormClosed += MainForm_Closed;
            }
        }

    }
}
