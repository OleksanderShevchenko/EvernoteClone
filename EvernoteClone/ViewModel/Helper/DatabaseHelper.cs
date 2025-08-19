using EvernoteClone.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel.Helper
{
	public class DatabaseHelper
	{
		private static string dbName = AppConfig.Instance.DbName;
		private static string dbFile = Path.Combine(Environment.CurrentDirectory, dbName);

		public static async Task<bool> Insert<T>(T item)
		{
			bool result = false;

			using (var db = new SQLite.SQLiteConnection(dbFile))
			{
				db.CreateTable<T>();
				int rows = db.Insert(item);
				if (rows > 0)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<bool> Update<T>(T item)
		{
			bool result = false;

			using (var db = new SQLite.SQLiteConnection(dbFile))
			{
				db.CreateTable<T>();
				int rows = db.Update(item);
				if (rows > 0)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<bool> Delete<T>(T item)
		{
			bool result = false;

			using (var db = new SQLite.SQLiteConnection(dbFile))
			{
				db.CreateTable<T>();
				int rows = db.Delete(item);
				if (rows > 0)
				{
					result = true;
				}
			}
			return result;
		}

		public static async Task<List<T>> Read<T>() where T : new()
		{
			List<T> result = new List<T>();

			using (var db = new SQLite.SQLiteConnection(dbFile))
			{
				db.CreateTable<T>();
				db.Table<T>().ToList().ForEach(item => result.Add(item));
			}
			return result;
		}

	}
}

