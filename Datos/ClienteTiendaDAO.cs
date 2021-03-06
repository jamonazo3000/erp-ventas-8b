﻿using ERP_ventas.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_ventas.Datos
{
    class ClienteTiendaDAO : Paginacion //Necesario heredar de la clase Paginación para funcionar
    {

        public List<ClienteTienda> ConsultaGeneral(string sql_where, List<string> parametros, List<object> valores)
        {
            List<ClienteTienda> clientes = new List<ClienteTienda>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Properties.Settings.Default.cadenaConexion))
                {
                    string cadena_sql = "select * from ClienteTienda " + sql_where;
                    //idCliente	nombre	Contacto	limiteCredito

                    SqlCommand comando = new SqlCommand(cadena_sql, conexion);

                    conexion.Open();
                    for (int i = 0; i < parametros.Count; i++)
                    {
                        comando.Parameters.AddWithValue(parametros[i], valores[i]);
                    }

                    SqlDataReader lector = comando.ExecuteReader();
                    if (lector.HasRows)
                    {
                        while (lector.Read())
                        {
                            Console.WriteLine(lector.GetDataTypeName(3));
                            ClienteTienda cliente = new ClienteTienda(
                                 (int)lector["idCliente"],
                                 (string)lector["nombre"],
                                 (string)lector["Contacto"],
                                 decimal.Parse(lector["limiteCredito"].ToString())
                                 );
                            clientes.Add(cliente);
                        }
                        lector.Close();
                        conexion.Close();
                    }
                    else
                    {
                        lector.Close();
                        conexion.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error relacionado con la BD. [ClienteIndividualDAO] \n Anota este error y contacta al administrador.\n" + ex.Message);
            }
            return clientes;
        }

        public ClienteTiendaDAO()
        {
            table = "Clientes_Tienda";  //Nombre de la tabla o vista
            order_by = "Nombre Tienda"; //Nombre de la columna para ordenar los registros
            rows_per_page = 3;          //Cantidad de registros por página
            try
            {
               CalculatePages(); //Calcula la cantidad de páginas que se deben emplear
            }catch (Exception ex)
            {
                throw ex;
            }
        }


        public object Registrar(ClienteTienda cte, int ID)
        {
            object resultado = new object();
            try
            {
                if (Validar(0, ID, cte))
                {
                    using (SqlConnection conexion = new SqlConnection(Properties.Settings.Default.cadenaConexion))
                    {
                        string cadena_sql = "insert into ClienteTienda values (@id, @nombre, @contacto, @limite)";

                        SqlCommand comando = new SqlCommand(cadena_sql, conexion);
                        comando.Parameters.AddWithValue("@id", ID);
                        comando.Parameters.AddWithValue("@nombre", cte.Nombre);
                        comando.Parameters.AddWithValue("@contacto", cte.Contacto);
                        comando.Parameters.AddWithValue("@limite", cte.Limite);
                        conexion.Open();

                        int cant_registros = (int)comando.ExecuteNonQuery();
                        conexion.Close();
                        if (cant_registros == 1)
                        {
                            resultado = true;
                        }
                        else
                        {
                            resultado = "Se ha generado un error no especificado";
                        }
                    }
                }
                else
                {
                    resultado = "Error: Ya existe un cliente de tienda con datos en común";
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error relacionado con la BD. [ClienteTiendaDAO.R] \n Anota este error y contacta al administrador.\n" + ex.Message);
            }
            return resultado;
        }

        public object Editar(ClienteTienda cte, int ID)
        {
            object resultado = new object();
            try
            {
                if (Validar(1, ID, cte))
                {
                    using (SqlConnection conexion = new SqlConnection(Properties.Settings.Default.cadenaConexion))
                    {
                        string cadena_sql = "update ClienteTienda set nombre= @nombre, contacto=@contacto, limiteCredito=@limite where idCliente=@id";

                        SqlCommand comando = new SqlCommand(cadena_sql, conexion);
                        comando.Parameters.AddWithValue("@id", ID);
                        comando.Parameters.AddWithValue("@nombre", cte.Nombre);
                        comando.Parameters.AddWithValue("@contacto", cte.Contacto);
                        comando.Parameters.AddWithValue("@limite", cte.Limite);
                        conexion.Open();

                        int cant_registros = (int)comando.ExecuteNonQuery();
                        conexion.Close();
                        if (cant_registros == 1)
                        {
                            resultado = true;
                        }
                        else
                        {
                            resultado = "Se ha generado un error no especificado";
                        }
                    }
                }
                else
                {
                    resultado = "Error: Ya existe un cliente de tienda con datos en común";
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error relacionado con la BD. [ClienteTiendaDAO.R] \n Anota este error y contacta al administrador.\n" + ex.Message);
            }
            return resultado;
        }

        public bool Validar(int tipo, int ID, ClienteTienda cte)
        {
            // tipo 0 -> insert ; 1 -> update

            try
            {
                using (SqlConnection conexion = new SqlConnection(Properties.Settings.Default.cadenaConexion))
                {
                    string cadena_sql = "";
                    if (tipo == 0)
                        cadena_sql = "select count(idcliente) from ClienteTienda where nombre=@nombre or contacto=@contacto";

                    else
                        cadena_sql = "select count(idcliente) from ClienteTienda where (nombre=@nombre or contacto=@contacto) and idcliente!=@id";

                    SqlCommand comando = new SqlCommand(cadena_sql, conexion);
                    comando.Parameters.AddWithValue("@nombre", cte.Nombre);
                    comando.Parameters.AddWithValue("@contacto", cte.Contacto);
                    if (tipo == 1)
                        comando.Parameters.AddWithValue("@id", ID);
                    conexion.Open();

                    int registros = (int)comando.ExecuteScalar();
                    if (registros != 0)
                        return false;
                    else
                        return true;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error relacionado con la BD. [ClienteTiendaDAO.V] \n Anota este error y contacta al administrador.\n" + ex.Message);
            }
        }

    }
}
