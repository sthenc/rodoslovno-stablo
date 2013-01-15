using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ApplicationLogic
{
	public partial class QueryProcessor
	{
		private Tree Drvo;
		
		// Podesiva funkcija za odlucivanje o kojoj osobi se radi kada je upit dvosmislen
		private Func<List<Person>, string, Person> QueryDisambiguator;

		// Podesiva funkcija za dohvacanje korisnikovog unosa
		// ovo je jedina funkcija koju mozemo koristiti za to
		private Func<string> GetLine;

		public QueryProcessor(Tree drvo, Func<List<Person>, string, Person> QD, 
								Func<string> daj_liniju, TextWriter tw = null)
		{
			// dodajmo malo couplinga
			Drvo = drvo;
			QueryDisambiguator = QD;
			GetLine = daj_liniju;

			// preusmjeri System.Console.Out tamo gdje nam odgovara da mozemo koristiti System.Console.WriteLine
			if (tw != null)		// Da omogucimo ovo http://saezndaree.wordpress.com/2009/03/29/how-to-redirect-the-consoles-output-to-a-textbox-in-c/
				System.Console.SetOut(tw);

			Initialize_Commands();
		}

		// tablica sa mapiranjem komandi i kljucnih rijeci, zajedno sa opisima
		private List<CommandDescriptor> komande;

		private class CommandDescriptor
		{
			public List<string> keywords { get; set; }
			public string description { get; set; }
			public Action<string[]> func { get; set;} 

			public CommandDescriptor(string[] words, Action<string[]> funk, string desc = null)
			{
				Initialize(words, funk, desc);
			}

			public CommandDescriptor(string word, Action<string[]> funk, string desc = null) 
			{
				Initialize(new string[] { word }, funk, desc);
			}
			
			public void Initialize(string[] words, Action<string[]> funk, string desc)
			{ 
				keywords = new List<string>();
				keywords.AddRange(words);

				func = funk;
				description = desc;
			}
		}

		public void Initialize_Commands()
		{
			komande = new List<CommandDescriptor>();
			komande.Add(new CommandDescriptor("dodaj_baku", DodajBaku, "dodaj_baku ime_unuk, prezime_unuk, ime, prezime"));
			komande.Add(new CommandDescriptor("promijeni_podatke", PromijeniPodatke, "promijeni_podatke ime, prezime"));
			komande.Add(new CommandDescriptor("izlaz", Izlaz));
			komande.Add(new CommandDescriptor("ispisi_drvo", IspisiDrvo));
			// TODO popis funkcija
		}
	}
}
