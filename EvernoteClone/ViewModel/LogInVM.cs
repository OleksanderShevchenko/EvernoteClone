using EvernoteClone.Core;
using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel
{
	public class LogInVM
	{
		private bool _isUseAzure = true;
		private User _user;

		public User User
		{
			get { return _user; }
			set { _user = value; }
		}

		public RegisterCommand RegisterCommand { get; set; }
		public LoginCommand LoginCommand { get; set; }

		public event EventHandler HasLoggedIn;

		public LogInVM()
		{
			User = new User();
			RegisterCommand = new RegisterCommand(this);
			LoginCommand = new LoginCommand(this);
			_isUseAzure = AppConfig.Instance.UseAzureDB;
		}

		public async void Login()
		{
			if (_isUseAzure)
			{
				var user = (await DatabaseHelper.client.GetTable<User>().Where(u => u.Username == User.Username).ToListAsync()).FirstOrDefault();
				if (user.Password == User.Password)
				{
					App.UserId = user.Id;
					HasLoggedIn(this, new EventArgs());
				}
			}
			else
			{
				using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.DBFile))
				{
					conn.CreateTable<User>();

					var user = conn.Table<User>().Where(u => u.Username == User.Username).FirstOrDefault();

					if (user.Password == User.Password)
					{
						App.UserId = user.Id;
						HasLoggedIn(this, new EventArgs());
					}
				}
			}
		}

		public async void Register()
		{
			if (_isUseAzure)
			{
				var result = await DatabaseHelper.Insert(User);

				if (result)
				{
					App.UserId = User.Id;
					HasLoggedIn(this, new EventArgs());
				}
			}
			else
			{
				using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(DatabaseHelper.DBFile))
				{
					conn.CreateTable<User>();

					var result = await DatabaseHelper.Insert(User);

					if (result)
					{
						App.UserId = User.Id;
						HasLoggedIn(this, new EventArgs());
					}
				}
			}
		}
	}
}
