using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI
{
	public static class ContactsView
	{
		public static View CreateView()
		{
			var dt = new DataTable();

			foreach (string columnName in GetColumnNames())
			{
				dt.Columns.Add(columnName);
			}

			List<string> firstRow = new List<string>() { "Yes", "Hanna", "Hard Worker", "hanna@localhost (work)", "1-800-123123 (work)" };
			dt.Rows.Add(firstRow.ToArray());

			List<string> secondRow = new List<string>() { "No", "Mike", "Madr", "mike@localhost (work)", "1-800-123321 (home)" };
			dt.Rows.Add(secondRow.ToArray());

			TableView tableView = new TableView() 
			{
				X = 0,
				Y = 0,
				Width = Dim.Fill(),
				Height = 10,
			};

			tableView.Table = dt;

			return tableView;
		}

		public static List<string> GetColumnNames()
		{
			return new List<string>() { "Secure", "First name", "Last name", "Email(s)", "Phone number(s)" };
		}
	}
}