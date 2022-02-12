using System;

public sealed class NoteSimplified
{
	// Non visible
	public int zeroBasedIndexNumber { get; set; }

	// Visible elements

	public bool IsSecure { get; set; }

	public string Title { get; set; }

	public string Text { get; set; }

	public DateTimeOffset CreationTime { get; set; }

	public DateTimeOffset ModificationTime { get; set; }
}