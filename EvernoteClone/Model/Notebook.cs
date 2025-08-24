using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EvernoteClone.Model
{
    public class Notebook : INotifyPropertyChanged
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

		private int userId;
		[Indexed]
        public int UserId
		{
			get { return userId; }
			set
			{
				userId = value;
				OnPropertyChanged(nameof(UserId));
			}
		}

		private string name;
		public string Name 
		{ 
			get { return name; }
			set
			{
				name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		private DateTime createdAt;
		public DateTime CreatedAt 
		{
			get { return createdAt; }
			set
			{
				createdAt = value;
				OnPropertyChanged(nameof(CreatedAt));
			}
		}

		private DateTime updatedAt;
		public DateTime UpdatedAt 
		{
			get { return updatedAt; }
			set
			{
				updatedAt = value;
				OnPropertyChanged(nameof(UpdatedAt));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
