using System;
using Terminal.Gui;

namespace WhisperDragonCLI
{
	public static class NewFileDialog
	{
		private static readonly int suggestedIterations = 100_000;
		private static readonly string[] prfArray = new string[] {"SHA-512", "SHA-256"};

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

			var dialog = new Dialog(LocMan.Get("Pbkdf2 settings"), 60, 20, ok, cancel);


			var enterPasswordLabel = new Label(1, 1, LocMan.Get("Enter password:"));

			var passwordFirstTime = new TextField("") 
			{
				Secret = true,
				X = 1, 
				Y = 2,
				Width = Dim.Fill(),
				Height = 1
			};


			var confirmPasswordLabel = new Label(1, 4, LocMan.Get("Confirm password:"));

			var passwordSecondTime = new TextField("") 
			{
				Secret = true,
				X = 1, 
				Y = 5,
				Width = Dim.Fill(),
				Height = 1
			};


			var prfLabel = new Label(1, 7, LocMan.Get("Pseudo-Random Function:"));

			var prfRadioSelection = new RadioGroup(1, 8, prfArray);

			
			var iterationsLabel = new Label(1, 11, LocMan.Get("How many iterations:"));

			var iterationsAmount = new TextField (suggestedIterations.ToString()) 
			{
				X = 1, 
				Y = 12,
				Width = Dim.Fill(),
				Height = 1
			};


			dialog.Add(enterPasswordLabel, passwordFirstTime, confirmPasswordLabel, passwordSecondTime, prfLabel, prfRadioSelection, iterationsLabel, iterationsAmount);

			return dialog;
		}
	}
}