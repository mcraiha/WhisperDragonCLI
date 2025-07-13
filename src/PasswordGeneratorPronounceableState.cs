
namespace WhisperDragonCLI;

public sealed class PasswordGeneratorPronounceableState
{
	public int HowManyWords { get; set; } = 2;

	public bool StartWithUpperCase { get; set; } = true;

	public bool IncludeNumbers { get; set; } = true;

	public bool IncludeSpecialCharSimple { get; set; } = true;

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
			GeneratedPronounceablePassword = generatedPronounceablePassword;
		} 
	}

	private string generatedPronounceablePassword = "";
	public string GeneratedPronounceablePassword 
	{ 
		get
		{
			if (VisiblePassword)
			{
				return this.generatedPronounceablePassword;
			}
			
			return string.Create(this.generatedPronounceablePassword.Length, '*', (chars, buf) => {
																		for (int i=0; i< chars.Length; i++) chars[i] = buf;
					});
		}
		set         
		{
			this.generatedPronounceablePassword = value;
		}
	}

	public string GetActualdPronounceablePassword()
	{
		return this.generatedPronounceablePassword;
	}
}
