using Terminal.Gui;
using System;

namespace WhisperDragonCLI;

public static class AddOrEditLoginInformationDialog
{
	public static Dialog CreateDialog(bool editMode, Action okAction, Action cancelAction)
	{
		string title = editMode ? "Edit login information" : "Add login information";

		string okActionText = editMode ? "Edit" : "Add";

		var ok = new Button(LocMan.Get(okActionText));
		ok.Clicked += () => { okAction.Invoke(); };

		var cancel = new Button(LocMan.Get("Cancel"));
		cancel.Clicked += () => { cancelAction.Invoke(); };

		var isSecure = new CheckBox("Is Secure", true);

		var loginTitleLabel = new Label(LocMan.Get("Login title:"))
		{
			X = 0,
			Y = Pos.Bottom(isSecure) + 1,
		};
		var loginTitleTextField = new TextField("") 
		{
			X = 0,
			Y = Pos.Bottom(loginTitleLabel),
			Width = Dim.Fill(),
			Height = 1
		};

		var loginURLLabel = new Label(LocMan.Get("Login URL:"))
		{
			X = 0,
			Y = Pos.Bottom(loginTitleTextField) + 1,
		};
		var loginURLTextField = new TextField("") 
		{
			X = 0, 
			Y = Pos.Bottom(loginURLLabel),
			Width = Dim.Fill(),
			Height = 1
		};

		var loginEmailLabel = new Label(LocMan.Get("Login email:"))
		{
			X = 0,
			Y = Pos.Bottom(loginURLTextField) + 1,
		};
		var loginEmailTextField = new TextField("") 
		{
			X = 0, 
			Y = Pos.Bottom(loginEmailLabel),
			Width = Dim.Fill(),
			Height = 1
		};

		var loginUsernameLabel = new Label(LocMan.Get("Login username:"))
		{
			X = 0,
			Y = Pos.Bottom(loginEmailTextField) + 1,
		};
		var loginUsernameTextField = new TextField("") 
		{
			X = 0, 
			Y = Pos.Bottom(loginUsernameLabel),
			Width = Dim.Fill(),
			Height = 1
		};

		var loginPasswordLabel = new Label(LocMan.Get("Login password:"))
		{
			X = 0,
			Y = Pos.Bottom(loginUsernameTextField) + 1,
		};
		var loginPasswordTextField = new TextField("") 
		{
			Secret = true,
			X = 0, 
			Y = Pos.Bottom(loginPasswordLabel),
			Width = Dim.Fill(),
			Height = 1
		};

		var loginNotesLabel = new Label(LocMan.Get("Login notes:"))
		{
			X = 0,
			Y = Pos.Bottom(loginPasswordTextField) + 1,
		};
		var loginNotesTextView = new TextView()
		{
			X = 0, 
			Y = Pos.Bottom(loginNotesLabel),
			Width = Dim.Fill(),
			Height = 3
		};

		var dialog = new Dialog(LocMan.Get(title), 0, 0, ok, cancel);

		dialog.Add(isSecure, loginTitleLabel, loginTitleTextField, loginURLLabel, loginURLTextField, loginEmailLabel, loginEmailTextField, loginUsernameLabel, loginUsernameTextField, loginPasswordLabel, loginPasswordTextField, loginNotesLabel, loginNotesTextView);

		return dialog;
	}
}
