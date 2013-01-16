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
	public class TreeXMLTest
	{
		private Tree drvo;
		private QueryProcessor qp;

		#region Lažni objekti i funkcije
		Person QueryDisambiguator(IEnumerable<Person> kandidati, string msg = "")
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
			qp = new QueryProcessor(QueryDisambiguator, GetLine);
			drvo = qp.Drvo;
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


		// ovo ne radi, unit testovi nemaju pristup datotecnom sustavu
		[Test]
		public void TreeXMLLoadSaveTest()
		{
			// 1. Korak - pripremi objekte
			Guid zoro1 = drvo.AddPerson("Zoro", "Zoric");
			Guid zoro2 = drvo.AddPerson("Zoro", "Zoric2");
			Guid zoro3 = drvo.AddPerson("Zoro", "Zoric3");
			drvo.AddParent(zoro1, zoro2);
			drvo.AddParent(zoro2, zoro3);
			Guid zena = drvo.AddPerson("Zorica", "Zoric3");
			drvo.AddParent(zoro3, zena);

			string data = drvo.SaveTest();


			// 2. Korak - napravi nesto

			Tree loadano = Tree.LoadTest(data);

			// 3. Korak - provjeri pretpostavku
			Assert.AreEqual(drvo.osobe, loadano.osobe);
			Assert.AreEqual(drvo.veze, loadano.veze);
		}
		#endregion
	}

}
