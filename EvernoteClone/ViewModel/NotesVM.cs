using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel
{
	public class NotesVM: INotifyPropertyChanged
	{

		public ObservableCollection<Notebook> Notebooks { get; set; }

		private Notebook selectedNotebook;

		public Notebook SelectedNotebook
		{
			get { return selectedNotebook; }
			set 
			{ 
				selectedNotebook = value;
				OnPropertyChanged(nameof(SelectedNotebook));
				GetNotes(); // Fetch notes when a notebook is selected
			}
		}

		public ObservableCollection<Note> Notes { get; set; }
		public NewNotebookCommand NewNotebookCommand { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }
		public ExitCommand ExitCommand { get; set; }
		public SpeechCommand SpeechCommand { get; set; }

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public NotesVM()
		{
			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();
			GetNotebooks(); // Load existing notebooks from the database
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);
			ExitCommand = new ExitCommand(this);
			SpeechCommand = new SpeechCommand(this);
		}

		public void CreateNote(int notebookID)
		{
			Note newNote = new Note()
			{
				NotebookId = notebookID,
				Title = "New Note",
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				FileLocation = string.Empty // Set default file location or leave empty
			};

			DatabaseHelper.Insert(newNote);
			GetNotes(); // Refresh notes after adding a new one
		}

		public void CreateNoteBook(int _userID)
		{
			Notebook newNotebook = new Notebook()
			{
				UserId = _userID,
				Name = "New Notebook",
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now
			};
			DatabaseHelper.Insert(newNotebook);
			GetNotebooks(); // Refresh notebooks after adding a new one
		}

		private void GetNotebooks()
		{
			Notebooks.Clear(); // Clear existing notebooks before loading new ones
			var notebooks = DatabaseHelper.Read<Notebook>();
			if (notebooks != null)
			{
				foreach (var notebook in notebooks)
				{
					Notebooks.Add(notebook);
				}
			}
		}

		private void GetNotes()
		{
			Notes.Clear(); // Clear existing notes before loading new ones
			if (selectedNotebook == null)
			{
				return; // No notebook selected, so no notes to retrieve
			}
			var notes = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == selectedNotebook.Id).ToList();
			if (notes != null)
			{
				foreach (var note in notes)
				{
					Notes.Add(note);
				}
			}
		}

	}
}
