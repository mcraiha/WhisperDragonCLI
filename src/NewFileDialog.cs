using System;
using Terminal.Gui;
using Terminal.Gui.TextValidateProviders;
using NStack;

namespace WhisperDragonCLI;

public sealed class NewFileDialog : Dialog
{
	private static readonly int suggestedIterations = 100_000;
	private static readonly ustring[] prfArray = new ustring[] {"SHA-512", "SHA-256"};

	private TextField password1Field;
	private TextField password2Field;

	private NewFileDialog(Button okButton, Button cancelButton, TextField password1, TextField password2) : base(LocMan.Get("Create new CommonSecrets file"), 60, 20, okButton, cancelButton)
	{
		this.password1Field = password1;
		this.password2Field = password2;
	}

	private bool DoPasswordsMatch()
	{
		return this.password1Field.Text == this.password2Field.Text;
	}

	private byte[] GetDerivedPassword()
	{
		return new byte[] { };
	}

	public static NewFileDialog CreateNewFileDialog(Action<int, byte[]>? okAction, Action? cancelAction)
	{
		var ok = new Button(3, 16, LocMan.Get("Ok"));

		var cancel = new Button(10, 16, LocMan.Get("Cancel"));
		cancel.Clicked += () => { cancelAction?.Invoke(); Application.RequestStop(); };

		var enterPasswordLabel = new Label(1, 1, LocMan.Get("Enter primary password:"));

		var passwordFirstTime = new TextField("")
		{
			Secret = true,
			X = 1,
			Y = 2,
			Width = Dim.Fill(),
			Height = 1
		};


		var confirmPasswordLabel = new Label(1, 4, LocMan.Get("Confirm primary password:"));

		var passwordSecondTime = new TextField("")
		{
			Secret = true,
			X = 1,
			Y = 5,
			Width = Dim.Fill(),
			Height = 1
		};

		var dialog = new NewFileDialog(ok, cancel, passwordFirstTime, passwordSecondTime);
		ok.Clicked += () => { okAction?.Invoke(1, dialog.GetDerivedPassword()); Application.RequestStop(); };

		var pbkdf2SettingsLabel = new Label(1, 7, LocMan.Get("- Pbkdf2 settings -"));

		var prfLabel = new Label(1, 9, LocMan.Get("Pseudo-Random Function:"));

		var prfRadioSelection = new RadioGroup(1, 10, prfArray);


		var iterationsLabel = new Label(1, 13, LocMan.Get("How many iterations:"));

		var positiveNumbersOnlyProvider = new TextRegexProvider("^[1-9]+[0-9]*$") { ValidateOnInput = true };
		var iterationsAmount = new TextValidateField(positiveNumbersOnlyProvider)
		{
			X = 1,
			Y = 14,
			Width = Dim.Fill(),
			Height = 1
		};
		iterationsAmount.Text = suggestedIterations.ToString();


		dialog.Add(enterPasswordLabel, passwordFirstTime, confirmPasswordLabel, passwordSecondTime, pbkdf2SettingsLabel, prfLabel, prfRadioSelection, iterationsLabel, iterationsAmount);

		return dialog;
	}
}