﻿using System;
using ERP_ventas.Modelo;
using ERP_ventas.Datos;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERP_ventas.Formularios.Tripulacion
{
    public partial class Tripulacion : Form
    {
        TripulacionDAO tripulacionDAO;
        public Tripulacion()
        {
            InitializeComponent();
            try
            {
                tripulacionDAO = new TripulacionDAO();
            }
            catch(Exception ex) {
                Mensajes.Error(ex.Message);
            }
        }

        private void Tripulacion_Load(object sender, EventArgs e)
        {
            elementosPaginacionCmb.SelectedIndex = 0;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            AddTripulacion addTripulacion = new AddTripulacion();
            addTripulacion.ShowDialog();
            ActualizaTabla();
        }

        private void ActualizaTabla() 
        {
            tripulacionDAO.actual_page = 0;
            tripulacionDAO.CalculatePages();
            if (tripulacionDAO.pages > 1)
            {
                anteriorBtn.Enabled = false;
                siguienteBtn.Enabled = true;
            }
            else 
            {
                anteriorBtn.Enabled = false;
                siguienteBtn.Enabled = false;
            }
            dataTripulacion.DataSource = tripulacionDAO.getNextPage();
            paginaxdey.Text = tripulacionDAO.actual_page + " de " + tripulacionDAO.pages; 
        }

        private void dataOfertas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataTripulacion_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn i in dataTripulacion.Columns)
            {
                i.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            dataTripulacion.AutoResizeColumns();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataTripulacion.SelectedRows.Count !=0)
            {
                DialogResult respuesta = Mensajes.PreguntaAdvertencia("¿Estás seguro que deseas eliminarlo?");
                if (respuesta == DialogResult.OK) 
                {
                    try
                    {
                        DataGridViewRow renglon = dataTripulacion.SelectedRows[0];
                        tripulacionDAO.Eliminar((int)renglon.Cells["ID"].Value);
                    }
                    catch (Exception ex) 
                    {
                        Mensajes.Error(ex.Message);
                    }
                }
            }
        }

        private void anteriorBtn_Click(object sender, EventArgs e)
        {
            siguienteBtn.Enabled = true; //Al presionar sobre anterior, se habilita siguiente
            if (tripulacionDAO.actual_page > 1)
            {
                dataTripulacion.DataSource = tripulacionDAO.getPreviousPage();
            }
            if (tripulacionDAO.actual_page == 1)
            {
                anteriorBtn.Enabled = false; //Deshabilita anterior porque está en la primer página
            }
            paginaxdey.Text = tripulacionDAO.actual_page + "  de  " + tripulacionDAO.pages;
        }

        private void siguienteBtn_Click(object sender, EventArgs e)
        {
            anteriorBtn.Enabled = true; //Al presionar sobre siguiente, se habilita anterior
            if (tripulacionDAO.actual_page < tripulacionDAO.pages)
            {
                dataTripulacion.DataSource = tripulacionDAO.getNextPage();
            }
            if (tripulacionDAO.actual_page == tripulacionDAO.pages)
            {
                siguienteBtn.Enabled = false; //Deshabilita siguiente porque está en la última página
            }
            paginaxdey.Text = tripulacionDAO.actual_page + "  de  " + tripulacionDAO.pages;
        }

        private void elementosPaginacionCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tripulacionDAO.rows_per_page = Convert.ToInt32(elementosPaginacionCmb.SelectedItem);
                actualizarTabla();
            }
            catch (Exception ex)
            {
                Mensajes.Error("Ha ocurrido un error. Contacta al administrador. \n" + ex.Message);
            }
        }

        private void actualizarTabla()
        {
            tripulacionDAO.actual_page = 0;
            tripulacionDAO.CalculatePages();
            if (tripulacionDAO.pages > 1)
            {
                anteriorBtn.Enabled = false;
                siguienteBtn.Enabled = true;
            }
            else
            {
                anteriorBtn.Enabled = false;
                siguienteBtn.Enabled = false;
            }
            dataTripulacion.DataSource = tripulacionDAO.getNextPage();
            paginaxdey.Text = tripulacionDAO.actual_page + "  de  " + tripulacionDAO.pages;
        }

        private void anteriorBtn_Click_1(object sender, EventArgs e)
        {
            siguienteBtn.Enabled = true; //Al presionar sobre anterior, se habilita siguiente
            if (tripulacionDAO.actual_page > 1)
            {
                dataTripulacion.DataSource = tripulacionDAO.getPreviousPage();
            }
            if (tripulacionDAO.actual_page == 1)
            {
                anteriorBtn.Enabled = false; //Deshabilita anterior porque está en la primer página
            }
            paginaxdey.Text = tripulacionDAO.actual_page + "  de  " + tripulacionDAO.pages;
        }

        private void siguienteBtn_Click_1(object sender, EventArgs e)
        {
            anteriorBtn.Enabled = true; //Al presionar sobre siguiente, se habilita anterior
            if (tripulacionDAO.actual_page < tripulacionDAO.pages)
            {
                dataTripulacion.DataSource = tripulacionDAO.getNextPage();
            }
            if (tripulacionDAO.actual_page == tripulacionDAO.pages)
            {
                siguienteBtn.Enabled = false; //Deshabilita siguiente porque está en la última página
            }
            paginaxdey.Text = tripulacionDAO.actual_page + "  de  " + tripulacionDAO.pages;
        }
    }
}
