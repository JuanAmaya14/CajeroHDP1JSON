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

        public Usuario(int identificacion, string nombre, string correo, string clave, long saldo)
        {
            this.Identificacion = identificacion;
            this.Nombre = nombre;
            this.Correo = correo;
            this.Clave = clave;
            this.Saldo = saldo;
        }

        public int Identificacion { get => identificacion; set => identificacion = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Clave { get => clave; set => clave = value; }
        public long Saldo { get => saldo; set => saldo = value; }
        public List<string> Historial { get => historial; set => historial = value; }

    }
}
