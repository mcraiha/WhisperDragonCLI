using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI;

public static class PaymentCardsView
{
	public static View CreateView(List<PaymentCardSimplified> paymentCards)
	{
		var dt = new DataTable();

		foreach (string columnName in GetColumnNames())
		{
			dt.Columns.Add(columnName);
		}

		foreach (PaymentCardSimplified pcs in paymentCards)
		{
			List<object> row = new List<object>() { pcs.IsSecure, pcs.Title, pcs.NameOnTheCard, pcs.CardType, pcs.Number, pcs.SecurityCode };
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

	private static readonly List<string> columnNames = new List<string>() { "Secure", "Title", "Name on the card", "Card type", "Number", "Security code" };

	public static List<string> GetColumnNames()
	{
		return columnNames;
	}
}