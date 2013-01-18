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
    public class TreeXMLTest : TreeTestBase
	{
		
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
