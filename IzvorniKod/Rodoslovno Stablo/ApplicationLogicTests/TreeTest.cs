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
	public class TreeTest : TreeTestBase
	{
		#region Testovi 
		//ovdje se trpaju testovi

		[Test]
		public void AddPerson_Test()
		{
			// 1. Korak - pripremi objekte
			drvo.AddPerson("Zoro", "Zoric");
 
			// 2. Korak - napravi nesto
			string actual = drvo.osobe.Single(x => x.name=="Zoro" && x.surname=="Zoric").name;
 
			// 3. Korak - provjeri pretpostavku
			string expected = "Zoro"; // Ovako je jasno sto se ocekuje
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void AddGetPartner_Test()
		{
			// 1. Korak - pripremi objekte
			Guid zoric = drvo.AddPerson("Zoro", "Zoric");
			Guid hadzi = drvo.AddPerson("Hadzi", "Hadzic");
			drvo.AddPartner(zoric, hadzi);

			// 2. Korak - napravi nesto

			string actual = drvo.veze.Single(x => x.personID1 == zoric && x.personID2 == hadzi).type;

			// 3. Korak - provjeri pretpostavku
			string expected = "partner"; // Ovako je jasno sto se ocekuje
			Assert.AreEqual(expected, actual);


			Guid ac_partner = drvo.GetPartner(hadzi).First();	

			Guid ex_partner = zoric;
			Assert.AreEqual(ex_partner, ac_partner);
		}

		[Test]
		public void DeletePersonWithConnections_Test()
		{
			// 1. Korak - pripremi objekte
			Guid zoric = drvo.AddPerson("Zoro", "Zoric");
			Guid hadzi = drvo.AddPerson("Hadzi", "Hadzic");
			Guid vix = drvo.AddPerson("Vix", "Vixic");

			Assert.AreNotEqual(zoric, hadzi);
			Assert.AreNotEqual(zoric, vix);
			drvo.AddPartner(zoric, hadzi);
			drvo.AddChild(zoric, vix);
			drvo.AddChild(hadzi, vix);

			// 2. Korak - napravi nesto
			drvo.DeletePersonWithConnections(zoric); // RIP kolega

			List<Connection> veze1 = drvo.veze.FindAll(x => x.personID1 == zoric || x.personID2 == zoric);

			Assert.IsEmpty(veze1);

			List<Connection> veze2 = drvo.veze.FindAll(x => x.personID1 == vix || x.personID2 == vix);

			Assert.IsNotEmpty(veze2);

			List<Connection> veze0 = drvo.veze.FindAll(x => x.personID1 == hadzi || x.personID2 == hadzi);

			Assert.IsNotEmpty(veze0);

			//3. Korak - provjeri pretpostavku
			IEnumerable<Guid> ac_parents = drvo.GetParent(vix);

			Assert.IsNotEmpty(ac_parents);


			IEnumerable<Guid> ac_children = drvo.GetChild(hadzi);

			Assert.IsNotEmpty(ac_children);
		}

		#endregion
	}

}
