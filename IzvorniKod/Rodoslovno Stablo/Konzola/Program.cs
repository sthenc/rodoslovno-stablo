using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationLogic;


namespace Konzola
{
	public class Program
	{
		private static ApplicationLogic.QueryProcessor qpro;
		private static ApplicationLogic.UserManager um;

		static void Init()
		{
			qpro = new ApplicationLogic.QueryProcessor(QueryDisambiguator, GetLine);
			um = new ApplicationLogic.UserManager();
		}

		static void PrintStartGreeting()
		{
			System.Console.Out.WriteLine("Dobro došli u aplikaciju Krv nije voda, verzija 0.01");
			System.Console.Out.WriteLine("Kopirajt grupa born2code sa FER-a, najjačeg fakulteta u Zagrebu i šire.\n");
		}

		static void PrintEndGreeting()
		{
			System.Console.Out.WriteLine("\nNadamo se da ste uživali koristeći Krv nije voda, verzija 0.01");
			System.Console.Out.WriteLine("Ako niste onda su krivi svi, samo ne mi.");
			System.Console.In.ReadLine();
		}

		static bool Login()
		{
			// za testiranje
			//return um.Login("admin", "abc123");

            string username, password;

            System.Console.Out.Write("Unesite svoje korisničko ime: ");

            username = System.Console.In.ReadLine();


            // ucitaj password, izvor http://stackoverflow.com/questions/3404421/password-masking-console-application
            password = "";
            Console.Write("Unesite svoju lozinku: ");
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Prekini unos kada korisnik lupi Enter
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();

            return um.Login(username, password);
		}

		static Person QueryDisambiguator(IEnumerable<Person> kandidati, string pitanje = "")
		{
			// TODO resolvanje dvosmislenosti upita
			return kandidati.ElementAt(0);
		}

		static string GetLine()
		{
			return System.Console.ReadLine();
		}
		
		static void MenuLoop()
		{
			while (true)
			{
				System.Console.WriteLine("\nUnesite svoj upit:");

				string query = System.Console.ReadLine();

				try
				{
					qpro.ProcessQuery(query);
				}
				catch (QueryProcessor.QuitException)
				{
					break;
				}
			}
		}

		static void Main(string[] args)
		{
			Init();

            PrintStartGreeting();

            if (Login())
                MenuLoop();

            PrintEndGreeting();
		}
	}
}
