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

		public Connection(Guid id)
		{ 
			ID = id;
		}
	}
}
