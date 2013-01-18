using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ApplicationLogic;

namespace ApplicationLogicTests
{
    /// <summary>
    /// Klasa za testriranje QueryProcessorAdvancedCommands
    /// izvor http://www.rhyous.com/2012/04/30/beginning-unit-testing-tutorial-in-c-with-nunit/
    /// </summary>
    [TestFixture]
    public class QueryProcessorAdvancedCommandsTest : TreeTestBase
    {
        #region Testovi
        //ovdje se trpaju testovi

        [Test]
        public void parsiraj_datumTest()
        {
            // 1. Korak - pripremi objekte
            DateTime expected1 = new DateTime(2012,11,10);
            DateTime expected2 = new DateTime(201, 11, 10);
            DateTime expected3 = new DateTime(1985, 1, 5);
            DateTime expected4 = new DateTime(1, 1, 1);

            // 2. Korak - napravi nesto
            DateTime actual1 = qp.parsiraj_datum("10,11.2012.");
            DateTime actual2 = qp.parsiraj_datum("10,11,201");
            DateTime actual3 = qp.parsiraj_datum("5.1,1985,");
            DateTime actual4 = qp.parsiraj_datum("1,1.1.");

            // 3. Korak - provjeri pretpostavku
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
            Assert.AreEqual(expected3, actual3);
            Assert.AreEqual(expected4, actual4);
        }

        [Test]
        public void Dohvati_sve_rodjene_izmedjuTest()
        {
            // 1. Korak - pripremi objekte


            // 2. Korak - napravi nesto


            // 3. Korak - provjeri pretpostavku

        }
        #endregion
    }
}
