using System;

public sealed class PaymentCardSimplified
{
	// Non visible
	public int zeroBasedIndexNumber { get; set; }

	// Visible elements

	public bool IsSecure { get; set; }

	public string Title { get; set; }

	public string NameOnTheCard { get; set; }

	public string CardType { get; set; }

	public string Number { get; set; }

	public string SecurityCode { get; set; }

	public DateTimeOffset CreationTime { get; set; }

	public DateTimeOffset ModificationTime { get; set; }
}