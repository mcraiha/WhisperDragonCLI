using Terminal.Gui;

namespace WhisperDragonCLI
{
	public static class SectionBar
	{
		private static Label loginInformationsLabel = new Label(1, 0, LocMan.Get("Login informations"));
		private static Button loginInformationsButton = new Button(1, 0, LocMan.Get("Login informations"));

		private static Label notesLabel = new Label(20, 0, LocMan.Get("Notes"));
		private static Button notesButton = new Button(20, 0, LocMan.Get("Notes"));

		private static Label filesLabel = new Label(26, 0, LocMan.Get("Files"));
		private static Button filesButton = new Button(30, 0, LocMan.Get("Files"));

		public static void AddSectionBar(Window window, ContainerSection section)
		{
			if (section == ContainerSection.None)
			{
				window.Add(loginInformationsLabel, notesLabel, filesLabel);
			}
			else if (section == ContainerSection.LoginInformations)
			{
				window.Add(loginInformationsLabel, notesButton, filesButton);
			}
		}
	}
}