using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{
	public partial class QueryProcessor
	{
		// TODO popis pametnih upita
		// Mislim da bi trebalo za svaku funkcionalnost napraviti funkciju 
		// koja obavlja logiku i drugu koja parsira ulaz i poziva tu prvu funkciju
		//
		//[GUID] Dohvati_sve_rodjene_izmedju(Datum1, Datum2)
		//[GUID] Dohvati_sve_umrle_izmedju(Datum1, Datum2)
		//[GUID] Dohvati_sve_koji_pozivjese_vise_od(broj_godina)
		//[GUID] Dohvati_sve_koji_pozivjese_manje_od(broj_godina)
		//broj Razlika_u_starosti(Osoba1, Osoba2)
		//broj Generacija_izmedju(Osoba1, Osoba2)
        //broj U_kojem_koljenu(Osoba1, Osoba2)

        private DateTime parsiraj_datum(string datum)
        {
            int dan = 1;
            int mjesec = 1;
            int godina = 1000;
            string[] dmg = datum.Replace(",", ".").Split('.');

            if (dmg.Length != 3)
                throw new System.ArgumentException("Datum nije u pravilnom obliku.", datum);

            //dohvati dan
            try
            {
                dan = Int32.Parse(dmg[0]);
                if (dan < 1 || dan > 31)
                {
                    dan = 1;
                }
            }
            catch(Exception)
            {
                throw new System.ArgumentException("Pogrešan datum: dan {1}", dmg[0]);
            }
            //dohvati mjesec
            try
            {
                mjesec = Int32.Parse(dmg[1]);
                if (mjesec < 1 || mjesec > 12)
                {
                    mjesec = 1;
                }
            }
            catch (Exception)
            {
                throw new System.ArgumentException("Pogrešan datum: mjesec {1}", dmg[1]);
            }
            //dohvati godina
            try
            {
                godina = Int32.Parse(dmg[2]);
                if (godina < 0)
                {
                    godina = 1;
                }
            }
            catch (Exception)
            {
                throw new System.ArgumentException("Pogrešan datum: godina {1}", dmg[2]);
            }

            return new DateTime(godina, mjesec, dan);

        }

        public void Dohvati_sve_rodjene_izmedju(string[] parametri)
        {
            if (parametri.Length != 2)
                throw new System.ArgumentException();

            DateTime datum1 = parsiraj_datum(parametri[0]);
            DateTime datum2 = parsiraj_datum(parametri[1]);
               
            List<Person> osobeRodjeneIzmedju = Drvo.osobe.FindAll(os => 
                DateTime.Compare(datum1, os.birthDate) <= 0 &&
                DateTime.Compare(os.birthDate, datum2) <= 0
                );
            List<Guid> osobeGuid = new List<Guid>();
            foreach (var os in osobeRodjeneIzmedju)
                osobeGuid.Add(os.ID);

            PrintPersons(osobeGuid);            
            
        }

        public void Dohvati_sve_umrle_izmedju(string[] parametri)
        {
            if (parametri.Length != 2)
                throw new System.ArgumentException();

            DateTime datum1 = parsiraj_datum(parametri[0]);
            DateTime datum2 = parsiraj_datum(parametri[1]);

            List<Person> osobeRodjeneIzmedju = Drvo.osobe.FindAll(os =>
                DateTime.Compare(datum1, os.deathDate) <= 0 &&
                DateTime.Compare(os.deathDate, datum2) <= 0
                );
            List<Guid> osobeGuid = new List<Guid>();
            foreach (var os in osobeRodjeneIzmedju)
                osobeGuid.Add(os.ID);

            PrintPersons(osobeGuid);
        }

        public void Dohvati_sve_koji_pozivjese_vise_od(string[] parametri)
        {
            if (parametri.Length != 1)
                throw new System.ArgumentException();

            int godineProzivjeli;

            try
            {
                godineProzivjeli = Int32.Parse(parametri[0]);
            }
            catch (Exception)
            {
                throw new System.ArgumentException("Pogrešan broj godina: {1}", parametri[0]);
            }

            List<Person> osobeProzivjele = Drvo.osobe.FindAll(os => (DateTime.Today.Year - os.birthDate.Year) >= godineProzivjeli);
            List<Guid> osobeGuid = new List<Guid>();
            foreach (var os in osobeProzivjele)
                osobeGuid.Add(os.ID);

            PrintPersons(osobeGuid);
        }

        public void Dohvati_sve_koji_pozivjese_manje_od(string[] parametri)
        {
            if (parametri.Length != 1)
                throw new System.ArgumentException();

            int godineProzivjeli;

            try
            {
                godineProzivjeli = Int32.Parse(parametri[0]);
            }
            catch (Exception)
            {
                throw new System.ArgumentException("Pogrešan broj godina: {1}", parametri[0]);
            }

            List<Person> osobeProzivjele = Drvo.osobe.FindAll(os => (DateTime.Today.Year - os.birthDate.Year) <= godineProzivjeli);
            List<Guid> osobeGuid = new List<Guid>();
            foreach (var os in osobeProzivjele)
                osobeGuid.Add(os.ID);

            PrintPersons(osobeGuid);
        }

        public void Razlika_u_starosti(string[] parametri)
        {
            if (parametri.Length != 4)
                throw new System.ArgumentException();

            Person osoba1, osoba2;
            Person stariji, mladji;
            osoba1 = Drvo.GetPersonByID(FindPersonByName(parametri[0], parametri[1]));
            osoba2 = Drvo.GetPersonByID(FindPersonByName(parametri[2], parametri[3]));

            TimeSpan starostOsoba1 = DateTime.Now - osoba1.birthDate;
            TimeSpan starostOsoba2 = DateTime.Now - osoba2.birthDate;
          
            if(TimeSpan.Compare(starostOsoba1, starostOsoba2) < 0)
            {
                stariji = osoba2;
                mladji = osoba1;
            }
            else
            {
                stariji = osoba1;
                mladji = osoba2;
            }

            int razlika;
            System.Console.Write("Razlika u starosti je ");
            if ((razlika = stariji.birthDate.Year - mladji.birthDate.Year) > 0)
            {                
                System.Console.Write("{1}", razlika);
                if((razlika % 10 == 2 || razlika % 10 == 3 || razlika % 10 == 4) && !(razlika > 10 && razlika < 20))
                    System.Console.Write(" godine.");
                else
                    System.Console.Write(" godina.");
            }
            else if ((razlika = stariji.birthDate.Month - mladji.birthDate.Month) > 0)
            {
                System.Console.Write("{1}", razlika);
                if(razlika == 1)
                    System.Console.Write(" mjesec.");
                else if (razlika == 2 || razlika == 3 || razlika  == 4)
                    System.Console.Write(" mjeseca.");
                else
                    System.Console.Write(" mjeseci.");
            }
            else if ((razlika = stariji.birthDate.Day - mladji.birthDate.Day) > 0)
            {
                System.Console.Write("{1}", razlika);
                if (razlika == 1)
                    System.Console.Write(" dan.");
                else 
                    System.Console.Write(" dana.");
            }
            else
                System.Console.Write("Te osobe su isto stare.");

        }

        public void Generacija_izmedju(string[] parametri)
        {
            if (parametri.Length != 4)
                throw new System.ArgumentException();

            Person osoba1, osoba2;
            Person stariji, mladji;
            osoba1 = Drvo.GetPersonByID(FindPersonByName(parametri[0], parametri[1]));
            osoba2 = Drvo.GetPersonByID(FindPersonByName(parametri[2], parametri[3]));

            int razlika = osoba1.birthDate.Year - osoba2.birthDate.Year;
            if (razlika > 0)
            {
                stariji = osoba2;
                mladji = osoba1;
            }
            else if (razlika < 0)
            {
                stariji = osoba1;
                mladji = osoba2;
            }
            else
            {

            }


            int generacijaIzmedju = 0;
            
        }

        public void U_kojem_koljenu(string[] parametri)
        {

        }
    }
}
