using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI
{
	public static class LoginInformationsView
	{
		public static View CreateView(List<LoginSimplified> logins)
		{
			var dt = new DataTable();

			foreach (string columnName in GetColumnNames())
			{
				dt.Columns.Add(columnName);
			}

			foreach (LoginSimplified ls in logins)
			{
				List<object> row = new List<object>() { ls.IsSecure, ls.Title, ls.URL, ls.Email, ls.Username, ls.Password};
				dt.Rows.Add(row.ToArray());
			}

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