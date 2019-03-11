using System;
using Terminal.Gui;

namespace WhisperDragonCLI
{
	class Program
	{
		private bool isFileOpen = false;

		static void Main(string[] args)
		{
			Application.Init ();
			var menu = new MenuBar (new MenuBarItem [] {
				new MenuBarItem ("_File", new MenuItem [] {
					new MenuItem("_New", "", () => 
					{
						var createNew = CreateNewDialog.CreateNewFileDialog(() => Application.RequestStop(), () => Application.RequestStop()); 
						Application.Run(createNew);
					}),
					new MenuItem("_Open", "", () => {}),
					new MenuItem("_Save", "", () => {}),
					new MenuItem("_Quit", "", () => { 
						Application.RequestStop (); 
					})
				}),
			});

			var win = new Window ("Hello") {
				X = 0,
				Y = 1,
				Width = Dim.Fill (),
				Height = Dim.Fill () - 1
			};

			// Add both menu and win in a single call
			Application.Top.Add (menu, win);
			Application.Run ();
		}
	}
}
