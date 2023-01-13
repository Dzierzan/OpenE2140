﻿#region Copyright & License Information

/*
 * Copyright 2007-2023 The OpenE2140 Developers (see AUTHORS)
 * This file is part of OpenE2140, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */

#endregion

using JetBrains.Annotations;
using OpenRA.Graphics;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.E2140.Traits.Rendering;

[TraitLocation(SystemActors.World | SystemActors.EditorWorld)]
[Desc("Add color shifts to player palettes. Use to add RGBA compatibility to PlayerColorPalette.")]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class PlayerColorShiftInfo : TraitInfo
{
	[PaletteReference(true)]
	[FieldLoader.RequireAttribute]
	[Desc("The name of the palette to base off.")]
	public readonly string BasePalette = "";

	[Desc("Hues between this and MaxHue will be shifted.")]
	public readonly float MinHue = 0.83f;

	[Desc("Hues between MinHue and this will be shifted.")]
	public readonly float MaxHue = 0.84f;

	[Desc("Hue reference for the color shift.")]
	public readonly float ReferenceHue = 0.835f;

	[Desc("Saturation reference for the color shift.")]
	public readonly float ReferenceSaturation = 1;

	public override object Create(ActorInitializer init) { return new PlayerColorShift(this); }
}

public class PlayerColorShift : ILoadsPlayerPalettes
{
	private readonly PlayerColorShiftInfo info;

	public PlayerColorShift(PlayerColorShiftInfo info)
	{
		this.info = info;
	}

	public void LoadPlayerPalettes(WorldRenderer worldRenderer, string playerName, Color color, bool replaceExisting)
	{
		var (_, hue, saturation, _) = color.ToAhsv();

		worldRenderer.SetPaletteColorShift(
			this.info.BasePalette + playerName,
			hue - this.info.ReferenceHue,
			saturation - this.info.ReferenceSaturation,
			this.info.MinHue,
			this.info.MaxHue
		);
	}
}