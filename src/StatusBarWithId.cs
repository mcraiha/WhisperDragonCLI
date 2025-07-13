
namespace WhisperDragonCLI;

public sealed class StatusBarWithId : Terminal.Gui.StatusBar
{
	private readonly VisibleElement tabType;

	public StatusBarWithId(VisibleElement visibleElement) : base()
	{
		this.tabType = visibleElement;
	}

	public StatusBarWithId(VisibleElement visibleElement, Terminal.Gui.StatusItem[] items) : base(items)
	{
		this.tabType = visibleElement;
	}

	public VisibleElement GetTabType()
	{
		return this.tabType;
	}
}
