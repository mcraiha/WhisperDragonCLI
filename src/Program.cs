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
			tabView.AddTab(new TabView.Tab("Login informations", LoginInformationsView.CreateView()), true);
			tabView.AddTab(new TabView.Tab("Notes", new ListView()), false);
			tabView.AddTab(new TabView.Tab("Files", new ListView()), false);
			tabView.AddTab(new TabView.Tab("Contacts", new ListView()), false);
			tabView.AddTab(new TabView.Tab("Payment cards", new ListView()), false);
			win.Add(tabView);

			// Add both menu and win in a single call
			Application.Top.Add (menu, win);
			Application.Run ();
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
