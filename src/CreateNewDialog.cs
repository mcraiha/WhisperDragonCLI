using System;
using Terminal.Gui;

namespace WhisperDragonCLI
{
	public static class CreateNewDialog
	{
		public static Dialog CreateNewFileDialog(Action okAction, Action cancelAction)
		{
			var ok = new Button(3, 14, LocMan.Get("Ok")) 
			{ 
				Clicked = okAction
			};

			var cancel = new Button(10, 14, LocMan.Get("Cancel")) 
			{
				Clicked = cancelAction
			};

			var dialog = new Dialog (LocMan.Get("Login"), 60, 18, ok, cancel);

			var passwordFirstTime = new TextField ("") 
			{
				Secret = true,
				X = 1, 
				Y = 1,
				Width = Dim.Fill (),
				Height = 1
			};

			var passwordSecondTime = new TextField ("") 
			{
				Secret = true,
				X = 1, 
				Y = 2,
				Width = Dim.Fill (),
				Height = 1
			};

			dialog.Add(passwordFirstTime, passwordSecondTime);

			return dialog;
		}
	}
}