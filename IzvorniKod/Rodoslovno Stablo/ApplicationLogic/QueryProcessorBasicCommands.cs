using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{
	public partial class QueryProcessor
	{
		#region Implementacija komandi (jeej)

		public void DodajBaku(string[] parametri)
		{
			//TODO
			throw new System.NotImplementedException("TODO DodajBaku");
		}

		public void PromijeniPodatke(string[] parametri)
		{
			//TODO 
			//System.Console.WriteLine


			throw new System.NotImplementedException("TODO PromijeniPodatke");
		}

		// definiraj novi exception, koristimo ga da program zna kada korisnik želi izaći iz programa
		public class QuitException : Exception { };

		public void Izlaz(string[] parametri)
		{
			throw new QuitException();
		}
		#endregion
	}
}
