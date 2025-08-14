using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EvernoteClone.View
{
	/// <summary>
	/// Interaction logic for NotesWindow.xaml
	/// </summary>
	public partial class NotesWindow : Window
	{
		public NotesWindow()
		{
			InitializeComponent();
		}

		private void rtbNoteContent_TextChanged(object sender, TextChangedEventArgs e)
		{
			int wordCount = new TextRange(rtbNoteContent.Document.ContentStart, rtbNoteContent.Document.ContentEnd).Text.Length;
			statusTextBlick.Text = $"Document length: {wordCount} characters";
		}

		private void btnBold_Click(object sender, RoutedEventArgs e)
		{
			if (btnBold.IsChecked == true)
			{
				rtbNoteContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
			}
			else
			{
				rtbNoteContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
			}
		}
	}
}
