﻿using ERP_ventas.Datos;
using ERP_ventas.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERP_ventas.Formularios.Envios
{
    public partial class AddEnvio : Form
    {
        Modelo.Envios envios;
        public AddEnvio()
        {
            InitializeComponent();
            modificarButton.Visible = false;
            btnNuevo.Visible = true;
        }
        

        public AddEnvio(Modelo.Envios env)
        {
            InitializeComponent();
            this.envios = env;
            label8.Text ="Modificar";
            modificarButton.Visible = true;
            btnNuevo.Visible = false;

        }


        private void AddEnvio_Load(object sender, EventArgs e)
        {
            comboBox2.Items.AddRange((new TransporteDAO().ConsultaGeneral("", new List<string>(), new List<object>())).ToArray());
            comboBox2.DisplayMember = "Trans";
            if (envios!=null)
            {
                dateTimePicker1.Value = Convert.ToDateTime(envios.fechaSalida);
                if (envios.estatus == 'A')
                {
                    comboBox1.SelectedIndex = 0;
                }
                else 
                {
                    comboBox1.SelectedIndex = 1;
                }
                for (int i=0;i<comboBox2.Items.Count;i++)
                {
                    comboBox2.SelectedIndex=i;
                    if (((Transporte)comboBox2.SelectedItem).ID==envios.idenvio)
                    {
                        break;
                    }
                }
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
            {
                Mensajes.Error("Rellene los campos vacios antes de realizar el registro");
            }
            else
            {
                int h = 0;
                string f = "";
                try
                {
                    EnviosDAO envdao = new EnviosDAO();
                    char status;
                    if (comboBox1.Text == "Activo")
                    {
                        status = 'A';
                    }
                    else
                    {
                        status = 'I';
                    }
                    Modelo.Envios env = new Modelo.Envios(1, dateTimePicker1.Value, ((Transporte)comboBox2.SelectedItem).ID, status);
                    envdao.Registrar(env);
                }
                catch (Exception ex)
                {
                    f = "" + ex;
                    h = 1;
                }
                if (h == 1)
                {
                    Mensajes.Error(f);
                }
                else
                {
                    Mensajes.Info("Registro completado");
                }
                this.Close();
            }

        }

        private void modificarButton_Click(object sender, EventArgs e)
        {
            int h = 0;
            string f = "";
            try
            {
                EnviosDAO envdao = new EnviosDAO();
                char status;
                if (comboBox1.Text == "Activo")
                {
                    status = 'A';
                }
                else
                {
                    status = 'I';
                }
                Modelo.Envios env = new Modelo.Envios(envios.idenvio, dateTimePicker1.Value, ((Transporte)comboBox2.SelectedItem).ID, status);
                envdao.Editar(env);
            }
            catch (Exception ex)
            {
                f = "" + ex;
                h = 1;
            }
            if (h == 1)
            {
                Mensajes.Error(f);
            }
            else
            {
                Mensajes.Info("Los campos se editaron correctamente");
            }
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
