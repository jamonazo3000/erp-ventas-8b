﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_ventas.Modelo
{
    class Oferta
    {
        public int idOferta  { get; set; }
        public String nombre  { get; set; }
        public String descripcion  { get; set; } 
        public float porDescuento  { get; set; } 
        public String fechaInicio  { get; set; } 
        public String fechaFin  { get; set; } 
        public int canMinProducto  { get; set; } 
        public String estatus  { get; set; }

        public Oferta(int idOferta, string nombre, string descripcion, float porDescuento, string fechaInicio, string fechaFin, int canMinProducto, string estatus)
        {
            this.idOferta = idOferta;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.porDescuento = porDescuento;
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
            this.canMinProducto = canMinProducto;
            this.estatus = estatus;
        }
    }
}