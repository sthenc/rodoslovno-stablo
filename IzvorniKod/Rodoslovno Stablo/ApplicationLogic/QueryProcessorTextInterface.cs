using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{
	public partial class QueryProcessor
	{
		public void ProcessQuery(string input)
		{
			// Parsiranje upita
			// format upita je  "upit argument1, argument2,argument3,    argument4"

			// prvo razdvoji argumente od imena upita
			char[] delimiters = new char[] { ' ', '\t', '\n' };

			input += "\n"; // za upite bez argumenata

			int index = input.IndexOfAny(delimiters);

			string upit = input.Substring(0, index).Trim();

			string argumenti = input.Substring(index).Trim();

			// onda razdvoji pojedinacne argumente
			string[] args = argumenti.Split(',');

			// izbaci spejseve sa pocetka i kraja
			for (int i = 0; i < args.Length; ++i)
			{
				args[i] = args[i].Trim();
			}

			bool komanda_nadjena = false;
			bool upit_dobar = false;
			foreach (var cd in komande)
			{
				// nadji odgovarajucu funkciju
				if (cd.keywords.Exists(x => x.Equals(upit)))
				{
					komanda_nadjena = true;
					// i pozovi ju
					try
					{
						cd.func(args);
					}
					catch (System.InvalidOperationException e)
					{
						System.Console.WriteLine("Nemoguća operacija: {0}\n", e.Message);
						break;
					}
					catch (System.ArgumentException e)
					{
						System.Console.WriteLine("Pogrešno oblikovan upit: {0}\n\nIspravno korištenje:\n {1}\n",
													e.Message, cd.description);
						break;
					}

					upit_dobar = true;
				}
			}

			if (!komanda_nadjena)
			{
				PrintCommandDescriptions();
			}

			if (upit_dobar)
			{ 
				// TODO spremi upit u bazu
			}
		}


		private void PrintCommandDescriptions()
		{
			System.Console.WriteLine("Program podržava slijedeće upite: \n");

			foreach (var cd in komande)
			{
				System.Console.WriteLine(cd.description);
			}
		}
	}
}
