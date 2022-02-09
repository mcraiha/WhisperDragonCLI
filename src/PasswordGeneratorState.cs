
namespace WhisperDragonCLI
{
	public sealed class PasswordGeneratorState
	{
		public int PasswordLength { get; set; } = 16;

		public bool IncludeUpperCaseLatinLetters { get; set; } = true;

		public bool IncludeLowerCaseLatinLetters { get; set; } = true;

		public bool IncludeDigits { get; set; } = true;

		public bool IncludeSpecialCharactersASCII { get; set; } = true;

		public bool IncludeEmojis { get; set; } = false;

		private bool visiblePassword = true;
		public bool VisiblePassword 
		{ 
			get
			{
				return this.visiblePassword;
			}
			
			set
			{
				this.visiblePassword = value;
				// Update to cause onpropertychange
				GeneratedPassword = generatedPassword;
			} 
		}

		private string generatedPassword = "";
		public string GeneratedPassword 
		{ 
			get
			{
				if (VisiblePassword)
				{
					return this.generatedPassword;
				}
				
				return string.Create(this.generatedPassword.Length, '*', (chars, buf) => {
																			for (int i=0; i< chars.Length; i++) chars[i] = buf;
						});
			}
			set         
			{
				this.generatedPassword = value;
			}
		}

		public string GetActualPassword()
		{
			return this.generatedPassword;
		}
	}
}
