using System;
using Terminal.Gui;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using CSCommonSecrets;

namespace WhisperDragonCLI;

public enum VisibleElement
{
	NewAndOpenWizard,

	CreateNewCommonSecretsContainer,
	OpenExistingCommonSecretsContainer,

	ShowPasswordCreators,

	ShowLoginInformations,
	ShowNotes,
	ShowFiles,
	ShowContacts,
	ShowPaymentCards,

	AddOrEditLoginInformation,
	AddOrEditNote,
	AddOrEditFile,
	AddOrEditContact,
	AddOrEditPaymentCard,

	DeleteItemConfirmation,
}

public enum CommonSecretsFileFormat
{
	Unknown = -1,
	Json = 0,
	Xml
}

class Program
{
	private static CommonSecretsFileFormat fileFormat = CommonSecretsFileFormat.Unknown;
	private static bool isContainerOpen = false;
	private static bool isContainerModified = false;

	private static string fullFilePath = "";

	private static readonly string defaultNameForNewFile = "*Unsaved*";
	private static string filename = "";

	private static CommonSecretsContainer? commonSecretsContainer = null;

	private static TabWithId loginsTab;

	private static StatusBar statusBar;

	private static readonly Dictionary<byte[], byte[]> knownDerivedPasswords = new Dictionary<byte[], byte[]>();

	static void Main(string[] args)
	{
		Application.Init();
		var menu = new MenuBar(new MenuBarItem[] {
			new MenuBarItem("_File", new MenuItem [] {
				new MenuItem("_New...", LocMan.Get("New CommonSecrets file..."), () =>
				{
					if (isContainerOpen)
					{
						if (TryToCloseFile())
						{

						}
						else
						{
							return;
						}
					}
					var createNew = NewFileDialog.CreateNewFileDialog(NewCommonSecretsCreated, null);
					Application.Run(createNew);
				}),
				new MenuItem("_Open...", LocMan.Get("Open existing CommonSecrets file..."), () => OpenCommonSecretsFile()),
				new MenuItem("_Save", "Save CommonSecrets file", () => SaveCommonSecretsFile(), () => commonSecretsContainer != null && fileFormat != CommonSecretsFileFormat.Unknown),
				new MenuItem("Save As...", "Save CommonSecrets file as...", () => SaveCommonSecretsFileAs(), () => commonSecretsContainer != null),
				new MenuItem("_Close", "Close file", () => {
					TryToCloseFile();
				}, () => isContainerOpen),
				new MenuItem("_Quit", "Quit", () => {
					TryToQuit();
				})
			}),

			new MenuBarItem("_Edit", new MenuItem[] {
				new MenuItem("_Add...", "Add entry", () => {}, () => isContainerOpen),
				new MenuItem("_Duplicate...", "Duplicate entry", () => {}, () => isContainerOpen),
				new MenuItem("_Remove...", "Remove entry", () => {}, () => isContainerOpen),
			}),

			new MenuBarItem("_Tools", new MenuItem[] {
				new MenuItem("_Generate Random Password...", LocMan.Get("Generate a new random password..."), () =>
				{
					var createPasswordSelection = PasswordGeneratorDialog.CreateDialog(() => Application.RequestStop());
					Application.Run(createPasswordSelection);
				}),
				new MenuItem("_Generate Pronounceable Password...", LocMan.Get("Generate a new pronounceable password..."), () =>
				{
					var createPronounceablePasswordSelection = PasswordGeneratorPronounceableDialog.CreateDialog(() => Application.RequestStop());
					Application.Run(createPronounceablePasswordSelection);
				}),
			}),

			new MenuBarItem("_Help", new MenuItem[] {
				new MenuItem("_About...", LocMan.Get("About..."), () =>
				{
					MessageBox.Query(50, 8, "About", "WhisperDragonCLI is CommonSecrets compatible password/secrets manager for terminals." + Environment.NewLine + Environment.NewLine + "https://github.com/mcraiha/WhisperDragonCLI", "Ok" );
				}),
			}),
		});

		var win = new Window(filename)
		{
			X = 0,
			Y = 1,
			Width = Dim.Fill(),
			Height = Dim.Fill() - 1
		};

		statusBar = new StatusBar(GetStatusItems(VisibleElement.ShowLoginInformations));
		/*
		//LoginInformationsWindow.CreateLoginInformationsDialog(win);
		TabView tabView = new TabView()
		{
			X = 0,
			Y = 0,
			Width = Dim.Fill(),
			Height = Dim.Fill(1)
		};

		loginsTab = new TabWithId(VisibleElement.ShowLoginInformations, "Login informations", LoginInformationsView.Create(GetTestLogins()));
		tabView.AddTab(loginsTab, true);

		tabView.AddTab(new TabWithId(VisibleElement.ShowNotes, "Notes", NotesView.CreateView(GetTestNotes())), false);
		tabView.AddTab(new TabWithId(VisibleElement.ShowFiles, "Files", FilesView.CreateView(GetTestFiles())), false);
		tabView.AddTab(new TabWithId(VisibleElement.ShowContacts, "Contacts", ContactsView.CreateView(GetTestContacts())), false);
		tabView.AddTab(new TabWithId(VisibleElement.ShowPaymentCards, "Payment cards", PaymentCardsView.CreateView(GetTestPaymentCards())), false);
		tabView.SelectedTabChanged += (_, tabChangedEventArgs) =>
		{
			TabWithId selectedTab = (TabWithId)tabChangedEventArgs.NewTab;
			statusBar.Items = GetStatusItems(selectedTab.GetTabType());
		};
		*/
		//win.Add(tabView);

		TextView tv = new TextView()
		{
			X = 0,
			Y = 0,
			Width = Dim.Fill(),
			Height = Dim.Fill(1)
		};
		tv.ReadOnly = true;
		tv.Text = EmbedResourceLoader.ReadAsString("Assets/BgAscii.txt");
		win.Add(tv);

		// Add both menu and win in a single call
		Application.Top.Add(menu, win, statusBar);
		Application.Run();
	}

