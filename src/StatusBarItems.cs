using Terminal.Gui;
using System.Collections.Generic;

namespace WhisperDragonCLI;

public static class StatusBarItems
{
	private static readonly Dictionary<VisibleElement, StatusItem[]> statusItems = new()
	{
		{ VisibleElement.ShowLoginInformations,  new StatusItem[] {
			new StatusItem(Key.F5 | Key.R, $"~F5~ {LocMan.Get("Open URL")}", () => {}),
			new StatusItem(Key.F6 | Key.R, $"~F6~ {LocMan.Get("Copy URL")}", () => {}),
			new StatusItem(Key.F7 | Key.R, $"~F7~ {LocMan.Get("Copy Username")}", () => {}),
			new StatusItem(Key.F8 | Key.R, $"~F8~ {LocMan.Get("Copy Password")}", () => {}),
		}},

		{ VisibleElement.ShowNotes, new StatusItem[] {
			new StatusItem(Key.F7 | Key.R, $"~F7~ {LocMan.Get("Copy Title")}", () => {}),
			new StatusItem(Key.F8 | Key.R, $"~F8~ {LocMan.Get("Copy Text")}", () => {}),
		}},
	};

	private static readonly StatusItem[] emptyItems = new StatusItem[0];

	public static StatusItem[] Get(VisibleElement visibleElement)
	{
		if (statusItems.ContainsKey(visibleElement))
		{
			return statusItems[visibleElement];
		}

		return emptyItems;
	}
}
