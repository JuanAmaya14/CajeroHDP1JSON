using System;
using System.Collections.Generic;

namespace CajeroHDP1
{
    internal class Usuario
    {
        private int identificacion;
        private string nombre;
        private string correo;
        private string clave;
        private long saldo;
        private List<string> historial = new List<string>();
        private string numeroCuenta;

        // Constructor con número de cuenta opcional
        public Usuario(int identificacion, string nombre, string correo, string clave, long saldo, string numeroCuenta = null)
        {
            this.Identificacion = identificacion;
            this.Nombre = nombre;
            this.Correo = correo;
            this.Clave = clave;
            this.Saldo = saldo;
            this.numeroCuenta = numeroCuenta ?? GenerarCuenta();
        }

        private string GenerarCuenta()
        {
            var random = new Random();
            string numeroCuenta = "";
            for (int i = 0; i < 11; i++)
            {
                numeroCuenta += random.Next(0, 10);
            }
            return numeroCuenta;
        }

        public int Identificacion { get => identificacion; set => identificacion = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Clave { get => clave; set => clave = value; }
        public long Saldo { get => saldo; set => saldo = value; }
        public List<string> Historial { get => historial; set => historial = value; }
        public string NumeroCuenta { get => numeroCuenta; set => numeroCuenta = value; }
    }
    
}