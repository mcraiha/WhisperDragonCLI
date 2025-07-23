using System;
using Terminal.Gui;
using System.Collections.Generic;
using System.IO;
using System.Threading;

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

class Program
{
	private static bool isFileOpen = false;
	private static bool isFileModified = false;

	private static string fullFilePath = "";

	private static string filename = "*Unsaved*";

	private static TabWithId loginsTab;

	private static StatusBar statusBar;

	static void Main(string[] args)
	{
		Application.Init();
		var menu = new MenuBar(new MenuBarItem[] {
			new MenuBarItem("_File", new MenuItem [] {
				new MenuItem("_New...", LocMan.Get("New CommonSecrets file..."), () =>
				{
					var createNew = NewFileDialog.CreateNewFileDialog(() => Application.RequestStop(), () => Application.RequestStop());
					Application.Run(createNew);
				}),
				new MenuItem("_Open...", LocMan.Get("Open existing CommonSecrets file..."), () => OpenCommonSecretsFile()),
				new MenuItem("_Save", "Save CommonSecrets file", () => {}, () => isFileOpen),
				new MenuItem("Save As...", "Save CommonSecrets file as...", () => SaveCommonSecretsFileAs(), () => isFileOpen),
				new MenuItem("_Close", "Close file", () => {
					TryToCloseFile();
				}, () => isFileOpen),
				new MenuItem("_Quit", "Quit", () => {
					TryToQuit();
				})
			}),

			new MenuBarItem("_Edit", new MenuItem[] {

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
		win.Add(tabView);

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

	private static void OpenCommonSecretsFile()
	{
		// Check if we have unmodified data before we try to open another file


		var allowedOpenFileExtensions = new List<string>() { ".json", ".xml" };
		var d = new OpenDialog("Open", "Open a CommonSecrets file", allowedOpenFileExtensions) { AllowsMultipleSelection = false };
		Application.Run(d);

		if (!d.Canceled)
		{
			// Try to open the file in path

			// Success, fill UI things
			fullFilePath = d.FilePaths[0];
			filename = Path.GetFileName(fullFilePath);
			isFileModified = false;
		}
	}

	private static void SaveCommonSecretsFileAs()
	{
		var allowedSaveFileExtensions = new List<string>() { ".json", ".xml" };
		var sd = new SaveDialog("Save file", "Choose the path where to save the file.", allowedSaveFileExtensions);
		//sd.FilePath = System.IO.Path.Combine (sd.FilePath.ToString (), Win.Title.ToString ());
		Application.Run(sd);

		if (!sd.Canceled)
		{
			if (System.IO.File.Exists(sd.FilePath.ToString()))
			{
				if (MessageBox.Query("Save File", "File already exists. Overwrite any way?", "No", "Ok") == 1)
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
			}
		}
		else
		{
			// Saving canceled
		}
	}

	private static void TryToCloseFile()
	{
		// Check if there are unsaved modifications
		if (isFileModified)
		{

		}
		else
		{

		}
	}

	private static void TryToQuit()
	{
		// Check if there are unsaved modifications
		if (isFileModified)
		{

		}
		else
		{
			Application.RequestStop();
		}
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
}
