using System;
using System.IO;
using System.Text.Json;
using System.Reflection;
using System.ComponentModel;
using System.Windows;

namespace EvernoteClone.Core
{
	/// <summary>
	/// Provides access to application configuration settings using a thread-safe singleton pattern.
	/// Reads configuration from an 'app_config.json' file located in the application's root directory.
	/// </summary>
	public sealed class AppConfig
	{
		#region Singleton Implementation

		// Static instance of the AppConfig class, initialized lazily and in a thread-safe manner.
		// The Lazy<T> class ensures that the instance is created only when it's first accessed.
		private static readonly Lazy<AppConfig> lazyInstance = new Lazy<AppConfig>(() => new AppConfig());

		/// <summary>
		/// Gets the single, thread-safe instance of the AppConfig.
		/// </summary>
		public static AppConfig Instance => lazyInstance.Value;

		#endregion

		#region Configuration Properties

		/// <summary>
		/// Thedatabase location.
		/// </summary>
		public string DbName { get; }
		public string MobileClient { get; }

		#endregion

		/// <summary>
		/// Private constructor to prevent external instantiation.
		/// This is where the configuration file is loaded and parsed.
		/// </summary>
		private AppConfig()
		{

			if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
			{
				// Provide mock config for designer
				DbName = "NotesDB.db3";  // use default name
				MobileClient = "https://wpfevernote.azurewebsites.net";
				return;
			}

			try
			{
				// Determine the path of the executable.
				string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				string configFilePath = Path.Combine(exePath, "app_config.json");

				if (!File.Exists(configFilePath))
				{
					DbName = "NotesDB.db3";  // use default name
					MobileClient = "https://wpfevernote.azurewebsites.net";
					return;
				}

				// Read the entire JSON file.
				string jsonContent = File.ReadAllText(configFilePath);

				// Deserialize the JSON into a temporary helper class.
				var configData = JsonSerializer.Deserialize<AppSettingsData>(jsonContent);

				// Assign the values to the public properties.
				DbName = configData?.dbName;
				MobileClient = configData?.mobileClient;
				// Validate that essential settings were loaded.
				if (string.IsNullOrEmpty(DbName))
				{
					DbName = "NotesDB.db3";  // use default name
					return;
				}
				if (string.IsNullOrEmpty(MobileClient))
				{
					MobileClient = "https://wpfevernote.azurewebsites.net"; // use default URL
				}
			}
			catch (Exception ex)
			{
				// Handle exceptions during file reading or parsing.
				// You might want to log this error to a file or show a message to the user.
				throw new InvalidOperationException("Failed to load or parse application configuration.", ex);
			}
		}

		/// <summary>
		/// Private helper class used for deserializing the JSON structure.
		/// The property names here must match the keys in the JSON file.
		/// </summary>
		private class AppSettingsData
		{
			public string dbName { get; set; }
			public string mobileClient { get; set; }
		}
	}
}

