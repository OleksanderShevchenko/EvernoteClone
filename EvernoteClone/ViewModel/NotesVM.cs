using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helper;
using System.Speech.Recognition;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;

namespace EvernoteClone.ViewModel
{
	public class NotesVM: INotifyPropertyChanged
	{
		private SpeechRecognitionEngine _recognizer;
		private bool _isListening;
		private string _recognizedText;
		private string _selectedLanguage;
		private bool _isEditing;

		public bool IsEditing
		{
			get { return _isEditing; }
			set { _isEditing = value; }
		}

		public bool IsListening
		{
			get { return _isListening; }
			set
			{
				_isListening = value;
				OnPropertyChanged(nameof(IsListening));
				OnPropertyChanged(nameof(SpeechButtonText));
			}
		}

		public string SpeechButtonText => IsListening ? "Stop" : "Speech";

		public string RecognizedText
		{
			get { return _recognizedText; }
			set
			{
				_recognizedText = value;
				OnPropertyChanged(nameof(RecognizedText));
			}
		}

		public string SelectedLanguage
		{
			get { return _selectedLanguage; }
			set
			{
				_selectedLanguage = value;
				OnPropertyChanged(nameof(SelectedLanguage));
			}
		}
		// collections
		public ObservableCollection<Notebook> Notebooks { get; set; }
		public ObservableCollection<string> Languages { get; set; }
		public ObservableCollection<Note> Notes { get; set; }

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

		private Note note;

		public Note SelectedNote
		{
			get { return note; }
			set
			{
				note = value;
				SelectedNoteChanged(this, new EventArgs());
			}
		}

		// Commands
		public NewNotebookCommand NewNotebookCommand { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }
		public ExitCommand ExitCommand { get; set; }
		public SpeechCommand SpeechCommand { get; set; }
		public BeginEditCommand BeginEditCommand { get; set; }
		public HasEditedCommand HasEditedCommand { get; set; }
		public DeleteNotebookCommand DeleteNotebookCommand { get; set; }

		public event PropertyChangedEventHandler? PropertyChanged;
		public event EventHandler SelectedNoteChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public NotesVM()
		{
			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();
			Languages = new ObservableCollection<string>() { "English", "Ukrainian" };
			SelectedLanguage = "English";
			_recognizedText = string.Empty;

			GetNotebooks(); // Load existing notebooks from the database
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);
			ExitCommand = new ExitCommand(this);
			SpeechCommand = new SpeechCommand(this);
			BeginEditCommand = new BeginEditCommand(this);
			HasEditedCommand = new HasEditedCommand(this);
			DeleteNotebookCommand = new DeleteNotebookCommand(this);
		}

		// This method is called from the SpeechCommand
		public void ToggleSpeechRecognition()
		{
			if (IsListening)
			{
				StopRecognition();
			}
			else
			{
				StartRecognition();
			}
		}

		private void StartRecognition()
		{
			// Stop any existing recognizer first
			StopRecognition();

			try
			{
				CultureInfo culture;
				string languageName;

				if (SelectedLanguage == "English")
				{
					culture = new CultureInfo("en-US");
					languageName = "English";
				}
				else // Assumes SelectedLanguage is "Ukrainian"
				{
					culture = new CultureInfo("uk-UA");
					languageName = "Ukrainian";
				}

				// Check if the selected language recognizer is available
				var installedRecognizers = SpeechRecognitionEngine.InstalledRecognizers();
				bool recognizerFound = false;
				foreach (var recognizerInfo in installedRecognizers)
				{
					if (recognizerInfo.Culture.Equals(culture))
					{
						recognizerFound = true;
						break;
					}
				}

				if (!recognizerFound)
				{
					MessageBox.Show(
						$"The {languageName} speech recognition pack is not installed on this system. " +
						"Please go to Windows Settings > Time & Language > Language & region to install it.",
						"Speech Recognition Not Available",
						MessageBoxButton.OK,
						MessageBoxImage.Warning
					);
					return; // Exit the method gracefully
				}

				_recognizer = new SpeechRecognitionEngine(culture);

				// Configure for dictation (free-form speech)
				_recognizer.LoadGrammar(new DictationGrammar());

				// Hook up the event handler
				_recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

				// Set the microphone as input
				_recognizer.SetInputToDefaultAudioDevice();

				// Start listening asynchronously
				_recognizer.RecognizeAsync(RecognizeMode.Multiple);

				IsListening = true;
				RecognizedText = string.Empty; // Clear previous text
			}
			catch (Exception ex)
			{
				// This catch block is for any other unforeseen errors
				MessageBox.Show(
					$"An unexpected error occurred: {ex.Message}",
					"Speech Recognition Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
				IsListening = false;
				if (_recognizer != null)
				{
					_recognizer.Dispose();
					_recognizer = null;
				}
			}
		}

		public void StopRecognition()
		{
			if (_recognizer != null)
			{
				_recognizer.RecognizeAsyncCancel();
				_recognizer.Dispose();
				_recognizer = null;
			}
			IsListening = false;
		}

		private void Recognizer_SpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
		{
			// Use Dispatcher to ensure the UI update happens on the main thread
			Application.Current.Dispatcher.Invoke(() =>
			{
				RecognizedText += e.Result.Text + " ";
			});
		}

		public async void CreateNote(int notebookID)
		{
			Note newNote = new Note()
			{
				NotebookId = notebookID,
				Title = "New Note",
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				FileLocation = string.Empty // Set default file location or leave empty
			};

			await DatabaseHelper.Insert(newNote);
			GetNotes(); // Refresh notes after adding a new one
		}

		public async void CreateNoteBook()
		{
			if(App.UserId == 0)
			{
				// Handle the case where UserId is not set
				MessageBox.Show("User not logged in. Please log in to create a notebook.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			Notebook newNotebook = new Notebook()
			{
				UserId = App.UserId,
				Name = "New Notebook",
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now
			};
			await DatabaseHelper.Insert(newNotebook);
			GetNotebooks(); // Refresh notebooks after adding a new one
		}

		private async void GetNotebooks()
		{
			Notebooks.Clear(); // Clear existing notebooks before loading new ones
			var notebooks = await DatabaseHelper.Read<Notebook>();
			if (notebooks != null)
			{
				foreach (var notebook in notebooks)
				{
					if (notebook.UserId == App.UserId) // Only add notebooks for the current user
						Notebooks.Add(notebook);
				}
			}
		}

		public async void DeleteNotebook(Notebook notebook)
		{
			await DatabaseHelper.Delete(notebook);
			Notebooks.Remove(notebook);
		}

		private async void GetNotes()
		{
			Notes.Clear(); // Clear existing notes before loading new ones
			if (selectedNotebook == null)
			{
				return; // No notebook selected, so no notes to retrieve
			}
			var notes = (await DatabaseHelper.Read<Note>()).Where(n => n.NotebookId == selectedNotebook.Id).ToList();
			if (notes != null)
			{
				foreach (var note in notes)
				{
					Notes.Add(note);
				}
			}
		}

		public void StartEditing()
		{
			IsEditing = true;
		}

		public async void HasRenamed(Notebook notebook)
		{
			if (notebook != null)
			{
				await DatabaseHelper.Update(notebook);
				IsEditing = false;
				GetNotebooks();
			}
		}

		public async void UpdateSelectedNote()
		{
			await DatabaseHelper.Update(SelectedNote);
		}

	}
}
