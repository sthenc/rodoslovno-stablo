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
    public class QueryProcessorUtilTest : TreeTestBase
	{
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

			Guid actual = qp.FindPersonByName("Zoro", "Zoric", "");

			// 3. Korak - provjeri pretpostavku
			Assert.AreEqual(expected, actual);
		}
		#endregion
	}

}
