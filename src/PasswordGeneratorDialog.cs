using Terminal.Gui;
using Terminal.Gui.TextValidateProviders;
using System;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace WhisperDragonCLI;

public static class PasswordGeneratorDialog
{
	private static readonly List<char> upperCaseLatinLetters = new List<char>()
	{
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
	};

	private static readonly List<char> lowerCaseLatinLetters = new List<char>()
	{
		'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
	};

	private static readonly List<char> digits = new List<char>()
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
	};

	private static readonly List<char> specialCharactersASCII = new List<char>()
	{
		'!', '"', '#', '$', '%', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~'
	};

	// Generated during runtime in ConstructEmojiList(), See https://en.wikipedia.org/wiki/Emoticons_(Unicode_block)
	private static readonly List<string> emoticonsUnicodeBlock = new List<string>();

	public static Dialog CreateDialog(Action okAction)
	{
		PasswordGeneratorState state = new PasswordGeneratorState();

		var ok = new Button(1, 14, LocMan.Get("Ok"));
		ok.Clicked += () => { okAction.Invoke(); };

		var dialog = new Dialog(LocMan.Get("🎲 Random Password Generator"), 60, 17, ok);

		var passwordLengthLabel = new Label(1, 1, LocMan.Get("Password length:"));

		var positiveNumbersOnlyProvider = new TextRegexProvider("^[1-9]+[0-9]$") { ValidateOnInput = true };
		var passwordLengthTextField = new TextValidateField(positiveNumbersOnlyProvider) 
		{
			X = 1, 
			Y = 2,
			Width = 4,
			Height = 1
		};
		passwordLengthTextField.Text = state.PasswordLength.ToString();


		var useUppercase = new CheckBox(1, 4, "Upper-case latin characters (e.g. A, C, K, Z)", state.IncludeUpperCaseLatinLetters);
		useUppercase.Toggled += (bool old) => { state.IncludeUpperCaseLatinLetters = !old; };

		var useLowercase = new CheckBox(1, 5, "Lower-case latin characters (e.g. a, c, k, z)", state.IncludeLowerCaseLatinLetters);
		useLowercase.Toggled += (bool old) => { state.IncludeLowerCaseLatinLetters = !old; };

		var useDigits = new CheckBox(1, 6, "Digits (e.g. 4, 6, 9)", state.IncludeDigits);
		useDigits.Toggled += (bool old) => { state.IncludeDigits = !old; };

		var useSpecialASCII = new CheckBox(1, 7, "Special characters ASCII", state.IncludeSpecialCharactersASCII);
		useSpecialASCII.Toggled += (bool old) => { state.IncludeSpecialCharactersASCII = !old; };

		var useBasicEmojis = new CheckBox(1, 8, "Basic emoji (e.g. 😊)", state.IncludeEmojis);
		useBasicEmojis.Toggled += (bool old) => { state.IncludeEmojis = !old; };


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
		copyToClipboardButton.Clicked += () => { Clipboard.TrySetClipboardData(state.GetActualPassword()); };

		var generateButton = new Button(23, 12, LocMan.Get("Generate"));
		generateButton.Clicked += () => { GenerateRandomPassword(state); generatedPasswordField.Text = state.GeneratedPassword; copyToClipboardButton.Enabled = true; };

		var visiblePassword = new CheckBox(23, 13, "Visible", state.VisiblePassword);
		visiblePassword.Toggled += (bool old) => { state.VisiblePassword = !old; generatedPasswordField.Text = state.GeneratedPassword; };

		dialog.Add(passwordLengthLabel, passwordLengthTextField, useUppercase, useLowercase, useDigits, useSpecialASCII, useBasicEmojis, generatedPasswordLabel, generatedPasswordField, copyToClipboardButton, generateButton, visiblePassword);

		return dialog;
	}

	private static void GenerateRandomPassword(PasswordGeneratorState state)
	{
		List<string> generated = new List<string>(state.PasswordLength);

		List<string> possibleChars = new List<string>();

		using (var generator = RandomNumberGenerator.Create())
		{
			if (state.IncludeUpperCaseLatinLetters)
			{
				int index = GetPositiveRandomInt(generator) % upperCaseLatinLetters.Count;
				generated.Add(upperCaseLatinLetters[index].ToString());
				possibleChars.AddRange(Array.ConvertAll<char, string>(upperCaseLatinLetters.ToArray(), element => element.ToString()));
			}

			if (state.IncludeLowerCaseLatinLetters)
			{
				int index = GetPositiveRandomInt(generator) % lowerCaseLatinLetters.Count;
				generated.Add(lowerCaseLatinLetters[index].ToString());
				possibleChars.AddRange(Array.ConvertAll<char, string>(lowerCaseLatinLetters.ToArray(), element => element.ToString()));
			}

			if (state.IncludeDigits)
			{
				int index = GetPositiveRandomInt(generator) % digits.Count;
				generated.Add(digits[index].ToString());
				possibleChars.AddRange(Array.ConvertAll<char, string>(digits.ToArray(), element => element.ToString()));
			}

			if (state.IncludeSpecialCharactersASCII)
			{
				int index = GetPositiveRandomInt(generator) % specialCharactersASCII.Count;
				generated.Add(specialCharactersASCII[index].ToString());
				possibleChars.AddRange(Array.ConvertAll<char, string>(specialCharactersASCII.ToArray(), element => element.ToString()));
			}

			if (state.IncludeEmojis)
			{
				if (emoticonsUnicodeBlock.Count < 1)
				{
					ConstructEmojiList();
				}

				int index = GetPositiveRandomInt(generator) % emoticonsUnicodeBlock.Count;
				generated.Add(emoticonsUnicodeBlock[index]);
				possibleChars.AddRange(emoticonsUnicodeBlock);
			}

			// Reorder all possible chars
			possibleChars = possibleChars.OrderBy(x => GetPositiveRandomInt(generator)).ToList();

			while (generated.Count < state.PasswordLength)
			{
				int index = GetPositiveRandomInt(generator) % possibleChars.Count;
				generated.Add(possibleChars[index]);
			}

			// Reorder all generated chars
			generated = generated.OrderBy(x => GetPositiveRandomInt(generator)).ToList();
		}

		state.GeneratedPassword = string.Join("", generated); //  new String( generated.ToArray());
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

	private static void ConstructEmojiList()
	{
		int startValue = 0x1F600;
		emoticonsUnicodeBlock.Add(Char.ConvertFromUtf32(startValue));
		for (int i = 0; i < 80; i++)
		{
			startValue++;
			emoticonsUnicodeBlock.Add(Char.ConvertFromUtf32(startValue));
		}
	}

}
