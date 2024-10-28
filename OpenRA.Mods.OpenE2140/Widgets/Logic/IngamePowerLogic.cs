﻿#region Copyright & License Information

/*
 * Copyright (c) The OpenE2140 Developers and Contributors
 * This file is part of OpenE2140, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */

#endregion

using JetBrains.Annotations;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Mods.OpenE2140.Traits.Power;
using OpenRA.Primitives;
using OpenRA.Widgets;

namespace OpenRA.Mods.OpenE2140.Widgets.Logic;

[UsedImplicitly]
public class IngamePowerLogic : ChromeLogic
{
	[FluentReference("usage", "capacity")]
	private const string PowerUsage = "label-power-usage";

	[FluentReference]
	private const string Infinite = "label-infinite-power";

	[ObjectCreator.UseCtor]
	public IngamePowerLogic(Widget widget, World world)
	{
		var developerMode = world.LocalPlayer.PlayerActor.Trait<DeveloperMode>();
		var powerManager = world.LocalPlayer.PlayerActor.Trait<PowerManagerBase>();

		var power = widget.Get<IngamePowerWidget>("POWER");
		var powerIcon = widget.Get<ImageWidget>("POWER_ICON");
		var unlimitedCapacity = FluentProvider.GetString(IngamePowerLogic.Infinite);

		powerIcon.GetImageName = () => powerManager.Power < 0 ? "power-critical" : "power-normal";
		power.GetColor = () => powerManager.Power < 0 ? power.CriticalPowerColor : power.NormalPowerColor;
		power.GetText = () => developerMode.UnlimitedPower ? unlimitedCapacity : powerManager.Power.ToString();

		var tooltipTextCached = new CachedTransform<(string, string), string>(
			((string usage, string capacity) args) => FluentProvider.GetString(
				IngamePowerLogic.PowerUsage,
				FluentProvider.GetString("usage", args.usage, "capacity", args.capacity)
			)
		);

		power.GetTooltipText = () => tooltipTextCached.Update(
			(powerManager.PowerConsumed.ToString(), developerMode.UnlimitedPower ? unlimitedCapacity : powerManager.PowerGenerated.ToString())
		);
	}
}
