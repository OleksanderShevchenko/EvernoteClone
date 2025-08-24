using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EvernoteClone.Model
{
    public class User : INotifyPropertyChanged
	{
		private int id;
		[PrimaryKey, AutoIncrement]
        public int Id 
		{ 
			get { return id; }
			set
			{
				id = value;
				OnPropertyChanged(nameof(Id));
			}
		}

		private string name;
		[MaxLength(50)]
		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				OnPropertyChanged(nameof(Name));
			} 
		}

		private string lastname;
		[MaxLength(50)]
		public string Lastname 
		{
			get { return lastname; }
			set
			{
				lastname = value;
				OnPropertyChanged(nameof(Lastname));
			}
		}

		private string userName;
		[MaxLength(50)]
		public string Username 
		{
			get { return userName; }
			set
			{
				userName = value;
				OnPropertyChanged(nameof(Username));
			}
		}

		private string email;
		[MaxLength(150)]
		public string Email
		{
			get { return email; }
			set
			{
				email = value;
				OnPropertyChanged(nameof(Email));
			}
		}

		private string password;
		[MaxLength(100)]
		public string Password 
		{
			get { return password; }
			set
			{
				password = value;
				OnPropertyChanged(nameof(Password));
			}
		}

        public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
