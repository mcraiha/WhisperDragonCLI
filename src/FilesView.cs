using Terminal.Gui;
using System.Data;
using System.Collections.Generic;

namespace WhisperDragonCLI;

public static class FilesView
{
	public static View CreateView(List<FileSimplified> files)
	{
		var dt = new DataTable();

		foreach (string columnName in GetColumnNames())
		{
			dt.Columns.Add(columnName);
		}

		foreach (FileSimplified fs in files)
		{
			List<object> row = new List<object>() { fs.IsSecure, fs.Filename, fs.Filesize,fs.Filetype };
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
		return new List<string>() { "Secure", "Filename", "File size", "File type" };
	}
}
