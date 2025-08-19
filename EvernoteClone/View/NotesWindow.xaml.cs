using EvernoteClone.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
		private NotesVM _viewModel;
		public NotesWindow()
		{
			// Step 1: Create a SINGLE instance of the ViewModel
			_viewModel = new NotesVM();

			// Step 2: Set the DataContext of the window to this instance
			this.DataContext = _viewModel;

			// Step 3: Listen for changes on the RecognizedText property from this instance
			_viewModel.PropertyChanged += ViewModel_PropertyChanged;
		}

		private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(NotesVM.RecognizedText))
			{
				// Update the RichTextBox with the new text.
				// Note: This approach clears and re-populates the text each time.
				// For a more efficient "append" behavior, you could check if the new text
				// is a continuation of the old one and only add the new part.
				rtbNoteContent.Document.Blocks.Clear();
				rtbNoteContent.Document.Blocks.Add(new Paragraph(new Run(_viewModel.RecognizedText)));
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Call the cleanup method on the ViewModel
			_viewModel.StopRecognition();
		}

		private void rtbNoteContent_TextChanged(object sender, TextChangedEventArgs e)
		{
			int wordCount = new TextRange(rtbNoteContent.Document.ContentStart, rtbNoteContent.Document.ContentEnd).Text.Length;
			statusTextBlick.Text = $"Document length: {wordCount} characters";
		}

		private void btnBold_Click(object sender, RoutedEventArgs e)
		{
			bool isBold = (sender as ToggleButton)?.IsChecked ?? false;
			if (isBold)
			{
				rtbNoteContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
			}
			else
			{
				rtbNoteContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
			}
		}

		private void rtbNoteContent_SelectionChanged(object sender, RoutedEventArgs e)
		{
			var selectedWeight = rtbNoteContent.Selection.GetPropertyValue(FontWeightProperty);
			btnBold.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && (selectedWeight.Equals(FontWeights.Bold));
		}
	}
}