	private static void CopyUsername()
	{
		//((LoginInformationsView)loginsTab.GetView()).CopyURLToClipboard();
	}

	private static void CopyPassword()
	{

	}

	// Development related data (will be removed at some point)

	private static List<LoginSimplified> GetTestLogins()
	{
		return new List<LoginSimplified>()
		{
			new LoginSimplified()
			{
				zeroBasedIndexNumber = 0,
				IsSecure = true,
				Title = "Fake service",
				URL = "https://fakeservice.com",
				Email = "dragon@example.com",
				Username = "Dragon",
				Password = "gwWTY#¤&%36",
				Category = "Samples",
				Tags = "Samples Demo",
			},
			new LoginSimplified()
			{
				zeroBasedIndexNumber = 1,
				IsSecure = false,
				Title = "Fake mail",
				URL = "https://fakemail.com",
				Email = "cooldragon@example.org",
				Username = "Dragon",
				Password = "Si0bSww5bYeKp7Rs",
				Category = "Samples",
				Tags = "Samples Demo",
			},
		};
	}

	private static List<NoteSimplified> GetTestNotes()
	{
		return new List<NoteSimplified>()
		{
			new NoteSimplified()
			{
				zeroBasedIndexNumber = 0,
				IsSecure = true,
				Title = "Shopping list",
				Text = "Butter, milk, eggs, cucumber"
			},
			new NoteSimplified()
			{
				zeroBasedIndexNumber = 1,
				IsSecure = false,
				Title = "TODO list",
				Text = "Call elevator repair person"
			}
		};
	}

	private static List<FileSimplified> GetTestFiles()
	{
		return new List<FileSimplified>()
		{
			new FileSimplified()
			{
				zeroBasedIndexNumber = 0,
				IsSecure = true,
				Filename = "nature.jpg",
				Filesize = "234 kB",
				Filetype = "JPEG"
			},
			new FileSimplified()
			{
				zeroBasedIndexNumber = 1,
				IsSecure = false,
				Filename = "cv.docx",
				Filesize = "83 kB",
				Filetype = "Microsoft Word document"
			}
		};
	}

	private static List<ContactSimplified> GetTestContacts()
	{
		return new List<ContactSimplified>()
		{
			new ContactSimplified()
			{
				zeroBasedIndexNumber = 0,
				IsSecure = true,
				FirstName = "Hanna",
				LastName = "Hard Worker",
				Emails = "hanna@localhost (work)",
				PhoneNumbers = "1-800-123123 (work)"
			},
			new ContactSimplified()
			{
				zeroBasedIndexNumber = 1,
				IsSecure = false,
				FirstName = "Mike",
				LastName = "Madr",
				Emails = "mike@localhost (work)",
				PhoneNumbers = "1-800-123321 (home)"
			}
		};
	}

