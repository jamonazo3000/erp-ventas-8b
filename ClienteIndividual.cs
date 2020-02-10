﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERP_ventas
{
    public partial class ClienteIndividual : Form
    {
        public ClienteIndividual()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            /*Verificación de ingresar datos*/
            String nombre, aPaterno, aMaterno, sexo,
                mensajeError;

            nombre = txtNombre.Text;
            aPaterno = txtAPaterno.Text;
            aMaterno = txtAMaterno.Text;
            mensajeError = "Errores: \n";
            /*
             * Se utilizan expresiones regulares para que todos los campos sean solo letras.
             
             En el caso del nombre existe la posibilidad de tener 2 nombres por lo que se permite el espacio
             */
            bool soloLetras = Regex.IsMatch(nombre, @"^[a-zA-Z][a-z A-Z]+$");
            soloLetras = soloLetras && Regex.IsMatch(aPaterno, @"^[a-zA-Z]+$");
            soloLetras = soloLetras && Regex.IsMatch(aMaterno, @"^[a-zA-Z]+$");

            if (soloLetras)
            {
                MessageBox.Show("Todo bien", "titulo");
            }
            else {
                mensajeError = "-Solo se permiten el uso de letras \n";
                if (Regex.IsMatch(nombre, @"^[a-z A-Z][a-zA-Z]+$")) { //¿Comienza con espacio?
                    mensajeError += "-No se puede comenzar con espacio \n";
                }
            }

            //Verificación de campo sexo
            if (rdtMasculino.Checked)
            {
                sexo = "M";
            }
            else if (rdtFemenino.Checked)
            {
                sexo = "F";
            }
            else
            {
                mensajeError += "Campo sexo no seleccionado \n";
            }

            //Mostrar errores
            MessageBox.Show(mensajeError, "Error al registrar");
        }
    }
}
