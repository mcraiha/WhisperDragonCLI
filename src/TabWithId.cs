namespace WhisperDragonCLI;

public sealed class TabWithId : Terminal.Gui.TabView.Tab
{
	private readonly int id = 0;

	public TabWithId(int customId) : base()
	{
		this.id = customId;
	}

	public TabWithId(int customId, string text, Terminal.Gui.View view) : base(text, view)
	{
		this.id = customId;
	}

	public int GetId()
	{
		return this.id;
	}
}