	private static List<PaymentCardSimplified> GetTestPaymentCards()
	{
		return new List<PaymentCardSimplified>()
		{
			new PaymentCardSimplified()
			{
				zeroBasedIndexNumber = 0,
				IsSecure = true,
				Title = "Dragon credit",
				NameOnTheCard = "Dave Dragon",
				CardType = "Credit",
				Number = "4024007105746837",
				SecurityCode = "678"
			},
			new PaymentCardSimplified()
			{
				zeroBasedIndexNumber = 1,
				IsSecure = false,
				Title = "Fire bank",
				NameOnTheCard = "Dave teh Dragon",
				CardType = "Debit",
				Number = "4024007182777473",
				SecurityCode = "599"
			}
		};
	}

	private static void SinglePasswordAsked(string password)
	{
		byte[] derivedPassword = commonSecretsContainer!.keyDerivationFunctionEntries[0].GeneratePasswordBytes(password);
		(bool valid, Exception exception) = Helpers.CheckDerivedPassword(derivedPassword);

		if (!valid)
		{
			// TODO: show error about incorrectly derived password
		}

		// Find first entry that is encrypted with given password
		string wantedKeyIdentifier = commonSecretsContainer!.keyDerivationFunctionEntries[0].GetKeyIdentifier();
		foreach (LoginInformationSecret loginInformationSecret in commonSecretsContainer.loginInformationSecrets)
		{
			if (loginInformationSecret.GetKeyIdentifier() == wantedKeyIdentifier && !loginInformationSecret.CanBeDecryptedWithDerivedPassword(derivedPassword))
			{
				// Show error and ask for password again
				if (MessageBox.ErrorQuery("Incorrect password", "The password you typed was not correct. Do you want to try again?", "Cancel", "Try again") == 1)
				{
					string primaryKeyIdentifier = commonSecretsContainer.keyDerivationFunctionEntries[0].GetKeyIdentifier();
					var askForSinglePassword = AskForSinglePasswordDialog.CreateAskForSinglePasswordDialog(primaryKeyIdentifier, SinglePasswordAsked, null);
					Application.Run(askForSinglePassword);
					return;
				}
			}
		}

		// Success
		CommonSecretsPasswordStepCompleted(fileToOpenAbsolutePath);
		AddKnownDerivedPassword(commonSecretsContainer!.keyDerivationFunctionEntries[0].keyIdentifier, derivedPassword);
	}

	private static readonly LoginInformation sampleLogin = new LoginInformation("Example", "https://example.com/", "dragon@example.com", "Dragon", Path.GetTempFileName().Replace(".", "!"));

	private static void NewCommonSecretsCreated(KeyDerivationFunctionEntry keyDerivationFunctionEntry, string password)
	{
		try
		{
			// Derive password bytes, and store them into memory
			ClearKnownDerivedPasswords();
			byte[] derivedPassword = keyDerivationFunctionEntry.GeneratePasswordBytes(password);
			AddKnownDerivedPassword(keyDerivationFunctionEntry.keyIdentifier, derivedPassword);

			// Create new container
			commonSecretsContainer = new CommonSecretsContainer(keyDerivationFunctionEntry);
			string keyIdentifierString = Encoding.UTF8.GetString(keyDerivationFunctionEntry.keyIdentifier);
			commonSecretsContainer.AddLoginInformationSecret(derivedPassword, sampleLogin, keyIdentifierString);

			// Set state to unsaved
			fullFilePath = "";
			filename = defaultNameForNewFile;
			isContainerModified = true;

			fileFormat = CommonSecretsFileFormat.Unknown;

			//loginsTab.GetView().set
		}
		catch (Exception ex)
		{
			File.WriteAllText("ex.txt", ex.ToString());
		}
	}

	/// <summary>
	/// Absolute file path to file that should be opened after asking for password(s)
	/// </summary>
	private static string fileToOpenAbsolutePath = "";

