using System;

public sealed class FileSimplified
{
	// Non visible
	public int zeroBasedIndexNumber { get; set; }

	// Visible elements

	public bool IsSecure { get; set; }

	public string Filename { get; set; }

	public string Filesize { get; set; }

	public string Filetype { get; set; }

	public DateTimeOffset CreationTime { get; set; }

	public DateTimeOffset ModificationTime { get; set; }
}