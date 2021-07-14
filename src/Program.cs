using System;
using Terminal.Gui;

namespace WhisperDragonCLI
{
	class Program
	{
		private bool isFileOpen = false;
		private bool isFileModified = false;

		private string fullFilePath = "";

		private string filename = "";

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
					new MenuItem("_Open...", LocMan.Get("Open existing CommonSecrets file..."), () => {}),
					new MenuItem("_Save", "Save CommonSecrets file", () => {}),
					new MenuItem("Save As...", "Save CommonSecrets file as...", () => {}),
					new MenuItem("_Quit", "Quit", () => { 
						Application.RequestStop (); 
					})
				}),

				new MenuBarItem("_Edit", new MenuItem[] {

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
			win.Add(tabView);

			// Add both menu and win in a single call
			Application.Top.Add (menu, win);
			Application.Run ();
		}
	}
}
