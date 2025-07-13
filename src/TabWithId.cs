namespace WhisperDragonCLI;

public sealed class TabWithId : Terminal.Gui.TabView.Tab
{
	private readonly VisibleElement tabType;

	public TabWithId(VisibleElement visibleElement) : base()
	{
		this.tabType = visibleElement;
	}

	public TabWithId(VisibleElement visibleElement, string text, Terminal.Gui.View view) : base(text, view)
	{
		this.tabType = visibleElement;
	}

	public VisibleElement GetTabType()
	{
		return this.tabType;
	}
}
