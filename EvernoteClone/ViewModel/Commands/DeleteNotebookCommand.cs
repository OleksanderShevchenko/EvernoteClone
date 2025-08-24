using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
	public class DeleteNotebookCommand : ICommand
	{
		public NotesVM Vm { get; set; }
		public event EventHandler CanExecuteChanged;

		public DeleteNotebookCommand(NotesVM vm)
		{
			Vm = vm;
		}

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter)
		{
			Notebook notebook = parameter as Notebook;
			if (notebook != null)
				Vm.DeleteNotebook(notebook);
		}
	}
}
