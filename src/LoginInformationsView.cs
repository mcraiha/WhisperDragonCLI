using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI
{
	public static class LoginInformationsView
	{
		public static View CreateView()
		{
			var dt = new DataTable();

			foreach (string columnName in GetColumnNames())
			{
				dt.Columns.Add(columnName);
			}

			List<string> firstRow = new List<string>() { "Yes", "Fake service", "https://fakeservice.com", "sample@email.com", "Dragon", "gwWTY#¤&%36" };
			dt.Rows.Add(firstRow.ToArray());

			List<string> secondRow = new List<string>() { "No", "Fake mail", "https://fakemail.com", "sample@email.com", "Dragon", "Si0bSww5bYeKp7Rs" };
			dt.Rows.Add(secondRow.ToArray());

			TableView tableView = new TableView() 
			{
				X = 0,
				Y = 0,
				Width = Dim.Fill(),
				Height = 10,
			};

			tableView.Table = dt;

			/*tableView.MouseClick += (m) => {
				m.MouseEvent.
			};*/

			tableView.KeyUp += (m) => {
				if (m.KeyEvent.Key == Key.F5)
				{
					Clipboard.TrySetClipboardData((string)dt.Rows[tableView.SelectedRow]["URL"]);
				}
				
			};

			return tableView;
		}

		public static List<string> GetColumnNames()
		{
			return new List<string>() { "Secure", "Title", "URL", "Email", "Username", "Password" };
		}


	}
}