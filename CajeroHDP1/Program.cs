using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace CajeroHDP1
{
    internal class Program
    {
        static string filePath = "usuarios.json";
        static List<Usuario> usuarios = new List<Usuario>();

        static void Main(string[] args)
        {
            // Cargar usuarios existentes desde el archivo JSON (si existe)
            CargarUsuarios();
            MenuSesion();
            Console.ReadKey();
        }

        // Menú de inicio de sesión y registro
        private static void MenuSesion()
        {
            int opcion = 0;

            while (opcion != 3)
            {
                Console.WriteLine("1. Iniciar sesión" +
                    "\n2. Registrarse" +
                    "\n3. Salir");

                opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        IniciarSesion();
                        break;

                    case 2:
                        Registrarse();
                        break;

                    case 3:
                        GuardarUsuarios();  // Guardar cambios antes de salir
                        Console.WriteLine("Saliendo del programa...");
                        break;

                    default:
                        Console.WriteLine("Esa opción no existe.");
                        break;
                }
            }
        }

        // Cargar usuarios desde el archivo JSON
        private static void CargarUsuarios()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();
            }
            else
            {
                usuarios = new List<Usuario>();  // Si no existe, creamos una lista vacía
            }
        }

        // Guardar usuarios en el archivo JSON
        private static void GuardarUsuarios()
        {
            string json = JsonConvert.SerializeObject(usuarios, Formatting.Indented);
            File.WriteAllText(filePath, json);
            Console.WriteLine("Los datos han sido guardados correctamente.");
        }

        // Iniciar sesión
        private static void IniciarSesion()
        {
            int identificacion;
            string contrasenha;

            Console.Write("Identificación: ");
            identificacion = Convert.ToInt32(Console.ReadLine());

            Console.Write("Contraseña: ");
            contrasenha = Console.ReadLine();

            var usuario = usuarios.FirstOrDefault(u => u.Identificacion == identificacion && u.Clave == contrasenha);

            if (usuario != null)
            {
                Console.WriteLine("Usuario validado.");
                Menu(usuario);
            }
            else
            {
                Console.WriteLine("Usuario inválido. Verifica los datos o regístrate si no tienes cuenta.");
            }
        }

        // Registro de usuario
        private static void Registrarse()
        {
            int identificacion;
            string nombre, correo, clave, claveConfirmacion;
            long saldo;

            Console.Write("Identificación: ");
            identificacion = Convert.ToInt32(Console.ReadLine());

            // Verificar si la identificación ya existe
            while (usuarios.Any(u => u.Identificacion == identificacion))
            {
                Console.WriteLine("Esta identificación ya existe. Intenta con otra.");
                Console.Write("Identificación: ");
                identificacion = Convert.ToInt32(Console.ReadLine());
            }

            Console.Write("Nombre: ");
            nombre = Console.ReadLine();

            Console.Write("Correo: ");
            correo = Console.ReadLine();

            // Verificar si el correo ya existe
            while (usuarios.Any(u => u.Correo == correo))
            {
                Console.WriteLine("Este correo ya está registrado. Intenta con otro.");
                Console.Write("Correo: ");
                correo = Console.ReadLine();
            }

            // Validar contraseña y confirmación
            do
            {
                Console.Write("Contraseña: ");
                clave = Console.ReadLine();
                Console.Write("Confirmar contraseña: ");
                claveConfirmacion = Console.ReadLine();

                if (clave != claveConfirmacion)
                    Console.WriteLine("Las contraseñas no coinciden.");
            } while (clave != claveConfirmacion);

            Console.Write("Saldo inicial: ");
            saldo = long.Parse(Console.ReadLine());

            // Agregar el nuevo usuario a la lista
            usuarios.Add(new Usuario(identificacion, nombre, correo, clave, saldo));
            Console.WriteLine("Registro exitoso.");

            // Guardar cambios en el archivo JSON
            GuardarUsuarios();
        }

        // Menú principal
        private static void Menu(Usuario usuario)
        {
            int opcion = 0;
            while (opcion != 5)
            {
                Console.WriteLine($"Número de tu cuenta: {usuario.NumeroCuenta}" +
                    "\n1. Retirar" +
                    "\n2. Consignar" +
                    "\n3. Consultar saldo" +
                    "\n4. Consultar Movimientos" +
                    "\n5. Cerrar sesión");

                opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        Retirar(usuario);
                        break;

                    case 2:
                        Consignar(usuario);
                        break;

                    case 3:
                        ConsultarSaldo(usuario);
                        break;

                    case 4:
                        ConsultarMovimientos(usuario);
                        break;

                    case 5:
                        Console.WriteLine("Cerrando sesión...");
                        break;

                    default:
                        Console.WriteLine("Esa opción no existe.");
                        break;
                }
            }
        }

        // Consultar movimientos
        private static void ConsultarMovimientos(Usuario usuario)
        {
            List<string> historial = usuario.Historial;

            if (historial.Count != 0)
            {
                foreach (string historia in historial)
                {
                    Console.WriteLine(historia);
                }
            }
            else
            {
                Console.WriteLine("No hay movimientos por el momento.");
            }
        }

        // Consultar saldo
        private static void ConsultarSaldo(Usuario usuario)
        {
            Console.WriteLine($"Actualmente tienes {usuario.Saldo:C}");
        }

        // Consignar a otra cuenta
        private static void Consignar(Usuario usuario)
        {
            Console.Write("Cantidad de dinero a enviar: ");
            int dinero = Convert.ToInt32(Console.ReadLine());

            Console.Write("Número de cuenta destino: ");
            string numeroCuenta = Console.ReadLine();

            var usuarioReceptor = usuarios.FirstOrDefault(u => u.NumeroCuenta == numeroCuenta);

            if (usuarioReceptor != null)
            {
                Console.WriteLine($"Vas a transferir {dinero:C} a la cuenta {numeroCuenta}. ¿Estás seguro? (1. Sí, 2. No)");
                int opcion = Convert.ToInt32(Console.ReadLine());

                if (opcion == 1)
                {
                    if (usuario.Saldo >= dinero)
                    {
                        usuario.Saldo -= dinero;
                        usuario.Historial.Add($"{DateTime.Today} Transferencia a la cuenta {numeroCuenta} de -{dinero:C}");

                        usuarioReceptor.Saldo += dinero;
                        usuarioReceptor.Historial.Add($"{DateTime.Today} Consignación de +{dinero:C}");

                        Console.WriteLine("La transferencia se realizó con éxito.");

                        // Guardar cambios en el archivo JSON
                        GuardarUsuarios();
                    }
                    else
                    {
                        Console.WriteLine($"Saldo insuficiente. Tu saldo es {usuario.Saldo:C}");
                    }
                }
                else
                {
                    Console.WriteLine("Cancelando transacción...");
                }
            }
            else
            {
                Console.WriteLine("La cuenta de destino no existe.");
            }
        }

        // Retirar dinero
        private static void Retirar(Usuario usuario)
        {
            Console.Write("Valor a retirar: ");
            int valorRetirar = Convert.ToInt32(Console.ReadLine());

            if (usuario.Saldo >= valorRetirar)
            {
                usuario.Saldo -= valorRetirar;
                usuario.Historial.Add($"{DateTime.Today} Retiro de -{valorRetirar:C}");
                Console.WriteLine($"Saldo actual: {usuario.Saldo:C}");

                // Guardar cambios en el archivo JSON
                GuardarUsuarios();
            }
            else
            {
                Console.WriteLine($"Saldo insuficiente. Tu saldo es {usuario.Saldo:C}");
            }
        }
    }
}


