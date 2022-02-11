using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI
{
	public static class NotesView
	{
		public static View CreateView()
		{
			var dt = new DataTable();

			foreach (string columnName in GetColumnNames())
			{
				dt.Columns.Add(columnName);
			}

			List<string> firstRow = new List<string>() { "Yes", "Shopping list", "Butter, milk, eggs, cucumber" };
			dt.Rows.Add(firstRow.ToArray());

			List<string> secondRow = new List<string>() { "No", "TODO list", "Call elevator repair person" };
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
			return new List<string>() { "Secure", "Title", "Text" };
		}
	}
}