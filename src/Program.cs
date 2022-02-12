using System;
using Terminal.Gui;
using System.Collections.Generic;

namespace WhisperDragonCLI
{
	class Program
	{
		private bool isFileOpen = false;
		private bool isFileModified = false;

		private static string fullFilePath = "";

		private static string filename = "";

		static void Main(string[] args)
		{
			Application.Init ();
			var menu = new MenuBar (new MenuBarItem [] {
				new MenuBarItem("_File", new MenuItem [] {
					new MenuItem("_New...", LocMan.Get("New CommonSecrets file..."), () => 
					{
						var createNew = NewFileDialog.CreateNewFileDialog(() => Application.RequestStop(), () => Application.RequestStop()); 
						Application.Run(createNew);
					}),
					new MenuItem("_Open...", LocMan.Get("Open existing CommonSecrets file..."), () => OpenCommonSecretsFile()),
					new MenuItem("_Save", "Save CommonSecrets file", () => {}),
					new MenuItem("Save As...", "Save CommonSecrets file as...", () => SaveCommonSecretsFileAs()),
					new MenuItem("_Close", "Close file", () => {}),
					new MenuItem("_Quit", "Quit", () => { 
						Application.RequestStop (); 
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
			});

			var win = new Window("Hello") {
				X = 0,
				Y = 1,
				Width = Dim.Fill (),
				Height = Dim.Fill () - 1
			};

			//LoginInformationsWindow.CreateLoginInformationsDialog(win);
			TabView tabView = new TabView()
			{
				X = 0,
				Y = 0,
				Width = Dim.Fill (),
				Height = Dim.Fill (1)
			};
			tabView.AddTab(new TabView.Tab("Login informations", LoginInformationsView.CreateView(GetTestLogins())), true);
			tabView.AddTab(new TabView.Tab("Notes", NotesView.CreateView()), false);
			tabView.AddTab(new TabView.Tab("Files", FilesView.CreateView()), false);
			tabView.AddTab(new TabView.Tab("Contacts", ContactsView.CreateView()), false);
			tabView.AddTab(new TabView.Tab("Payment cards", PaymentCardsView.CreateView()), false);
			win.Add(tabView);

			// Add both menu and win in a single call
			Application.Top.Add (menu, win);
			Application.Run ();
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

		private static void OpenCommonSecretsFile()
		{
			var d = new OpenDialog ("Open", "Open a CommonSecrets file") { AllowsMultipleSelection = false };
			Application.Run (d);

			if (!d.Canceled) {
				fullFilePath = d.FilePaths[0];
			}
		}

		private static void SaveCommonSecretsFileAs()
		{
			var allowedFileExtensions = new List<string> () { ".json", ".xml" };
			var sd = new SaveDialog ("Save file", "Choose the path where to save the file.", allowedFileExtensions);
			//sd.FilePath = System.IO.Path.Combine (sd.FilePath.ToString (), Win.Title.ToString ());
			Application.Run (sd);

			if (!sd.Canceled) 
			{
				if (System.IO.File.Exists (sd.FilePath.ToString ())) 
				{
					if (MessageBox.Query ("Save File", "File already exists. Overwrite any way?", "No", "Ok") == 1) 
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
	}
}
