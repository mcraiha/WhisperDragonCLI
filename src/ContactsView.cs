using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI
{
	public static class ContactsView
	{
		public static View CreateView(List<ContactSimplified> contacts)
		{
			var dt = new DataTable();

			foreach (string columnName in GetColumnNames())
			{
				dt.Columns.Add(columnName);
			}

			foreach (ContactSimplified cs in contacts)
			{
				List<object> row = new List<object>() { cs.IsSecure, cs.FirstName, cs.LastName, cs.Emails, cs.PhoneNumbers };
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
			return new List<string>() { "Secure", "First name", "Last name", "Email(s)", "Phone number(s)" };
		}
	}
}