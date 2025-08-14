using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
	public class NewNoteCommand: ICommand
	{
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			if (parameter as Notebook == null)
			{
				return false; // Cannot execute if no notebook is selected
			}
			return true; // Logic to determine if the command can execute
		}

		public void Execute(object parameter)
		{
			Notebook selectedNotebook = parameter as Notebook;

			if (selectedNotebook != null)
			{
				_notesVM.CreateNote(selectedNotebook.Id);
			}
		}

		private NotesVM _notesVM;
		public NewNoteCommand(NotesVM vm)
		{
			_notesVM = vm;
		}
	}
}
