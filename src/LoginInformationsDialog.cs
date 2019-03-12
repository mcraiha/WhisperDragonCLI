using Terminal.Gui;

namespace WhisperDragonCLI
{
	public static class LoginInformationWindow
	{
		public static Window CreateLoginInformationDialog()
		{
			return new Window(LocMan.Get("Login informations"));
		}
	}
}