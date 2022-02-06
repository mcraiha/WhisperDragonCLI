using Terminal.Gui;
using Terminal.Gui.TextValidateProviders;
using NStack;
using System;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace WhisperDragonCLI
{
	public static class PasswordGeneratorView
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

		private static readonly List<char> specialCharactersPronounceable = new List<char>()
		{
			'!', '#', '$', '%', '*', '+', '-', '.', '?', '@', '_',
		};

		// Generated during runtime in ConstructEmojiList(), See https://en.wikipedia.org/wiki/Emoticons_(Unicode_block)
		private static readonly List<string> emoticonsUnicodeBlock = new List<string>();

		public static View CreateView()
		{
			PasswordGeneratorState state = new PasswordGeneratorState();

			View returnValue = new View();

			var passwordLengthLabel = new Label(1, 1, LocMan.Get("Password length:"));

			var positiveNumbersOnlyProvider = new TextRegexProvider("^[1-9]+[0-9]$") { ValidateOnInput = true };
			var passwordLengthTextField = new TextValidateField(positiveNumbersOnlyProvider) 
			{
				X = 1, 
				Y = 12,
				Width = Dim.Fill(),
				Height = 1
			};
			passwordLengthTextField.Text = state.PasswordLength.ToString();

			returnValue.Add(passwordLengthTextField);

			return returnValue;
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
}