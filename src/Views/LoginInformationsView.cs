using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI;

public class LoginInformationsView : View
{
	private readonly DataTable dataTable;

	private readonly TableView tableView;

	private LoginInformationsView(List<LoginSimplified> logins)
	{
		dataTable = new DataTable();

		foreach (string columnName in GetColumnNames())
		{
			dataTable.Columns.Add(columnName);
		}

		foreach (LoginSimplified ls in logins)
		{
			List<object> row = new List<object>() { ls.IsSecure, ls.Title, ls.URL, ls.Email, ls.Username, ls.Password };
			dataTable.Rows.Add(row.ToArray());
		}

		TableView tableView = new TableView()
		{
			X = 0,
			Y = 0,
			Width = Dim.Fill(),
			Height = Dim.Fill(),
		};

		tableView.Table = dataTable;

		Add(tableView);
	}

	public void CopyURLToClipboard()
	{
		Clipboard.TrySetClipboardData((string)dataTable.Rows[tableView.SelectedRow]["URL"]);
	}

	public static LoginInformationsView Create(List<LoginSimplified> logins)
	{
		LoginInformationsView returnValue = new LoginInformationsView(logins)
		{
			X = 0,
			Y = 0,
			Width = Dim.Fill(),
			Height = Dim.Fill(1),
		};

		/*var addLoginButton = new Button(6, 19, LocMan.Get("+ Add login information"));
		addLoginButton.Clicked += () =>
		{
			var addLoginInformation = AddOrEditLoginInformationDialog.CreateDialog(editMode: false, () => { }, () => Application.RequestStop());
			Application.Run(addLoginInformation);
		};

		returnValue.Add(addLoginButton);
		*/

		return returnValue;
	}

	private static readonly List<string> columnNames = new List<string>() { "Secure", "Title", "URL", "Email", "Username", "Password" };

	public static List<string> GetColumnNames()
	{
		return columnNames;
	}

}
