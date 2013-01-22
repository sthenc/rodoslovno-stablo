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

        public readonly static DateTime nedefiniranDatum = new DateTime(1000, 1, 1);

        public DateTime parsiraj_datum(string datum)
        {
            int dan = nedefiniranDatum.Day;
            int mjesec = nedefiniranDatum.Month;
            int godina = nedefiniranDatum.Year;
            string[] dmg = datum.Replace(",", ".").Split('.');

            if (dmg.Length < 3)
                throw new System.ArgumentException("Datum nije u pravilnom obliku.", datum);

            //dohvati dan
            try
            {
                dan = Int32.Parse(dmg[0]);
                if (dan < 1 || dan > 31)
                {
                    dan = nedefiniranDatum.Day;
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
                    mjesec = nedefiniranDatum.Month;
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
                    godina = nedefiniranDatum.Year;
                }
            }
            catch (Exception)
            {
                throw new System.ArgumentException("Pogrešan datum: godina {1}", dmg[2]);
            }

            return new DateTime(godina, mjesec, dan);

        }

        public void GetAllBornBetween(string[] parametri)
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

        public void GetAllDiedBetween(string[] parametri)
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

        public void GetAllLivedLonger(string[] parametri)
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

            List<Person> osobeProzivjele = Drvo.osobe.FindAll(os => (os.birthDate != nedefiniranDatum) && ((DateTime.Today.Year - os.birthDate.Year) >= godineProzivjeli));
            List<Guid> osobeGuid = new List<Guid>();
            foreach (var os in osobeProzivjele)
                osobeGuid.Add(os.ID);

            PrintPersons(osobeGuid);
        }

        public void GetAllLivedShorter(string[] parametri)
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

            List<Person> osobeProzivjele = Drvo.osobe.FindAll(os => (os.birthDate != nedefiniranDatum) &&  ((DateTime.Today.Year - os.birthDate.Year) <= godineProzivjeli));
            List<Guid> osobeGuid = new List<Guid>();
            foreach (var os in osobeProzivjele)
                osobeGuid.Add(os.ID);

            PrintPersons(osobeGuid);
        }
        public void CountGenerations(string [] parametri) {
            if (parametri.Length != 4) throw new System.ArgumentException();
            Person osoba1, osoba2, pointer;
            int distance,result=-1;
            
            List<Pair<Person, int> > queue= new List<Pair<Person,int>>();
            List<Person> visited = new List<Person>();

            osoba1 = Drvo.GetPersonByID(FindPersonByName(parametri[0], parametri[1]));
            osoba2= Drvo.GetPersonByID(FindPersonByName(parametri[2],parametri[3]));

            visited.Add(osoba1);
            queue.Add( new Pair<Person,int>(osoba1,0));
            
            
            while ( queue.Count!=0){
                pointer = queue.Last().First;
                distance = queue.Last().Second;
                queue.Remove(queue.Last());
                visited.Add(pointer);
                if (osoba2.Equals(pointer)){
                    // kraj, pronasli smo odgovor
                    result=Math.Abs(distance);
                    break;

                }
                // sredimo svu djecu
                List <Person> djeca = Drvo.GetChildren(pointer.ID).ToList();
                foreach (Person i in djeca){
                    if (visited.Find(x => x==i)==null){
                        
                        queue.Add(new Pair<Person, int>(i,distance+1));

                    }   
                }
                 // sredimo sve bracne drugove
                List <Person> brak = Drvo.GetPartners(pointer.ID).ToList();
                foreach (Person i in brak){
                    if (visited.Find(x => x==i)==null){
                       
                        queue.Add(new Pair<Person, int>(i,distance+0));

                    }   
                }
                
                 // sredimo sve roditelje
                List <Person> roditelji = Drvo.GetParents(pointer.ID).ToList();
                foreach (Person i in roditelji){
                    if (visited.Find(x => x==i)==null){
                       
                        queue.Add(new Pair<Person, int>(i,distance-1));

                    }   
                }
               


            
            }
            if (result == -1)
                System.Console.WriteLine("Osobe nisu u istom stablu. Provjerite imate li bipartitni graf");
            else
                System.Console.WriteLine("Broj generacija između osoba jest " + result);

            

        
        }
    
        public void DifferenceInAge(string[] parametri)
        {
            if (parametri.Length != 4)
                throw new System.ArgumentException();

            Person osoba1, osoba2;
            Person stariji, mladji;
            osoba1 = Drvo.GetPersonByID(FindPersonByName(parametri[0], parametri[1]));
            osoba2 = Drvo.GetPersonByID(FindPersonByName(parametri[2], parametri[3]));

            if (osoba1.birthDate == nedefiniranDatum)
            {
                System.Console.WriteLine("Datum rodjena nije definiran za {1} {2}.", parametri[0], parametri[1]);
                return;
            }
            if (osoba2.birthDate == nedefiniranDatum)
            {
                System.Console.WriteLine("Datum rodjena nije definiran za {1} {2}.", parametri[2], parametri[3]);
                return;
            }

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
            string ispis = "Razlika u starosti je ";
            if ((razlika = stariji.birthDate.Year - mladji.birthDate.Year) > 0)
            {
                ispis += razlika.ToString();
                if((razlika % 10 == 2 || razlika % 10 == 3 || razlika % 10 == 4) && !(razlika > 10 && razlika < 20))
                    ispis += " godine.";
                else
                    ispis +=" godina.";
            }
            else if ((razlika = stariji.birthDate.Month - mladji.birthDate.Month) > 0)
            {
                ispis += razlika.ToString();
                if(razlika == 1)
                    ispis += " mjesec.";
                else if (razlika == 2 || razlika == 3 || razlika  == 4)
                    ispis += " mjeseca.";
                else
                    ispis += " mjeseci.";
            }
            else if ((razlika = stariji.birthDate.Day - mladji.birthDate.Day) > 0)
            {
                ispis += razlika.ToString();
                if (razlika == 1)
                    ispis += " dan.";
                else
                    ispis += " dana.";
            }
            else
            {
                ispis = "Te osobe su isto stare.";
            }

            System.Console.WriteLine(ispis);

        }

        public class Pair<T, U>
        {
            public Pair()
            {
            }

            public Pair(T first, U second)
            {
                this.First = first;
                this.Second = second;
            }

            public T First { get; set; }
            public U Second { get; set; }

            public override bool Equals(System.Object obj)
            {
                // ako je null nije jednako
                if (obj == null)
                {
                    return false;
                }

                // Ako se ne moze kastati sigurno nije dobro
                Pair<T, U> p = obj as Pair<T, U>;
                if ((System.Object)p == null)
                {
                    return false;
                }

                return this.Equals(p);
            }

            public bool Equals(Pair<T, U> p)
            {
                return this.First.Equals(p.First);
            }
        };

        public void TraverseAncestors(HashSet<Pair<Guid, int>> store, Pair<Guid, int> start)
        {
            var red = new Queue<Pair<Guid, int>>();

            red.Enqueue(start);

            while (red.Count() > 0)
            {
                var aktualni = red.Dequeue();

                if (!store.Contains(aktualni))
                {
                    store.Add(aktualni);

                    IEnumerable<Guid> roditelji = Drvo.GetParent(aktualni.First);

                    foreach (var rod in roditelji)
                    {
                        red.Enqueue(new Pair<Guid, int>(rod, aktualni.Second + 1));
                    }
                }
            }
        }

        // http://www.forum.hr/archive/index.php/t-242082.html
        public void NumberOfKneesBetween(string[] parametri)
        {
            if (parametri.Length != 4)
                throw new System.ArgumentException();

            Person osoba1, osoba2;
            osoba1 = Drvo.GetPersonByID(FindPersonByName(parametri[0], parametri[1]));
            osoba2 = Drvo.GetPersonByID(FindPersonByName(parametri[2], parametri[3]));

            var obisao1 = new HashSet<Pair<Guid,int>>();
            var obisao2 = new HashSet<Pair<Guid,int>>();

            //System.Console.WriteLine("Do ovdje je OK");

            TraverseAncestors(obisao1, new Pair<Guid, int>(osoba1.ID, 0));
            TraverseAncestors(obisao2, new Pair<Guid, int>(osoba2.ID, 0));

            int koljena = int.MaxValue;

            //System.Console.WriteLine("Do ovdje je OK");

          
            foreach (var o1 in obisao1)
            {
                //System.Console.WriteLine("{0}", i);

                Pair<Guid, int> o2;

                try
                {
                    o2 = obisao2.First(x => x.First == o1.First) ?? null;
                }
                catch (Exception)
                {
                    continue;
                }

                koljena = Math.Min(Math.Max(o1.Second, o2.Second), koljena);

                if ((o1.Second == 0) || (o2.Second == 0))
                    koljena = 1;
            }


            if (koljena == int.MaxValue)
                System.Console.WriteLine("Ove dvije osobe nisu u rodu.");
            else if (koljena == 1)
                System.Console.WriteLine("Ove dvije osobe su u neposrednom krvnom srodstvu.");
            else
                System.Console.WriteLine("Ove dvije osobe su u rodu u {0}. koljenu", koljena - 1);
        }

    }
}
