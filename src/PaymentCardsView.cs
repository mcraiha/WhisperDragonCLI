using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI
{
	public static class PaymentCardsView
	{
		public static View CreateView()
		{
			var dt = new DataTable();

			foreach (string columnName in GetColumnNames())
			{
				dt.Columns.Add(columnName);
			}

			List<string> firstRow = new List<string>() { "Yes", "Dragon credit", "Dave Dragon", "Credit", "4024007105746837", "678" };
			dt.Rows.Add(firstRow.ToArray());

			List<string> secondRow = new List<string>() { "No", "Fire bank", "Dave teh Dragon", "Debit", "4024007182777473", "599" };
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
			return new List<string>() { "Secure", "Title", "Name on the card", "Card type", "Number", "Security code" };
		}
	}
}