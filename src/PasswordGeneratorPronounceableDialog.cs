using Terminal.Gui;
using Terminal.Gui.TextValidateProviders;
using NStack;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace WhisperDragonCLI
{
	public static class PasswordGeneratorPronounceableDialog
	{
		private static readonly List<(string, string)> languageToCommonWords = new List<(string, string)>()
		{
			("English", "CommonWords/English-Common.txt")
		};

		private static readonly List<char> specialCharactersPronounceable = new List<char>()
		{
			'!', '#', '$', '%', '*', '+', '-', '.', '?', '@', '_',
		};

		public static Dialog CreateDialog(Action okAction)
		{
			PasswordGeneratorPronounceableState state = new PasswordGeneratorPronounceableState();

			var ok = new Button(1, 14, LocMan.Get("Ok"));
			ok.Clicked += () => { okAction.Invoke(); };

			var dialog = new Dialog(LocMan.Get("💬 Pronounceable Password Generator"), 60, 17, ok);

			var passwordLengthLabel = new Label(1, 1, LocMan.Get("How many words:"));

			var oneToTenProvider = new TextRegexProvider("^[1-9]|10$") { ValidateOnInput = true };
			var howManyWordsTextField = new TextValidateField(oneToTenProvider) 
			{
				X = 1, 
				Y = 2,
				Width = 2,
				Height = 1
			};
			howManyWordsTextField.Text = state.HowManyWords.ToString();


			var startWithUppercase = new CheckBox(1, 4, "Start with Upper-case (e.g. A, C, K or Z)", state.StartWithUpperCase);
			startWithUppercase.Toggled += (bool old) => { state.StartWithUpperCase = !old; };

			var includeNumbers = new CheckBox(1, 5, "Include number (e.g. 1, 15 or 76)", state.IncludeNumbers);
			includeNumbers.Toggled += (bool old) => { state.IncludeNumbers = !old; };

			var includeSpecialChar = new CheckBox(1, 6, "Include special character", state.IncludeSpecialCharSimple);
			includeSpecialChar.Toggled += (bool old) => { state.IncludeSpecialCharSimple = !old; };

			var generatedPasswordLabel = new Label(1, 10, LocMan.Get("Generated password:"));
			var generatedPasswordField = new TextField("") 
			{
				ReadOnly = true,
				X = 1, 
				Y = 11,
				Width = Dim.Fill(),
				Height = 1
			};


			var copyToClipboardButton = new Button(1, 12, LocMan.Get("Copy to Clipboard"));
			copyToClipboardButton.Enabled = false;
			copyToClipboardButton.Clicked += () => { Clipboard.TrySetClipboardData(state.GetActualdPronounceablePassword()); };

			var generateButton = new Button(23, 12, LocMan.Get("Generate"));
			generateButton.Clicked += () => { GeneratePronounceablePassword(state); generatedPasswordField.Text = state.GeneratedPronounceablePassword; copyToClipboardButton.Enabled = true; };

			var visiblePassword = new CheckBox(23, 13, "Visible", state.VisiblePassword);
			visiblePassword.Toggled += (bool old) => { state.VisiblePassword = !old; generatedPasswordField.Text = state.GeneratedPronounceablePassword; };

			dialog.Add(passwordLengthLabel, howManyWordsTextField, startWithUppercase, includeNumbers, includeSpecialChar, generatedPasswordLabel, generatedPasswordField, copyToClipboardButton, generateButton, visiblePassword);

			return dialog;
		}

		private static void GeneratePronounceablePassword(PasswordGeneratorPronounceableState state)
		{
			List<string> commonWords = EmbedResourceLoader.ReadAsList(languageToCommonWords[0].Item2);

			string currentPronounceablePassword = "";

			int wordCount = state.HowManyWords; 

			using (var generator = RandomNumberGenerator.Create())
			{
				int bigIndex = GetPositiveRandomInt(generator);
				int smallIndex = bigIndex % commonWords.Count;

				string firstWord = commonWords[smallIndex];

				if (state.StartWithUpperCase)
				{
					firstWord = char.ToUpper(firstWord[0]) + firstWord.Substring(1);
				}

				currentPronounceablePassword = firstWord;

				for (int i = 1; i < wordCount; i++)
				{
					bigIndex = GetPositiveRandomInt(generator);
					smallIndex = bigIndex % commonWords.Count;
					string tempWord = commonWords[smallIndex];

					currentPronounceablePassword += tempWord;
				}

				if (state.IncludeNumbers)
				{
					bigIndex = GetPositiveRandomInt(generator);
					smallIndex = bigIndex % 99;
					currentPronounceablePassword += smallIndex;
				}

				if (state.IncludeSpecialCharSimple)
				{
					int index = GetPositiveRandomInt(generator) % specialCharactersPronounceable.Count;
					currentPronounceablePassword += specialCharactersPronounceable[index];
				}		
			}

			state.GeneratedPronounceablePassword = currentPronounceablePassword;
		}

		private static int GetPositiveRandomInt(RandomNumberGenerator rng)
		{
			int returnValue = -1;

			byte[] byteArray = new byte[4];

			while (returnValue < 0)
			{
				rng.GetBytes(byteArray);
				returnValue = BitConverter.ToInt32(byteArray, 0);
			}

			return returnValue;
		}

	}
}