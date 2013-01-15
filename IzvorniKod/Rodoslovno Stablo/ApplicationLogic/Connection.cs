using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{

//	VEZA
//	vezaID
//	vrstaVeze
//	osobaID1
//	osobaID2

	public class Connection
	{
		// personID1 je ConnectionType osobi sa personID2
		public Guid ID { get; set; }
		public string type { get; set; }	// zasad samo {parent, partner}
		public Guid personID1 { get; set; }		// nadredjena osoba
		public Guid personID2 { get; set; }
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }

		public Connection(Guid id)
		{ 
			ID = id;
		}
	}
}
