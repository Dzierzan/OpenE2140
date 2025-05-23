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

using OpenRA.Activities;

namespace OpenRA.Mods.OpenE2140.Traits.Resources.Activities;

public class ConveyorBeltLoadUnloadCrate : Activity
{
	private enum DockState
	{
		None,
		Docking,
		Docked,
		Undocking,
		Complete
	}

	private readonly CrateTransporter crateTransporter;
	private readonly ConveyorBelt conveyorBelt;
	private readonly Actor conveyorBeltActor;
	private readonly IConveyorBeltDockHost conveyorBeltDockHost;

	private DockState state = DockState.None;

	// Perhaps use this.crateTransporter.Crate != null to distinguish between loading and unloading?
	private bool IsLoading => this.conveyorBelt is ResourceMine;

	public ConveyorBeltLoadUnloadCrate(Actor self, IConveyorBeltDockHost dockHost, Actor conveyorBeltActor)
	{
		this.crateTransporter = self.Trait<CrateTransporter>();
		this.conveyorBelt = conveyorBeltActor.Trait<ConveyorBelt>();
		this.conveyorBeltActor = conveyorBeltActor;
		this.conveyorBeltDockHost = dockHost;

		this.state = DockState.Docking;

		this.IsInterruptible = false;
	}

	protected override void OnFirstRun(Actor self)
	{
		switch (this.state)
		{
			case DockState.None:
				break;
			case DockState.Docking:
			{
				this.QueueChild(this.conveyorBeltDockHost.GetInnerDockActivity(
					this.conveyorBeltActor, self, () => this.state = DockState.Docked, new(this.IsLoading, DockAnimation.Docking)));

				break;
			}
			case DockState.Undocking:
				break;
			default:
				break;
		}
	}

	public override bool Tick(Actor self)
	{
		switch (this.state)
		{
			case DockState.Docking:
			{
				break;
			}
			case DockState.Docked:
			{
				if (this.crateTransporter.OnConveyorBeltDockTick(self, this.conveyorBelt, this.conveyorBeltActor))
					this.state = DockState.Undocking;
				break;
			}
			case DockState.Undocking:
			{
				this.QueueChild(this.conveyorBeltDockHost.GetInnerDockActivity(
					this.conveyorBeltActor, self, () => this.state = DockState.Complete, new(this.IsLoading, DockAnimation.Undocking)));

				break;
			}
			case DockState.Complete:
			{
				this.crateTransporter.OnConveyorBeltUndock();
				return true;
			}
			default:
				break;
		}

		return false;
	}
}

// TODO: maybe create two activities: one for docking, one for undocking and remove this enum?
public enum DockAnimation
{
	Docking,
	Undocking
}
