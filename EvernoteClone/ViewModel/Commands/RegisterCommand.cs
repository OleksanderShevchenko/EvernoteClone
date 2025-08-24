using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
	public class RegisterCommand: ICommand
	{
		public event EventHandler CanExecuteChanged;
		public bool CanExecute(object parameter)
		{
			return true; // Logic to determine if the command can execute
		}
		public void Execute(object parameter)
		{
			_logInVM.Register();
		}

		private LogInVM _logInVM;
		public RegisterCommand(LogInVM vm)
		{
			_logInVM = vm;
		}
	}
	
}
