﻿using ERP_ventas.Datos;
using ERP_ventas.Formularios.Clientes;
using ERP_ventas.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERP_ventas.Clientes
{
    public partial class ClienteIndividualForm : Form
    {

        ClienteDAO clienteDAO;
        ClienteIndividualDAO clienteIndividual;
        public ClienteIndividualForm()
        {
            InitializeComponent();
            clienteDAO = new ClienteDAO();
            try
            {
                clienteIndividual = new ClienteIndividualDAO();
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex.Message);
            }
        }

        private void ClienteIndividual_Load(object sender, EventArgs e)
        {
            elementosPaginacionCmb.SelectedIndex = 0;
        }

        private void actualizarTabla()
        {
            clienteIndividual.actual_page = 0;
            clienteIndividual.CalculatePages();
            if (clienteIndividual.pages > 1)
            {
                anteriorBtn.Enabled = false;
                siguienteBtn.Enabled = true;
            }
            else
            {
                anteriorBtn.Enabled = false;
                siguienteBtn.Enabled = false;
            }
            dataGridViewClientes.DataSource = clienteIndividual.getNextPage();
            paginaxdey.Text = clienteIndividual.actual_page + "  de  " + clienteIndividual.pages;
        }

        private void dataGridViewClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewClientes.Rows.Count > 0)
            {
                if (e.RowIndex != -1)
                {
                    DataGridViewRow renglon = dataGridViewClientes.Rows[e.RowIndex];
                    string sql_where = " where idcliente=@id";
                    List<string> parametros = new List<string>();
                    List<object> valores = new List<object>();

                    parametros.Add("@id");
                    valores.Add(renglon.Cells["ID"].Value);

                    Cliente cliente = clienteDAO.ConsultaGeneral(sql_where, parametros, valores)[0];
                    ClienteIndividualAgregar clienteIndividualAgregar = new ClienteIndividualAgregar(cliente);
                    clienteIndividualAgregar.ShowDialog();
                    actualizarTabla();
                }
                else
                {
                    Mensajes.Error("Selecciona un registro");
                }
            }
        }

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {
            ClienteIndividualAgregar clienteIndividualAgregar = new ClienteIndividualAgregar();
            clienteIndividualAgregar.ShowDialog();
            actualizarTabla();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewClientes.SelectedRows.Count != 0)
            {
                DialogResult respuesta = Mensajes.PreguntaAdvertencia("¿Estás seguro de que quieres eliminar el cliente seleccionado?");
                if (respuesta == DialogResult.OK)
                {
                    try
                    {
                        DataGridViewRow renglon = dataGridViewClientes.SelectedRows[0];
                        clienteDAO.Eliminar((int)renglon.Cells["ID"].Value);
                        Mensajes.Info("Cliente eliminado");
                        actualizarTabla();
                    }
                    catch (Exception ex)
                    {
                        Mensajes.Error(ex.Message);
                    }
                }
            }
        }

        private void dataGridViewClientes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn i in dataGridViewClientes.Columns)
            {
                i.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            dataGridViewClientes.AutoResizeColumns();
        }

        private void anteriorBtn_Click(object sender, EventArgs e)
        {
            siguienteBtn.Enabled = true; //Al presionar sobre anterior, se habilita siguiente
            if (clienteIndividual.actual_page > 1)
            {
                dataGridViewClientes.DataSource = clienteIndividual.getPreviousPage();
            }
            if (clienteIndividual.actual_page == 1)
            {
                anteriorBtn.Enabled = false; //Deshabilita anterior porque está en la primer página
            }
            paginaxdey.Text = clienteIndividual.actual_page + "  de  " + clienteIndividual.pages;
        }

        private void siguienteBtn_Click(object sender, EventArgs e)
        {
            anteriorBtn.Enabled = true; //Al presionar sobre siguiente, se habilita anterior
            if (clienteIndividual.actual_page < clienteIndividual.pages)
            {
                dataGridViewClientes.DataSource = clienteIndividual.getNextPage();
            }
            if (clienteIndividual.actual_page == clienteIndividual.pages)
            {
                siguienteBtn.Enabled = false; //Deshabilita siguiente porque está en la última página
            }
            paginaxdey.Text = clienteIndividual.actual_page + "  de  " + clienteIndividual.pages;
        }

        private void elementosPaginacionCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clienteIndividual.rows_per_page = Convert.ToInt32(elementosPaginacionCmb.SelectedItem);
                actualizarTabla();
            }
            catch (Exception ex)
            {
                Mensajes.Error("Ha ocurrido un error. Contacta al administrador. \n" + ex.Message);
            }
        }

        private void buscartextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var dataTable = (DataTable)dataGridViewClientes.DataSource;
                var rows = dataTable.Select(string.Format("[Nombre] LIKE '%{0}%' OR " +
                                                          "[A. Paterno] LIKE '%{0}%' OR" +
                                                          "[A. Materno] LIKE '%{0}%' OR" +
                                                          "[Sexo] LIKE '%{0}%' OR" +
                                                          "[Dirección] LIKE '%{0}%' OR" +
                                                          "[Códifo Postal] LIKE '%{0}%' OR" +
                                                          "[Códifo Postal] LIKE '%{0}%' OR" +
                                                          "[RFC] LIKE '%{0}%' OR" +
                                                          "[Teléfono] LIKE '%{0}%' OR" +
                                                          "[email] LIKE '%{0}%' OR" +
                                                          "[Ciudad] LIKE '%{0}%' ",
                                                          buscartextBox.Text));
                if (rows.Count() != 0)
                    dataGridViewClientes.DataSource = rows.CopyToDataTable();
                else
                    Mensajes.Info("No se han encontrado resultados");
                dataGridViewClientes.Refresh();

            }
            catch (Exception ex)
            {
                Mensajes.Error(ex.Message);
            }
        }

        private void limpiarbutton_Click(object sender, EventArgs e)
        {
            buscartextBox.Text = "";
            actualizarTabla();
        }
    }
}
