using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ApplicationLogic
{

//	VEZA
//	vezaID
//	vrstaVeze
//	osobaID1
//	osobaID2

	[Serializable]
    public class Connection
	{
		// personID1 je ConnectionType osobi sa personID2
        [XmlElement("ID")]
		public Guid ID { get; set; }

        [XmlElement("type")]
		public string type { get; set; }	// zasad samo {parent, partner}

        [XmlElement("personID1")]
		public Guid personID1 { get; set; }		// nadredjena osoba

        [XmlElement("personID2")]
		public Guid personID2 { get; set; }

        [XmlElement("startDate")]
		public DateTime startDate { get; set; }

        [XmlElement("endDate")]
		public DateTime endDate { get; set; }

		public Connection() { }

		public Connection(Guid id)
		{ 
			ID = id;
		}

		public override string ToString()
		{
			return String.Format("ID = {0}, type = {1}, osobaID1 = {2}, osobaID2 = {3}", ID, type, personID1, personID2);
		}

		public override bool Equals(System.Object obj)
		{
			// ako je null nije jednako
			if (obj == null)
			{
				return false;
			}

			// Ako se ne moze kastati sigurno nije dobro
			Connection c = obj as Connection;
			if ((System.Object)c == null)
			{
				return false;
			}

			return this.Equals(c);
		}

		public bool Equals(Connection c)
		{
			return ID == c.ID && type == c.type && personID1 == c.personID1 && personID2 == c.personID2 && startDate == c.startDate && endDate == c.endDate;
		}
	}
}
