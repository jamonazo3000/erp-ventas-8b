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

namespace ERP_ventas.Formularios.Ventas
{
    public partial class VentasForm : Form
    {
        private VentaDAO ventaDAO;
        public VentasForm()
        {
            InitializeComponent();
            ventaDAO = new VentaDAO();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            VentasAgregar ventasAgregar = new VentasAgregar();
            ventasAgregar.ShowDialog();
            actualizarTabla();

        }

        private void VentasForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal();
            menu.Show();
        }

        private void VentasForm_Load(object sender, EventArgs e)
        {
            dataVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            try
            {
                dataVentas.DataSource = ventaDAO.ConsultaGeneral(" where estatus !='I'", new List<string>(), new List<object>());
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex.Message);
            }
        }

        private void actualizarTabla()
        {
            try
            {
                dataVentas.DataSource = ventaDAO.ConsultaGeneral(" where estatus != 'I'", new List<string>(), new List<object>());
            }
            catch (Exception ex)
            {
                Mensajes.Error(ex.Message);
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (dataVentas.SelectedRows.Count == 1)
            {
                var row = dataVentas.SelectedRows[0];
                var venta = (Venta)row.DataBoundItem; //Castea el row como objeto de la clase venta
                if (venta.EstatusChar == 'P')
                {
                    //Mensajes.Error(venta.ToString());
                    EnviosVentas.AddEnviosVentas addEnviosVentas = new EnviosVentas.AddEnviosVentas(venta.ID,venta.Fecha);
                    addEnviosVentas.ShowDialog();
                }
                else
                    Mensajes.Info("La venta seleccionada aún no se puede enviar");
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            if (dataVentas.SelectedRows.Count == 1)
            {
                var row = dataVentas.SelectedRows[0];
                Venta venta = (Venta)row.DataBoundItem;

                if (venta.EstatusChar == 'A')
                {
                    VentasAgregar ventasAgregar = new VentasAgregar(venta);
                    ventasAgregar.ShowDialog();

                    actualizarTabla();
                } else if (venta.EstatusChar == 'P') {
                    VentasAgregar ventasAgregar = new VentasAgregar(venta,"Completa");
                    ventasAgregar.ShowDialog();
                }
                else
                {
                    Mensajes.Error("La venta no se puede editar");
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if (dataVentas.SelectedRows.Count == 1)
            {
                var row = dataVentas.SelectedRows[0];
                Venta venta = (Venta)row.DataBoundItem;
                DialogResult result = Mensajes.PreguntaAdvertencia(string.Format("¿Estás seguro de eliminar la venta no. {0}?", venta.ID));
                if (result == DialogResult.OK)
                {
                    try
                    {
                        if (venta.EstatusChar != 'P' && venta.EstatusChar != 'F')
                        {
                            if (ventaDAO.Eliminar(venta.ID))
                            {
                                Mensajes.Info(string.Format("La venta no. {0} ha sido eliminada", venta.ID));
                                actualizarTabla();
                            }
                            else
                            {
                                Mensajes.Info(string.Format("La venta no. {0} no pudo ser eliminada", venta.ID));
                            }
                        }
                        else
                            Mensajes.Error("No es posible eliminar una venta que ya ha sido completada.");
                    }
                    catch (Exception ex)
                    {
                        Mensajes.Error(ex.StackTrace);
                    }
                }
            }

        }

        private void CopiarGrilla()
        {
            dataVentas.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataVentas.MultiSelect = true;
            dataVentas.SelectAll();
            DataObject dataObj = dataVentas.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);

            dataVentas.MultiSelect = false;
            dataVentas.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.CopiarGrilla();

            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object valor = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(valor);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

            MessageBox.Show("Exportación finalizada");
        }
    }
}
