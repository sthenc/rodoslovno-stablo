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

		#endregion
	}

}
