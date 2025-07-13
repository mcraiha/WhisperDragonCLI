
namespace WhisperDragonCLI;

public sealed class StatusBarWithId : Terminal.Gui.StatusBar
{
	private readonly int id = 0;

	public StatusBarWithId(int customId) : base()
	{
		this.id = customId;
	}

	public StatusBarWithId(int customId, Terminal.Gui.StatusItem[] items) : base(items)
	{
		this.id = customId;
	}

	public int GetId()
	{
		return this.id;
	}
}
