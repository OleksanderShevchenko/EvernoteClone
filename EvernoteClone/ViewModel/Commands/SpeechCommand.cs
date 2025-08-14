using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
	public class SpeechCommand: ICommand
	{
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return true; // You can add logic to determine if the command can execute
		}
		public void Execute(object parameter)
		{
			// Logic to handle speech command execution
			Console.WriteLine("Speech command executed.");
		}
		
		private NotesVM _notesVM;
		public SpeechCommand(NotesVM vm)
		{
			_notesVM = vm;
		}
	}

}
