using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ApplicationLogic;

namespace ApplicationLogicTests
{
    /// <summary>
    /// Klasa koja sadrži gotovo drvo za testiranje
    /// izvor http://www.rhyous.com/2012/04/30/beginning-unit-testing-tutorial-in-c-with-nunit/
    /// </summary>
    [TestFixture]
    public class TreeTestBase
    {
        protected Tree drvo;
        protected QueryProcessor qp;
        protected Guid[] osobe;

        // automatizirati korisnicki unos podataka
        private int line_counter = 0;
        private string[] lines = { "bla bla" };

        #region Lažne funkcije
        Person QueryDisambiguator(IEnumerable<Person> kandidati, string msg = "")
        {
            // TODO resolvanje dvosmislenosti upita
            return kandidati.ElementAt(0);
        }

        // samo vraća idući string iz niza, kad ponestane ide iz pocetka
        string GetLine()
        {
            int i = line_counter;
            line_counter = (line_counter + 1) % lines.Length;

            return lines[i];
        }

        void izgradiDrvo()
        {
            osobe = new Guid[]{drvo.AddPerson("Pero", "Koza", new DateTime(1921, 3, 29), new DateTime(1984, 11, 8)),
                    drvo.AddPerson("Ivo", "Koza", new DateTime(1953, 8, 25)),
                    drvo.AddPerson("Mara", "Koza", new DateTime(1955, 3, 2)),
                    drvo.AddPerson("Darko", "Koza", new DateTime(1989, 10, 1)),
                    drvo.AddPerson("Bara", "Vučemilović", new DateTime(1990, 5, 18)),      
                    drvo.AddPerson("Milutin", "Koza", new DateTime(2012, 5, 15))
                    };

            drvo.AddParent(osobe[1], osobe[0]);
            drvo.AddParent(osobe[3], osobe[1]);
            drvo.AddParent(osobe[3], osobe[2]);
            drvo.AddParent(osobe[5], osobe[3]);
            drvo.AddParent(osobe[5], osobe[4]);

            drvo.AddPartner(osobe[1], osobe[2]);
            drvo.AddPartner(osobe[3], osobe[4]);
        }

        void srusiDrvo()
        {
            // pobrisi sve
            drvo.osobe.RemoveAll(x => true);
            drvo.veze.RemoveAll(x => true);
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
            drvo = Tree.GetInstance();
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
            izgradiDrvo();
        }

        /// <summary>
        /// Ova setup funkcija se pokrece nakon svakog testa
        /// </summary>
        [TearDown]
        public void TearDownForEachTest()
        {
            srusiDrvo();
        }
        #endregion
    }
}
