using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ApplicationLogic
{
	public class QueryProcessor
	{
		private Tree Drvo;
		private Func<List<Person>, Person> QueryDisambiguator;

		public QueryProcessor(Tree drvo, Func<List<Person>, Person> QD, TextWriter tw = null)
		{
			// dodajmo malo couplinga
			Drvo = drvo;
			QueryDisambiguator = QD;

			if (tw != null)		// Da omogucimo ovo http://saezndaree.wordpress.com/2009/03/29/how-to-redirect-the-consoles-output-to-a-textbox-in-c/
				System.Console.SetOut(tw);
		}

		public void RunCommand(string input)
		{
			// TODO parsaj upit
			throw new QuitException();
		}

		public class QuitException : Exception { };
	}
}
