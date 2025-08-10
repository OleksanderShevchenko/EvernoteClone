using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
	public class LoginCommand: ICommand
	{
		public event EventHandler CanExecuteChanged;
		public bool CanExecute(object parameter)
		{
			return true; // Logic to determine if the command can execute
		}
		public void Execute(object parameter)
		{
			// Logic to execute when the command is invoked
			Console.WriteLine("Login command executed.");
		}

		private LogInVM _logInVM;
		public LoginCommand(LogInVM vm)
		{
			_logInVM = vm;
		}
	
	}
}
