using EvernoteClone.Model;
using System.Windows;
using System.Windows.Controls;

namespace EvernoteClone.View.UserControls
{
	public partial class NoteBookControl : UserControl
	{
		public Model.Notebook DisplayNotebook
		{
			get { return (Model.Notebook)GetValue(notebookProperty); }
			set { SetValue(notebookProperty, value); }
		}

		public static readonly DependencyProperty notebookProperty =
			DependencyProperty.Register("DisplayNotebook", typeof(Model.Notebook), typeof(NoteBookControl), new PropertyMetadata(null, SetValues));

		private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NoteBookControl notebook = d as NoteBookControl;

			if (notebook != null)
			{
				notebook.DataContext = notebook.DisplayNotebook;
			}
		}

		public NoteBookControl()
		{
			InitializeComponent();
		}
	}
}