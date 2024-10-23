using System;
using System.Collections.Generic;
using System.Linq;

namespace CajeroHDP1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<Usuario> usuarios = new List<Usuario>();

            Usuario usuario1 = new Usuario(1867463, "pepe",
                    "pepe54@gmail.com", "pepe546713", 3000000);

            Usuario usuario2 = new Usuario(389759873, "Sofia",
                    "Sofia76@gmail.com", "peluche4563", 10000);


            usuarios.Add(usuario1);
            usuarios.Add(usuario2);

            MenuSesion(usuarios);


            Console.ReadKey();

        }

        //Inicio de sesion y registro
        private static void MenuSesion(List<Usuario> usuarios)
        {
            int opcion = 0;

            while (opcion != 3)
            {

                Console.WriteLine("1. Iniciar sesion" +
                    "\n2. Registrarse" +
                    "\n3. salir");

                opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        iniciarSesion(usuarios);
                        break;

                    case 2:
                        Registrarse(usuarios);
                        break;

                    case 3:
                        Console.WriteLine("Saliendo del programa");
                        break;

                    default:
                        Console.WriteLine("Esa opción no existe");
                        break;
                }
            }
        }

        // Iniciar sesion
        private static void iniciarSesion(List<Usuario> usuarios)
        {
            int identificacion;
            string contrasenha;

            Console.Write("Identificacion: ");
            identificacion = Convert.ToInt32(Console.ReadLine());

            Console.Write("Contrasenha: ");
            contrasenha = Console.ReadLine();

            foreach (Usuario usuario in usuarios)
            {
                if (usuario.Identificacion == identificacion && usuario.Clave == contrasenha)
                {
                    Console.WriteLine("Usuario validado");
                    Menu(usuario, usuarios);
                }
            }

            Console.WriteLine("Usuario invalido, verifique que ingresaste bien los datos o " +
    "en caso de que no estes registrado te invitamos a que te registres.");
            MenuSesion(usuarios);
        }


        // Registro
        private static void Registrarse(List<Usuario> usuarios)
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

            // Agregar el nuevo usuario
            usuarios.Add(new Usuario(identificacion, nombre, correo, clave, saldo));

            Console.WriteLine("Registro exitoso.");
            MenuSesion(usuarios);
        }

        // Menu principal
        private static void Menu(Usuario usuario, List<Usuario> usuarios)
        {
            int opcion = 0;
            while (opcion != 5)
            {

                Console.WriteLine("1. Retirar" +
                    "\n2. Consignar" +
                    "\n3. Consultar saldo" +
                    "\n4. Consultar Movimientos" +
                    "\n5. Cerrar sesion");

                opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        Retirar(usuario);
                        break;

                    case 2:
                        Consignar(usuario, usuarios);
                        break;

                    case 3:
                        ConsultarSaldo(usuario);
                        break;

                    case 4:
                        ConsultarMovimientos(usuario);
                        break;

                    case 5:
                        MenuSesion(usuarios);
                        break;

                    default:
                        Console.WriteLine("Esa opción no existe");
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

                Console.WriteLine("No hay movimientos por el momento");

            }



        }

        //Consultar saldo
        private static void ConsultarSaldo(Usuario usuario)
        {
            Console.WriteLine($"Actualmente tienes {usuario.Saldo:C}");
        }

        // Consignar a otra cuenta
        private static void Consignar(Usuario usuario, List<Usuario> usuarios)
        {
            Usuario usuarioReceptor;

            int opcion;

            Console.Write("Cantidad de dinero a enviar: ");
            int dinero = Convert.ToInt32(Console.ReadLine());

            Console.Write("Cuenta destino (identificacion): ");
            int identificacion = Convert.ToInt32(Console.ReadLine());


            foreach (Usuario usuario_ in usuarios)
            {
                if (usuario_.Identificacion == identificacion)
                {

                    Console.WriteLine($"Vas a transferir {dinero:C} a la cuenta {identificacion}" +
                    $"\n Estas seguro? 1. Si 2. No");
                    opcion = Convert.ToInt32(Console.ReadLine());

                    while (opcion != 1 && opcion != 2)
                    {

                        Console.WriteLine($"Opcion no valida." +
                            $"\n1. Si 2. No");
                        opcion = Convert.ToInt32(Console.ReadLine());

                    }
                    if (opcion == 1)
                    {

                        if (usuario.Saldo >= dinero)
                        {
                            usuario.Saldo -= dinero;

                            usuario.Historial.Add($"Trasferencia a la cuenta {identificacion} de -{dinero:c}");

                            usuarioReceptor = usuario_;

                            usuarioReceptor.Saldo += dinero;

                            usuarioReceptor.Historial.Add($"Consignacion de +{dinero:c}");

                            Console.WriteLine("La transferencia se realizo con exito");
                        }
                        else
                        {

                            Console.WriteLine($"Saldo insuficiente, Tu saldo es {usuario.Saldo}");

                        }


                    }
                    else if (opcion == 2)
                    {

                    }

                }
            }

        }

        // Retirar
        private static void Retirar(Usuario usuario)
        {
            Console.Write("Valor a retirar: ");
            int valorRetirar = Convert.ToInt32(Console.ReadLine());

            if (usuario.Saldo >= valorRetirar)
            {

                usuario.Saldo -= valorRetirar;

                usuario.Historial.Add($"Retiro de -{valorRetirar:C}");
                Console.WriteLine($"Saldo actual: {usuario.Saldo:C}");

            }
            else
            {

                Console.WriteLine($"Saldo insuficiente, Tu saldo es {usuario.Saldo:C}");

            }
        }
    }
}

