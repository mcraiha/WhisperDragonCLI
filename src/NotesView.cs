using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI
{
	public static class NotesView
	{
		public static View CreateView(List<NoteSimplified> notes)
		{
			var dt = new DataTable();

			foreach (string columnName in GetColumnNames())
			{
				dt.Columns.Add(columnName);
			}

			foreach (NoteSimplified ns in notes)
			{
				List<object> row = new List<object>() { ns.IsSecure, ns.Title, ns.Text };
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

			return tableView;
		}

		public static List<string> GetColumnNames()
		{
			return new List<string>() { "Secure", "Title", "Text" };
		}
	}
}