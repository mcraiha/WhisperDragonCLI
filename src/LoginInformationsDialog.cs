using Terminal.Gui;
using System.Collections.Generic;

namespace WhisperDragonCLI
{
	public static class LoginInformationsWindow
	{
		public static void CreateLoginInformationsDialog(Window parentWindow)
		{
			SectionBar.AddSectionBar(parentWindow, ContainerSection.LoginInformations);

			List<string> stringList = new List<string>() { "some what long string", "and nice collections to show", "for users"};

			ListView listView = new ListView(new Rect(0, 1, 20, 10), stringList);
			parentWindow.Add(listView);
		}
	}
}