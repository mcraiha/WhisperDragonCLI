using System;
using Terminal.Gui;
using Terminal.Gui.TextValidateProviders;
using NStack;
using CSCommonSecrets;

namespace WhisperDragonCLI;

public sealed class AskForSinglePasswordDialog : Dialog
{
	private TextField passwordField;

	private AskForSinglePasswordDialog(Button okButton, Button cancelButton, TextField password) : base(LocMan.Get("Input password"), 40, 8, okButton, cancelButton)
	{
		this.passwordField = password;
	}

	public string GetPassword()
	{
		return this.passwordField.Text.ToString()!;
	}

	public static AskForSinglePasswordDialog CreateAskForSinglePasswordDialog(string passwordIdentifier, Action<string>? okAction, Action? cancelAction)
	{
		var ok = new Button(3, 4, LocMan.Get("Ok"));

		var cancel = new Button(10, 4, LocMan.Get("Cancel"));
		cancel.Clicked += () => { cancelAction?.Invoke(); Application.RequestStop(); };

		var enterPasswordLabel = new Label(1, 1, string.Format(LocMan.Get("Enter '{0}' password:"), passwordIdentifier));

		var password = new TextField("")
		{
			Secret = true,
			X = 1,
			Y = 2,
			Width = Dim.Fill(),
			Height = 1
		};

		var dialog = new AskForSinglePasswordDialog(ok, cancel, password);
		ok.Clicked += () => { okAction?.Invoke(dialog.GetPassword()); Application.RequestStop(); };

		dialog.Add(enterPasswordLabel, password);

		return dialog;
	}
}