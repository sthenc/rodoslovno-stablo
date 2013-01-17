using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data;

using ServiceStack.Common.Utils;
using ServiceStack.DataAnnotations;
using ServiceStack.Common.Extensions;
using System.Reflection;

using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;


namespace ApplicationLogic
{
	public class User
	{	// TODO ostatak
		public User() { }
		[AutoIncrement]
		[Alias("UserID")]
		public Int32 ID { get; set; }
		[Index(Unique=true)]
		[StringLength(10)]
		public string Username { get; set; }
		[StringLength(10)]
		public string Password { get; set; }
	}

	public class UserManager
	{
		// Izvor za orm  https://github.com/ServiceStack/ServiceStack.OrmLite
		// inspiracija za ovaj kod https://github.com/ServiceStack/ServiceStack.OrmLite/blob/master/src/SqliteExpressionsTest/Program.cs

		// rijesenje buga sa loadanjem .dll-a http://mdetras.com/2011/09/29/unable-to-load-dll-sqlite-interop-dll/

		private static  string GetFileConnectionString()
		{
			var connectionString = "~/db.sqlite".MapAbsolutePath();
			if (File.Exists(connectionString))
				File.Delete(connectionString);

			System.Console.WriteLine("Baza na lokaciji {0}", connectionString);
			return connectionString;
		}
		public UserManager()
		{
			OrmLiteConfig.DialectProvider = SqliteOrmLiteDialectProvider.Instance;
			SqlExpressionVisitor<User> ev = OrmLiteConfig.DialectProvider.ExpressionVisitor<User>();
			
			using (IDbConnection db =
			       GetFileConnectionString().OpenDbConnection())
			using (IDbCommand dbCmd = db.CreateCommand())
			{
				dbCmd.DropTable<User>();
				dbCmd.CreateTable<User>();
				dbCmd.DeleteAll<User>();

				List<User> users = new List<User>();
				users.Add(new User() { Username = "bata", Password = "zivoj"});
				users.Add(new User() { Username = "bata1", Password = "zivoj" });
				users.Add(new User() { Username = "bata2", Password = "zivoj" });
				users.Add(new User() { Username = "bata3", Password = "zivoj" });

				dbCmd.InsertAll(users);	
			}
		}

		public bool Login(string username, string password)
		{
			// TODO, ovo i povezivanje sa bazom
			//throw new NotImplementedException();

			if (username == "admin" && password == "abc123")
				return true;

			return false;
		}

		public bool Logoff()
		{	
			return true;
		}
	}
}
