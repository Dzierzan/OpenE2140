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

using System.Runtime.CompilerServices;
using OpenRA.Support;

namespace OpenRA.Mods.OpenE2140.Extensions;

public static class VariableExpressionExtensions
{
	public static IntegerExpression Concat(this IntegerExpression expression, FormattableString format)
	{
		var args = format.GetArguments()
			.Select(
				arg => arg switch
				{
					VariableExpression variable => variable.Expression,
					_ => arg
				}
			)
			.ToArray();

		return new IntegerExpression(expression.Expression + FormattableStringFactory.Create(format.Format, args));
	}
}
