using System;
using System.IO;
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
            // Da bi mogli uhvatiti rezultate
            // izvor http://msdn.microsoft.com/en-us/library/system.console.readline.aspx
            TextWriter expected = new StreamWriter(Console.OpenStandardOutput());
            TextWriter actual = new StreamWriter(Console.OpenStandardOutput());
            string[][] trazeniDatumi = { new string[] {"1.1.1900.", "1.1.2013."},
                                         new string[] {"1.1.1920.", "1.1.1922."},
                                         new string[] {"1.1.1988.", "1.1.1991."}
                                       };
            Guid[][] ocekivaneOsobe = {osobe, 
                                       new Guid[]{osobe[0]},
                                       new Guid[]{osobe[3], osobe[4]}
                                      };
  
            // Radi + provjeravaj
            for (int i = 0; i < trazeniDatumi.Length; i++)
            {
                Console.SetOut(expected);
                qp.PrintPersons(ocekivaneOsobe[i]);

                Console.SetOut(actual);
                qp.GetAllBornBetween(trazeniDatumi[i]);

                Assert.AreEqual(expected.ToString(), actual.ToString());
            }

        }

        [Test]
        public void Dohvati_sve_umrle_izmedjuTest()
        {

            // 1. Korak - pripremi objekte
            // Da bi mogli uhvatiti rezultate
            // izvor http://msdn.microsoft.com/en-us/library/system.console.readline.aspx
            TextWriter expected = new StreamWriter(Console.OpenStandardOutput());
            TextWriter actual = new StreamWriter(Console.OpenStandardOutput());
            string[][] trazeniDatumi = { new string[] {"1.1.1980.", "1.1.1990."},
                                         new string[] {"1.1.1900.", "1.1.1950."}
                                       };
            Guid[][] ocekivaneOsobe = {new Guid[]{osobe[0]},
                                       new Guid[]{}
                                      };

            // Radi + provjeravaj
            for (int i = 0; i < trazeniDatumi.Length; i++)
            {
                Console.SetOut(expected);
                qp.PrintPersons(ocekivaneOsobe[i]);

                Console.SetOut(actual);
                qp.GetAllDiedBetween(trazeniDatumi[i]);

                Assert.AreEqual(expected.ToString(), actual.ToString());
            }

        }

        [Test]
        public void Dohvati_sve_koji_pozivjese_vise_odTest()
        {

            // 1. Korak - pripremi objekte
            // Da bi mogli uhvatiti rezultate
            // izvor http://msdn.microsoft.com/en-us/library/system.console.readline.aspx
            TextWriter expected = new StreamWriter(Console.OpenStandardOutput());
            TextWriter actual = new StreamWriter(Console.OpenStandardOutput());
            string[][] trazenaDob = { new string[] {"0"},
                                      new string[] {"10"},
                                      new string[] {"50"},
                                      new string[] {"100"}
                                    };
            Guid[][] ocekivaneOsobe = {osobe,
                                       new Guid[]{osobe[0], osobe[1], osobe[2], osobe[3], osobe[4]},
                                       new Guid[]{osobe[0], osobe[1], osobe[2]},
                                       new Guid[]{}
                                      };

            // Radi + provjeravaj
            for (int i = 0; i < trazenaDob.Length; i++)
            {
                Console.SetOut(expected);
                qp.PrintPersons(ocekivaneOsobe[i]);

                Console.SetOut(actual);
                qp.GetAllLivedLonger(trazenaDob[i]);

                Assert.AreEqual(expected.ToString(), actual.ToString());
            }

        }

        [Test]
        public void Dohvati_sve_koji_pozivjese_manje_odTest()
        {

            // 1. Korak - pripremi objekte
            // Da bi mogli uhvatiti rezultate
            // izvor http://msdn.microsoft.com/en-us/library/system.console.readline.aspx
            TextWriter expected = new StreamWriter(Console.OpenStandardOutput());
            TextWriter actual = new StreamWriter(Console.OpenStandardOutput());
            string[][] trazenaDob = { new string[] {"0"},
                                      new string[] {"10"},
                                      new string[] {"50"},
                                      new string[] {"100"}
                                    };
            Guid[][] ocekivaneOsobe = {new Guid[]{},
                                       new Guid[]{osobe[5]},
                                       new Guid[]{osobe[3], osobe[4], osobe[5]},
                                       osobe
                                      };

            // Radi + provjeravaj
            for (int i = 0; i < trazenaDob.Length; i++)
            {
                Console.SetOut(expected);
                qp.PrintPersons(ocekivaneOsobe[i]);

                Console.SetOut(actual);
                qp.GetAllLivedShorter(trazenaDob[i]);

                Assert.AreEqual(expected.ToString(), actual.ToString());
            }

        }

        [Test]
        public void Razlika_u_starostiTest()
        {

            // 1. Korak - pripremi objekte
            // Da bi mogli uhvatiti rezultate
            // izvor http://msdn.microsoft.com/en-us/library/system.console.readline.aspx
            TextWriter expected = new StreamWriter(Console.OpenStandardOutput());
            TextWriter actual = new StreamWriter(Console.OpenStandardOutput());
            string[][] trazenaRazlika = { new string[] {"Pero", "Koza", "Ivo", "Koza"},
                                          new string[] {"Pero", "Koza", "Pero", "Koza"}
                                        };
            string[] ocekivano = {  "Razlika u starosti je 32 godine.",
                                    "Te osobe su isto stare."
                                 };

            // Radi + provjeravaj
            for (int i = 0; i < trazenaRazlika.Length; i++)
            {
                Console.SetOut(expected);
                System.Console.WriteLine(ocekivano[i]);

                Console.SetOut(actual);
                qp.DifferenceInAge(trazenaRazlika[i]);

                Assert.AreEqual(expected.ToString(), actual.ToString());
            }

        }
        [Test]
        public void Broji_Generacije_Test() { 
        
        
        }
        #endregion
    }
}
