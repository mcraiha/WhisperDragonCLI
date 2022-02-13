using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI
{
	public static class LoginInformationsStatusBar
	{
		public static StatusBar CreateStatusBar()
		{
			StatusBar statusBar = new StatusBar(new StatusItem [] {
				new StatusItem(Key.F5 | Key.R, $"~F5~ {LocMan.Get("Open URL")}", () => {}),
				new StatusItem(Key.F6 | Key.R, $"~F6~ {LocMan.Get("Copy URL")}", () => {}),
				new StatusItem(Key.F7 | Key.R, $"~F7~ {LocMan.Get("Copy Username")}", () => {}),
				new StatusItem(Key.F8 | Key.R, $"~F8~ {LocMan.Get("Copy Password")}", () => {}),
			});

			return statusBar;
		}
	}
}