	private static void OpenCommonSecretsFile()
	{
		// Check if we have unmodified data before we try to open another file
		if (isContainerModified)
		{
			if (MessageBox.Query("Save modifications", "Do you want to save modifications?", "Yes", "Cancel") == 1)
			{
				// TODO: Do actual save
			}
		}

		var allowedOpenFileExtensions = new List<string>() { ".json", ".xml" };
		var d = new OpenDialog("Open", "Open a CommonSecrets file", allowedOpenFileExtensions) { AllowsMultipleSelection = false };
		Application.Run(d);

		if (!d.Canceled)
		{
			string absoluteFilePath = d.FilePaths[0];
			// Try to open the file in path (only text serialization formats are supported for now)
			string fileText = File.ReadAllText(absoluteFilePath);

			// Check file format and try to deserialize it
			foreach (char c in fileText)
			{
				if (Char.IsWhiteSpace(c))
				{
					continue;
				}

				if (c == '{')
				{
					// It should be JSON
					commonSecretsContainer = JsonSerializer.Deserialize<CommonSecretsContainer>(fileText);
					fileFormat = CommonSecretsFileFormat.Json;
					break;
				}
				else if (c == '<')
				{
					// It should be XML
					var xmlserializer = new XmlSerializer(typeof(CommonSecretsContainer));
					using (XmlReader reader = XmlReader.Create(new StringReader(fileText)))
					{
						commonSecretsContainer = (CommonSecretsContainer?)xmlserializer.Deserialize(reader);
					}
					fileFormat = CommonSecretsFileFormat.Xml;
				}
				else
				{
					// TODO: Show error
					return;
				}
			}

			// If opening failed, return out (maybe show an error message?)
			if (commonSecretsContainer == null)
			{
				fileFormat = CommonSecretsFileFormat.Unknown;
				return;
			}

			// Ask for password(s)
			if (commonSecretsContainer.keyDerivationFunctionEntries.Count < 1)
			{
				// Do not ask for passwords
				CommonSecretsPasswordStepCompleted(absoluteFilePath);
			}
			else if (commonSecretsContainer.keyDerivationFunctionEntries.Count == 1)
			{
				fileToOpenAbsolutePath = absoluteFilePath;

				// Ask for single password
				string primaryKeyIdentifier = commonSecretsContainer.keyDerivationFunctionEntries[0].GetKeyIdentifier();
				var askForSinglePassword = AskForSinglePasswordDialog.CreateAskForSinglePasswordDialog(primaryKeyIdentifier, SinglePasswordAsked, null);
				Application.Run(askForSinglePassword);
			}
			else
			{
				// TODO: Ask for multiple passwords
			}
		}
	}

	private static void CommonSecretsPasswordStepCompleted(string absoluteFilePath)
	{
		fullFilePath = absoluteFilePath;
		filename = Path.GetFileName(fullFilePath);
		isContainerModified = false;
		ClearKnownDerivedPasswords();
	}

	private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
	{
		// TODO: Maybe do some configuration for this?
		WriteIndented = true
	};

	/// <summary>
	/// Save CommonSecrets file with existing settings
	/// </summary>
	private static void SaveCommonSecretsFile()
	{
		// Use same serialization format as existing file

		// Save to .tmp file

		// Replace the old file with new one
	}

	/// <summary>
	/// Save CommonSecrets file with settings that user chooses
	/// </summary>
	private static void SaveCommonSecretsFileAs()
	{
		var allowedSaveFileExtensions = new List<string>() { ".commonsecrets.json", ".commonsecrets.xml" };
		var sd = new SaveDialog("Save file", "Choose the path where to save the file.", allowedSaveFileExtensions);
		//sd.FilePath = System.IO.Path.Combine (sd.FilePath.ToString (), Win.Title.ToString ());
		Application.Run(sd);

		if (!sd.Canceled)
		{
			string? absoluteFilePath = sd.FilePath.ToString();

			if (string.IsNullOrWhiteSpace(absoluteFilePath))
			{
				// TODO: show error in here
				return;
			}

			if (System.IO.File.Exists(absoluteFilePath))
			{
				if (MessageBox.Query("Save File", "File already exists. Overwrite it any way?", "No", "Ok") == 1)
				{
					// Overwrite existing file
				}
				else
				{
					// Do nothing
				}
			}
			else
			{
				// Create a new file
				if (absoluteFilePath.EndsWith(".json"))
				{
					string json = JsonSerializer.Serialize(commonSecretsContainer, serializerOptions);
					File.WriteAllText(absoluteFilePath, json);
					fileFormat = CommonSecretsFileFormat.Json;
				}
				else if (absoluteFilePath.EndsWith(".xml"))
				{
					var xmlserializer = new XmlSerializer(typeof(CommonSecretsContainer));
					var stringWriter = new StringWriter();
					using (var writer = XmlWriter.Create(stringWriter))
					{
						xmlserializer.Serialize(writer, commonSecretsContainer);
						string xml = stringWriter.ToString();
						File.WriteAllText(absoluteFilePath, xml);
					}
					fileFormat = CommonSecretsFileFormat.Xml;
				}
			}
		}
		else
		{
			// Saving canceled
		}
	}

