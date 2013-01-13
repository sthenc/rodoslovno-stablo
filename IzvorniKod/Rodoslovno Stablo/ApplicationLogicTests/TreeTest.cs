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
	public class TreeTest
	{
		private Tree Drvo;

		#region Setup and Tear down
		/// <summary>
		/// Ovo se pokrece samo jednom prije pokretanja svih testova
		/// </summary>
		[TestFixtureSetUp]
		public void InitialSetup()
		{
			Drvo = new Tree();
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
			Drvo.osobe.RemoveAll(x => true);
			Drvo.veze.RemoveAll(x => true);
		}
		#endregion


		#region Testovi 
		//ovdje se trpaju testovi

		/// <summary>
		/// This setup functions runs after each test method
		/// </summary>
		[Test]
		public void AddPerson_Test()
		{
			// 1. Korak - pripremi objekte
			Drvo.AddPerson("Zoro", "Zoric");
 
			// 2. Korak - napravi nesto
			string actual = Drvo.osobe.Single(x => true).personName;
 
			// 3. Korak - provjeri pretpostavku
			string expected = "Zoro"; // Ovako je jasno sto se ocekuje
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void AddGetPartner_Test()
		{
			// 1. Korak - pripremi objekte
			Guid zoric = Drvo.AddPerson("Zoro", "Zoric");
			Guid hadzi = Drvo.AddPerson("Hadzi", "Hadzic");
			Drvo.AddPartner(zoric, hadzi);

			// 2. Korak - napravi nesto

			string actual = Drvo.veze.Single(x => x.personID1 == zoric && x.personID2 == hadzi).type;

			// 3. Korak - provjeri pretpostavku
			string expected = "partner"; // Ovako je jasno sto se ocekuje
			Assert.AreEqual(expected, actual);


			Guid ac_partner = Drvo.GetPartner(hadzi).First();	

			Guid ex_partner = zoric;
			Assert.AreEqual(ex_partner, ac_partner);
		}

		[Test]
		public void DeletePersonAlsoDeleteConnections_Test()
		{
			// 1. Korak - pripremi objekte
			Guid zoric = Drvo.AddPerson("Zoro", "Zoric");
			Guid hadzi = Drvo.AddPerson("Hadzi", "Hadzic");
			Guid vix = Drvo.AddPerson("Vix", "Vixic");

			Assert.AreNotEqual(zoric, hadzi);
			Assert.AreNotEqual(zoric, vix);
			Drvo.AddPartner(zoric, hadzi);
			Drvo.AddChild(zoric, vix);
			Drvo.AddChild(hadzi, vix);

			// 2. Korak - napravi nesto
			Drvo.DeletePerson(zoric); // RIP kolega

			List<Connection> veze1 = Drvo.veze.FindAll(x => x.personID1 == zoric || x.personID2 == zoric);

			Assert.IsEmpty(veze1);

			List<Connection> veze2 = Drvo.veze.FindAll(x => x.personID1 == vix || x.personID2 == vix);

			Assert.IsNotEmpty(veze2);

			List<Connection> veze0 = Drvo.veze.FindAll(x => x.personID1 == hadzi || x.personID2 == hadzi);

			Assert.IsNotEmpty(veze0);

			//3. Korak - provjeri pretpostavku
			Guid[] ac_parents = Drvo.GetParent(vix);

			Assert.IsNotEmpty(ac_parents);


			Guid[] ac_children = Drvo.GetChild(hadzi);

			Assert.IsNotEmpty(ac_children);
		}

		#endregion
	}

}
