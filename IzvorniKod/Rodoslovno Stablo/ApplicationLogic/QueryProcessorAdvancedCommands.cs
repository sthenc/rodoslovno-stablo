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
            }
            catch(Exception e)
            {
                throw new System.ArgumentException("Pogrešan datum: dan {1}", dmg[0]);
            }
            //dohvati mjesec
            try
            {
                mjesec = Int32.Parse(dmg[1]);
            }
            catch (Exception e)
            {
                throw new System.ArgumentException("Pogrešan datum: mjesec {1}", dmg[1]);
            }
            //dohvati godina
            try
            {
                godina = Int32.Parse(dmg[2]);
            }
            catch (Exception e)
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
                DateTime.Compare(datum1, os.birthDate) <= 0 &&
                DateTime.Compare(os.birthDate, datum2) <= 0
                );
            List<Guid> osobeGuid = new List<Guid>();
            foreach (var os in osobeRodjeneIzmedju)
                osobeGuid.Add(os.ID);

            PrintPersons(osobeGuid);
        }
	
	}
}
