using Terminal.Gui;
using System;

namespace WhisperDragonCLI
{
	public static class AddOrEditLoginInformationDialog
	{
		public static Dialog CreateDialog(bool editMode, Action okAction, Action cancelAction)
		{
			string title = editMode ? "Edit login information" : "Add login information";

			string okActionText = editMode ? "Edit" : "Add";

			var ok = new Button(1, 14, LocMan.Get(okActionText));
			ok.Clicked += () => { okAction.Invoke(); };

			var cancel = new Button(1, 14, LocMan.Get("Cancel"));
			cancel.Clicked += () => { cancelAction.Invoke(); };

			var isSecure = new CheckBox(1, 4, "Is Secure", true);


			var loginTitleLabel = new Label(1, 1, LocMan.Get("Login title:"));
			var loginTitleTextField = new TextField("") 
			{
				X = 1, 
				Y = 2,
				Width = Dim.Fill(),
				Height = 1
			};

			var loginURLLabel = new Label(1, 1, LocMan.Get("Login URL:"));
			var loginURLTextField = new TextField("") 
			{
				X = 1, 
				Y = 2,
				Width = Dim.Fill(),
				Height = 1
			};

			var loginEmailLabel = new Label(1, 1, LocMan.Get("Login email:"));
			var loginEmailTextField = new TextField("") 
			{
				X = 1, 
				Y = 2,
				Width = Dim.Fill(),
				Height = 1
			};

			var loginUsernameLabel = new Label(1, 1, LocMan.Get("Login username:"));
			var loginUsernameTextField = new TextField("") 
			{
				X = 1, 
				Y = 2,
				Width = Dim.Fill(),
				Height = 1
			};

			var loginPasswordLabel = new Label(1, 1, LocMan.Get("Login password:"));
			var loginPasswordTextField = new TextField("") 
			{
				Secret = true,
				X = 1, 
				Y = 2,
				Width = Dim.Fill(),
				Height = 1
			};

			var loginNotesLabel = new Label(1, 1, LocMan.Get("Login notes:"));
			var loginNotesTextField = new TextField("") 
			{
				X = 1, 
				Y = 2,
				Width = Dim.Fill(),
				Height = 5
			};

			var dialog = new Dialog(LocMan.Get(title), 60, 17, ok, cancel);

			dialog.Add(isSecure, loginTitleLabel, loginTitleTextField, loginURLLabel, loginURLTextField, loginEmailLabel, loginEmailTextField, loginUsernameLabel, loginUsernameTextField, loginPasswordLabel, loginPasswordTextField, loginNotesLabel, loginNotesTextField);

			return dialog;
		}
	}
}