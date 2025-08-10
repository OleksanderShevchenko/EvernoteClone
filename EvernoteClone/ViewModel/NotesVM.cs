using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel
{
	public class NotesVM
	{

		public ObservableCollection<Notebook> Notebooks { get; set; }

		private Notebook selectedNotebook;

		public Notebook SelectedNotebook
		{
			get { return selectedNotebook; }
			set 
			{ 
				selectedNotebook = value;
				// TODO - get totes of the notebook from the database
			}
		}

		public ObservableCollection<Note> Notes { get; set; }
		public NewNotebookCommand NewNotebookCommand { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }

		public NotesVM()
		{
			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);

		}

	}
}
