using System;

public sealed class LoginSimplified
{
	// Non visible
	public int zeroBasedIndexNumber { get; set; }

	// Visible elements

	public bool IsSecure { get; set; }
	public string Title { get; set; }
	public string URL { get; set; }
	public string Email { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public string Notes { get; set; }
	public byte[] Icon { get; set; }
	public string Category { get; set; }
	public string Tags { get; set; }
	public DateTimeOffset CreationTime { get; set; }
	public DateTimeOffset ModificationTime { get; set; }
}