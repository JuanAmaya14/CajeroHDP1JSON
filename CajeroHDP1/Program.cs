using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace CajeroHDP1
{
    internal class Program
    {

        // Convierte la hora UTC a la hora local
        static DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
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
                Console.WriteLine(" ╔════════════════════════════════════╗");
                Console.WriteLine(" ║             BANCOHDP1              ║");
                Console.WriteLine(" ║════════════════════════════════════║");
                Console.WriteLine(" ║              OPCIONES              ║");
                Console.WriteLine(" ║ (Digita el numero correspondiente) ║");
                Console.WriteLine(" ║════════════════════════════════════║");
                Console.WriteLine(" ║         1. Iniciar Sesion          ║");
                Console.WriteLine(" ║         2. Registrarse             ║");
                Console.WriteLine(" ║         3. Salir                   ║");
                Console.WriteLine(" ╚════════════════════════════════════╝");
                Console.Write(" ═>");
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
                        Console.WriteLine(" ╔═══════════════════════╗");
                        Console.WriteLine(" ║ Saliendo del programa ║");
                        Console.WriteLine(" ╚═══════════════════════╝");
                        break;

                    default:
                        Console.WriteLine(" ╔══════════════════════════════════════════╗");
                        Console.WriteLine(" ║ Esta opcion no existe.Digite nuevamente  ║");
                        Console.WriteLine(" ╚══════════════════════════════════════════╝");
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
            Console.WriteLine(" ╔═══════════════════════════════════════════════╗");
            Console.WriteLine(" ║  Los datos han sido guardados correctamente.  ║");
            Console.WriteLine(" ╚═══════════════════════════════════════════════╝");
        }

        // Iniciar sesión
        private static void IniciarSesion()
        {
            int identificacion;
            string contrasenha;

            Console.WriteLine(" ╔═══════════════════════╗");
            Console.WriteLine(" ║     IDENTIFICACION:   ║");
            Console.WriteLine(" ╚═══════════════════════╝");
            Console.Write(" ═>");
            identificacion = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(" ╔═══════════════════════╗");
            Console.WriteLine(" ║      CONTRASEÑA:      ║");
            Console.WriteLine(" ╚═══════════════════════╝");
            Console.Write(" ═>");
            contrasenha = Console.ReadLine();

            var usuario = usuarios.FirstOrDefault(u => u.Identificacion == identificacion && u.Clave == contrasenha);

            if (usuario != null)
            {
                Console.WriteLine(" ╔═══════════════════════╗");
                Console.WriteLine(" ║   Usuario validado.   ║");
                Console.WriteLine(" ╚═══════════════════════╝");
                Menu(usuario);
            }
            else
            {
                Console.WriteLine(" ╔═══════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine(" ║        Usuario invalido, verifique que ingresaste bien los datos          ║");
                Console.WriteLine(" ║     o en caso de que no estes registrado te invitamos a que te registres  ║");
                Console.WriteLine(" ╚═══════════════════════════════════════════════════════════════════════════╝");
            }
        }

        // Registro de usuario
        private static void Registrarse()
        {
            int identificacion;
            string nombre, correo, clave, claveConfirmacion;
            long saldo;

            Console.WriteLine(" ╔═══════════════════════╗");
            Console.WriteLine(" ║     IDENTIFICACION:   ║");
            Console.WriteLine(" ╚═══════════════════════╝");
            Console.Write(" ═>");
            identificacion = Convert.ToInt32(Console.ReadLine());

            // Verificar si la identificación ya existe
            while (usuarios.Any(u => u.Identificacion == identificacion))
            {
                Console.WriteLine(" ╔═══════════════════════════════════════════════════════╗");
                Console.WriteLine(" ║     Esta identificacion ya existe. Intenta con otra   ║");
                Console.WriteLine(" ╚═══════════════════════════════════════════════════════╝");
                Console.WriteLine(" ╔═══════════════════════╗");
                Console.WriteLine(" ║     IDENTIFICACION:   ║");
                Console.WriteLine(" ╚═══════════════════════╝");
                Console.Write(" ═>");
                identificacion = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine(" ╔════════════════╗");
            Console.WriteLine(" ║     NOMBRE:    ║");
            Console.WriteLine(" ╚════════════════╝");
            Console.Write(" ═>");
            nombre = Console.ReadLine();

            Console.WriteLine(" ╔════════════════╗");
            Console.WriteLine(" ║     CORREO:    ║");
            Console.WriteLine(" ╚════════════════╝");
            Console.Write(" ═>");
            correo = Console.ReadLine();

            // Verificar si el correo ya existe
            while (usuarios.Any(u => u.Correo == correo))
            {
                Console.WriteLine(" ╔═══════════════════════════════════════════════════════╗");
                Console.WriteLine(" ║   Este correo ya esta registrado. Intente con otro    ║");
                Console.WriteLine(" ╚═══════════════════════════════════════════════════════╝");
                Console.WriteLine(" ╔════════════════╗");
                Console.WriteLine(" ║     CORREO:    ║");
                Console.WriteLine(" ╚════════════════╝");
                Console.Write(" ═>");
                correo = Console.ReadLine();
            }

            // Validar contraseña y confirmación
            do
            {
                Console.WriteLine(" ╔═══════════════════════╗");
                Console.WriteLine(" ║      CONTRASEÑA:      ║");
                Console.WriteLine(" ╚═══════════════════════╝");
                Console.Write(" ═>");
                clave = Console.ReadLine();

                Console.WriteLine(" ╔═══════════════════════╗");
                Console.WriteLine(" ║ CONFIRMAR CONTRASEÑA  ║");
                Console.WriteLine(" ╚═══════════════════════╝");
                Console.Write(" ═>");
                claveConfirmacion = Console.ReadLine();

                if (clave != claveConfirmacion)
                {
                    Console.WriteLine(" ╔══════════════════════════════════════════════════╗");
                    Console.WriteLine(" ║ LAS CONTRASEÑAS NO COINCIDEN. DIGITE NUEVAMENTE  ║");
                    Console.WriteLine(" ╚══════════════════════════════════════════════════╝");
                }

            } while (clave != claveConfirmacion);

            Console.WriteLine(" ╔══════════════════╗");
            Console.WriteLine(" ║   SALDO INICIAL  ║");
            Console.WriteLine(" ╚══════════════════╝");
            Console.Write(" ═>");
            saldo = long.Parse(Console.ReadLine());

            // Agregar el nuevo usuario a la lista
            usuarios.Add(new Usuario(identificacion, nombre, correo, clave, saldo));
            Console.WriteLine(" ╔════════════════════╗");
            Console.WriteLine(" ║  REGISTRO EXITOSO  ║");
            Console.WriteLine(" ╚════════════════════╝");

            // Guardar cambios en el archivo JSON
            GuardarUsuarios();
        }

        // Menú principal
        private static void Menu(Usuario usuario)
        {
            int opcion = 0;
            while (opcion != 5)
            {
                Console.WriteLine(" ╔════════════════════════════════════╗");
                Console.WriteLine(" ║             BANCOHDP1              ║");
                Console.WriteLine(" ║════════════════════════════════════║");
                Console.WriteLine(" ║         Número de tu cuenta:       ║");
                Console.WriteLine($"         {usuario.NumeroCuenta}       ");
                Console.WriteLine(" ║════════════════════════════════════║");
                Console.WriteLine(" ║         1. Retirar                 ║");
                Console.WriteLine(" ║         2. Consignar               ║");
                Console.WriteLine(" ║         3. Consultar saldo         ║");
                Console.WriteLine(" ║         4. Consultar Movimientos   ║");
                Console.WriteLine(" ║         5. Cerrar sesion           ║");
                Console.WriteLine(" ╚════════════════════════════════════╝");
                Console.Write(" ═>");

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
                        Console.WriteLine(" ╔═══════════════════════╗");
                        Console.WriteLine(" ║ Cerrando sesión...    ║");
                        Console.WriteLine(" ╚═══════════════════════╝");
                        break;

                    default:
                        Console.WriteLine(" ╔═══════════════════════╗");
                        Console.WriteLine(" ║ ESTA OPCION NO EXISTE ║");
                        Console.WriteLine(" ╚═══════════════════════╝");
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
                    Console.WriteLine(" ╔═════════════════════════════════════════════════════════════════════╗");
                    Console.WriteLine($"                             {historia}                                ");
                    Console.WriteLine(" ╚═════════════════════════════════════════════════════════════════════╝");

                }
            }
            else
            {
                Console.WriteLine(" ╔═══════════════════════════════════════╗");
                Console.WriteLine(" ║   NO HAY MOVIMIENTOS POR EL MOMENTO   ║");
                Console.WriteLine(" ╚═══════════════════════════════════════╝");
            }
        }

        // Consultar saldo
        private static void ConsultarSaldo(Usuario usuario)
        {
            Console.WriteLine(" ╔═════════════════════════════════════════╗");
            Console.WriteLine($" ║  Actualmente tienes {usuario.Saldo:C}   ");
            Console.WriteLine(" ╚═════════════════════════════════════════╝");
        }

        // Consignar a otra cuenta
        private static void Consignar(Usuario usuario)
        {
            Console.WriteLine(" ╔═══════════════════════════════════════╗");
            Console.WriteLine(" ║     CANTIDAD DE DINERO A ENVIAR       ║");
            Console.WriteLine(" ╚═══════════════════════════════════════╝");
            Console.Write(" ═>");
            int dinero = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(" ╔═══════════════════════════════════════╗");
            Console.WriteLine(" ║    CUENTA DESTINO (Cuenta destino)    ║");
            Console.WriteLine(" ╚═══════════════════════════════════════╝");
            Console.Write(" ═>");
            string numeroCuenta = Console.ReadLine();

            var usuarioReceptor = usuarios.FirstOrDefault(u => u.NumeroCuenta == numeroCuenta);

            if (usuarioReceptor != null)
            {
                Console.WriteLine(" ╔═══════════════════════════════════════════════════════════════╗");
                Console.WriteLine($" ║  Vas a transferir {dinero:C} a la cuenta {numeroCuenta}     ");
                Console.WriteLine(" ║═══════════════════════════════════════════════════════════════║");
                Console.WriteLine(" ║                        ¿Estas seguro?                         ║");
                Console.WriteLine(" ║                            1. Si                              ║");
                Console.WriteLine(" ║                            2. No                              ║");
                Console.WriteLine(" ╚═══════════════════════════════════════════════════════════════╝");
                Console.Write(" ═>");
                int opcion = Convert.ToInt32(Console.ReadLine());

                if (opcion == 1)
                {
                    if (usuario.Saldo >= dinero)
                    {
                        usuario.Saldo -= dinero;
                        usuario.Historial.Add($"{localTime} Transferencia a la cuenta {numeroCuenta} de -{dinero:C}");

                        usuarioReceptor.Saldo += dinero;
                        usuarioReceptor.Historial.Add($"{localTime} Consignación de +{dinero:C}");

                        Console.WriteLine(" ╔══════════════════════════════════════════╗");
                        Console.WriteLine(" ║  La transferencia se realizo con exito   ║");
                        Console.WriteLine(" ╚══════════════════════════════════════════╝");

                        // Guardar cambios en el archivo JSON
                        GuardarUsuarios();
                    }
                    else
                    {
                        Console.WriteLine(" ╔═════════════════════════════════════════════════════╗");
                        Console.WriteLine($" ║  Saldo insuficiente, Tu saldo es {usuario.Saldo}    ");
                        Console.WriteLine(" ╚═════════════════════════════════════════════════════╝");
                    }
                }
                else
                {
                    Console.WriteLine(" ╔════════════════════════════╗");
                    Console.WriteLine(" ║  Cancelando transacción... ║");
                    Console.WriteLine(" ╚════════════════════════════╝");
                }
            }
            else
            {
                Console.WriteLine(" ╔══════════════════════════════════╗");
                Console.WriteLine(" ║  La cuenta de destino no existe. ║");
                Console.WriteLine(" ╚══════════════════════════════════╝");
            }
        }

        // Retirar dinero
        private static void Retirar(Usuario usuario)
        {
            Console.WriteLine(" ╔══════════════════════╗");
            Console.WriteLine(" ║    VALOR A RETIRAR   ║");
            Console.WriteLine(" ╚══════════════════════╝");
            Console.Write(" ═>");
            int valorRetirar = Convert.ToInt32(Console.ReadLine());

            if (usuario.Saldo >= valorRetirar)
            {
                usuario.Saldo -= valorRetirar;
                usuario.Historial.Add($"{localTime} Retiro de -{valorRetirar:C}");
                Console.WriteLine(" ╔═════════════════════════════════╗");
                Console.WriteLine($" ║ Saldo actual: {usuario.Saldo:C} ");
                Console.WriteLine(" ╚═════════════════════════════════╝");

                // Guardar cambios en el archivo JSON
                GuardarUsuarios();
            }
            else
            {
                Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
                Console.WriteLine($"  ║  Saldo insuficiente, Tu saldo es {usuario.Saldo}    ");
                Console.WriteLine("  ╚═════════════════════════════════════════════════════╝");
            }
        }
    }
}


