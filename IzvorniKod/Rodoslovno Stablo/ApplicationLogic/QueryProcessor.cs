using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ApplicationLogic
{
	public partial class QueryProcessor
	{
		//private Tree Drvo;
		// za potrebe testiranja
		private Tree Drvo;

		// Podesiva funkcija za odlucivanje o kojoj osobi se radi kada je upit dvosmislen
		private Func<IEnumerable<Person>, string, Person> QueryDisambiguator;

		// Podesiva funkcija za dohvacanje korisnikovog unosa
		// ovo je jedina funkcija koju mozemo koristiti za to
		private Func<string> GetLine;

		public QueryProcessor(Func<IEnumerable<Person>, string, Person> QD, 
								Func<string> daj_liniju, TextWriter tw = null)
		{
            Drvo = Tree.GetInstance();

			// dodajmo malo couplinga
			QueryDisambiguator = QD;
			GetLine = daj_liniju;

			// preusmjeri System.Console.Out tamo gdje nam odgovara da mozemo koristiti System.Console.WriteLine
			if (tw != null)		// Da omogucimo ovo http://saezndaree.wordpress.com/2009/03/29/how-to-redirect-the-consoles-output-to-a-textbox-in-c/
				System.Console.SetOut(tw);

			InitializeCommands();
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

		public void InitializeCommands()
		{
			komande = new List<CommandDescriptor>();

            // pojedinac
			komande.Add(new CommandDescriptor("dodaj_osobu", AddPerson, "dodaj_osobu ime, prezime"));
			komande.Add(new CommandDescriptor("nadji_osobu", GetPerson, "nadji_osobu ime, prezime\n"));

            // muzevi, zene
            komande.Add(new CommandDescriptor("dodaj_supruznika", AddSpouse, "dodaj_supruznika ime_osoba, prezime_osoba, ime_supruznik, prezime_supruznik"));
            komande.Add(new CommandDescriptor("dohvati_supruznika", GetSpouse, "dohvati_supruznika ime_osoba, prezime_osoba\n"));

            // djeca
            komande.Add(new CommandDescriptor("dodaj_dijete", AddChild, "dodaj_dijete ime_osoba, prezime_osoba, ime_dijete, prezime_dijete"));
            komande.Add(new CommandDescriptor("dohvati_djecu", GetChild, "dohvati_djecu ime_osoba, prezime_osoba\n"));

            // roditelji
            komande.Add(new CommandDescriptor("dodaj_roditelja", AddParent, "dodaj_roditelja ime_osoba, prezime_osoba, ime_roditelj, prezime_roditelj"));
            komande.Add(new CommandDescriptor("dohvati_roditelje", GetParent, "dohvati_roditelje ime_osoba, prezime_osoba\n"));

            //braca i sestre
			komande.Add(new CommandDescriptor("nadji_bracu", GetBrother, "nadji_bracu ime_osoba, prezime_osoba"));
			komande.Add(new CommandDescriptor("nadji_sestre", GetSister, "nadji_djedove ime_osoba, prezime_osoba"));
			komande.Add(new CommandDescriptor("nadji_bracu_i_sestre", GetUnknownSibling, "nadji_bracu_i_sestre ime_osoba, prezime_osoba"));
			komande.Add(new CommandDescriptor("dodaj_brata", AddBrother, "dodaj_brata ime_osoba, prezime_osoba, ime_brat, prezime_brat"));
			komande.Add(new CommandDescriptor("dodaj_sestru", AddSister, "dodaj_sestru ime_unuk, prezime_unuk, ime_brat, prezime_brat"));
			komande.Add(new CommandDescriptor("dodaj_brata_ili_sestru", AddUnknownSibling, "dodaj_brata_ili_sestru ime_osoba, prezime_osoba, ime_brat, prezime_brat\n"));

            //bake i djedovi
            komande.Add(new CommandDescriptor("dodaj_baku", AddGrandmother, "dodaj_baku ime_unuk, prezime_unuk, ime, prezime"));
            komande.Add(new CommandDescriptor("dodaj_djeda", AddGrandfather, "dodaj_djeda ime_unuk, prezime_unuk, ime, prezime"));
            komande.Add(new CommandDescriptor("dodaj_praroditelja", AddGrandparent, "dodaj_praroditelja ime_unuk, prezime_unuk, ime, prezime"));
            komande.Add(new CommandDescriptor("nadji_bake", GetGrandmother, "nadji_bake ime_unuk, prezime_unuk"));
            komande.Add(new CommandDescriptor("nadji_djedove", GetGrandfather, "nadji_djedove ime_unuk, prezime_unuk"));
            komande.Add(new CommandDescriptor("nadji_praroditelje", GetGrandparent, "nadji_praroditelje ime_unuk, prezime_unuk\n"));

            // pametni upiti
            komande.Add(new CommandDescriptor("broji_generacije", CountGenerations, "broji_generacije ime_osoba, prezime_osoba, ime_druga_osoba, prezime_druga_osoba"));
            komande.Add(new CommandDescriptor("broj_koljena", NumberOfKneesBetween, "broj_koljena ime1, prezime1, ime2, prezime2"));
            komande.Add(new CommandDescriptor("rodjeni_izmedju", GetAllBornBetween, "rodjeni_izmedju datum1, datum2"));
            komande.Add(new CommandDescriptor("umrli_izmedju", GetAllDiedBetween, "umrli_izmedju datum1, datum2"));
            komande.Add(new CommandDescriptor("zivjeli_duze", GetAllLivedLonger, "zivjeli_duze brojGodina"));
            komande.Add(new CommandDescriptor("zivjeli_krace", GetAllLivedShorter, "zivjeli_krace brojGodina"));
            komande.Add(new CommandDescriptor("razlika_u_starosti", DifferenceInAge, "razlika_u_starosti ime1, prezime1, ime2, prezime2\n"));
	        
            // ostalo, za rad sa podacima
            komande.Add(new CommandDescriptor("promijeni_podatke", ChangeData, "promijeni_podatke ime, prezime"));
            komande.Add(new CommandDescriptor("ispisi_stablo", PrintTree, "ispisi_stablo"));
            komande.Add(new CommandDescriptor("ispisi_osobu", PrintPerson, "ispisi_osobu ime, prezime"));
			komande.Add(new CommandDescriptor("izlaz", Quit, "izlaz\n"));            
			// TODO popis funkcija
		}
	}
}
