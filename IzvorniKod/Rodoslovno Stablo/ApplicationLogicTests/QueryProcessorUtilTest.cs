using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ApplicationLogic;

namespace ApplicationLogicTests
{

	/// <summary>
	/// Klasa za testriranje Tree-a
	/// izvor http://www.rhyous.com/2012/04/30/beginning-unit-testing-tutorial-in-c-with-nunit/
	/// </summary>
	[TestFixture]
	public class QueryProcessorUtilTest
	{
		private Tree drvo;
		private QueryProcessor qp;

		#region Lažni objekti i funkcije
		Person QueryDisambiguator(List<Person> kandidati)
		{
			// TODO resolvanje dvosmislenosti upita
			return kandidati.ElementAt(0);
		}

		// cirkus koji slijedi sluzi za to da se moze automatizirati korisnicki unos podataka
		private int line_counter = 0;
		private string[] lines = {"bla bla"};

		// samo vraća idući string iz niza, kad ponestane ide iz pocetka
		string GetLine()
		{ 
			int i = line_counter;

			line_counter = (line_counter + 1) % lines.Length;

			return lines[i];
		}

		#endregion

		#region Setup and Tear down
		/// <summary>
		/// Ovo se pokrece samo jednom prije pokretanja svih testova
		/// </summary>
		[TestFixtureSetUp]
		public void InitialSetup()
		{
			drvo = new Tree();
			qp = new QueryProcessor(drvo, QueryDisambiguator, GetLine);
		}

		/// <summary>
		/// Ovo se pokrece samo jednom nakon pokretanja svih testova
		/// </summary>
		[TestFixtureTearDown]
		public void FinalTearDown()
		{

		}

		/// <summary>
		/// Ova setup funkcija se pokrece prije svakog testa
		/// </summary>
		[SetUp]
		public void SetupForEachTest()
		{
			
		}

		/// <summary>
		/// Ova setup funkcija se pokrece nakon svakog testa
		/// </summary>
		[TearDown]
		public void TearDownForEachTest()
		{
			// pobrisi sve
			drvo.osobe.RemoveAll(x => true);
			drvo.veze.RemoveAll(x => true);
		}
		#endregion


		#region Testovi 
		//ovdje se trpaju testovi

		[Test]
		public void NadjiOsobuPoImenuTest()
		{
			// 1. Korak - pripremi objekte
			Guid expected = drvo.AddPerson("Zoro", "Zoric");
			drvo.AddPerson("Zoro", "Zoric");
			drvo.AddPerson("Zoro", "Zoric");

			// 2. Korak - napravi nesto

			Guid actual = qp.NadjiOsobuPoImenu("Zoro", "Zoric");

			// 3. Korak - provjeri pretpostavku
			Assert.AreEqual(expected, actual);
		}
		#endregion
	}

}