	/// <summary>
	/// Try to close file (check if there are any changes that should be saved)
	/// </summary>
	/// <returns>True if close was success; False otherwise</returns>
	private static bool TryToCloseFile()
	{
		// Check if there are unsaved modifications
		if (isContainerModified)
		{
			int pressedButton = MessageBox.Query("Save modifications", "Do you want to save modifications?", "Yes", "No", "Cancel");
			if (pressedButton == 1)
			{
				ClearKnownDerivedPasswords();
				// TODO: Do the actual save here
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			ClearKnownDerivedPasswords();
			return true;
		}
	}

	private static void TryToQuit()
	{
		// Check if there are unsaved modifications
		if (isContainerModified)
		{
			int pressedButton = MessageBox.Query("Save modifications", "Do you want to save modifications?", "Yes", "No", "Cancel");
			if (pressedButton == 1)
			{
				ClearKnownDerivedPasswords();
				// TODO: Do the actual save here

			}
		}

		Application.RequestStop();
	}

	private static readonly TimeSpan statusItemsWaitTime = TimeSpan.FromMilliseconds(500);

	private static readonly (StatusItem[], VisibleElement) usernameCopiedEffect = (new StatusItem[] { new StatusItem(Key.Null, "Username copied to clipboard", () => { }) }, VisibleElement.ShowLoginInformations);

	private static readonly Dictionary<VisibleElement, StatusItem[]> statusItems = new()
	{
		{ VisibleElement.ShowLoginInformations, new StatusItem[] {
			new StatusItem(Key.F5, $"~F5~ {LocMan.Get("Open URL")}", () => {}),
			new StatusItem(Key.F6, $"~F6~ {LocMan.Get("Copy URL")}", () => {}),
			new StatusItem(Key.F7, $"~F7~ {LocMan.Get("Copy Username")}", () => { CopyUsername(); TryToExecuteStatusItemsTimedEffect(usernameCopiedEffect); }),
			new StatusItem(Key.F8, $"~F8~ {LocMan.Get("Copy Password")}", () => { CopyPassword(); }),
		}},

		{ VisibleElement.ShowNotes, new StatusItem[] {
			new StatusItem(Key.F7, $"~F7~ {LocMan.Get("Copy Title")}", () => {}),
			new StatusItem(Key.F8, $"~F8~ {LocMan.Get("Copy Text")}", () => {}),
		}},
	};

	private static readonly StatusItem[] emptyItems = new StatusItem[0];

	public static StatusItem[] GetStatusItems(VisibleElement visibleElement)
	{
		if (statusItems.ContainsKey(visibleElement))
		{
			return statusItems[visibleElement];
		}

		return emptyItems;
	}

	private static void TryToExecuteStatusItemsTimedEffect((StatusItem[], VisibleElement) nextEffect)
	{
		statusBar.Items = nextEffect.Item1;
		Application.Refresh();
		Thread thread1 = new Thread(() => WaitAndReturnToChosenStatusItems(nextEffect.Item2));
		thread1.Start();
	}

	private static void WaitAndReturnToChosenStatusItems(VisibleElement returnToThis)
	{
		Thread.Sleep(statusItemsWaitTime);
		statusBar.Items = GetStatusItems(returnToThis);
		Application.Refresh();
	}

	private static void ClearKnownDerivedPasswords()
	{
		// Fill values with zeroes (most likely unneeded feature)
		foreach (byte[] val in knownDerivedPasswords.Values)
		{
			for (int i = 0; i < val.Length; i++)
			{
				val[i] = 0;
			}
		}
		knownDerivedPasswords.Clear();
	}

	private static void AddKnownDerivedPassword(byte[] id, byte[] derivedPassword)
	{
		knownDerivedPasswords[id] = derivedPassword;
	}
}
