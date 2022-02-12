using System;

public sealed class ContactSimplified
{
	// Non visible
	public int zeroBasedIndexNumber { get; set; }

	// Visible elements

	public bool IsSecure { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Emails { get; set; }

	public string PhoneNumbers { get; set; }

	public DateTimeOffset CreationTime { get; set; }

	public DateTimeOffset ModificationTime { get; set; }
}