using Terminal.Gui;

namespace WhisperDragonCLI
{
	public static class CreatePasswordSelectionView
	{
		public static Window CreateView()
		{
			var win = new Window("Create password") {
				X = 0,
				Y = 1,
				Width = Dim.Fill (),
				Height = Dim.Fill () - 1
			};

			//LoginInformationsWindow.CreateLoginInformationsDialog(win);
			TabView tabView = new TabView()
			{
				X = 0,
				Y = 0,
				Width = Dim.Fill (),
				Height = Dim.Fill (1)
			};
			tabView.AddTab(new TabView.Tab("🎲 Random", PasswordGeneratorView.CreateView()), true);
			tabView.AddTab(new TabView.Tab("💬 Pronounceable", new ListView()), false);
			win.Add(tabView);

			return win;
		}
	}
}