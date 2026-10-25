SetupUnits = function()
	Trigger.AfterDelay(1, function()
		Utils.Do(Map.NamedActors, function(a)
			if a.HasProperty("TransportCrates") then
				a.TransportCrates()
			end
		end)
	end)
end

WorldLoaded = function()
	ED_1 = Player.GetPlayer("ED_1")
	ED_2 = Player.GetPlayer("ED_2")
	UCS_1 = Player.GetPlayer("UCS_1")
	UCS_2 = Player.GetPlayer("UCS_2")
	UCS_3 = Player.GetPlayer("UCS_3")
	Trigger.AfterDelay(DateTime.Seconds(1), function()
		ED_1.GrantCondition("enable-ed_1-ai")
		ED_2.GrantCondition("enable-ed_2-ai")
		UCS_1.GrantCondition("enable-ucs_1-ai")
		UCS_2.GrantCondition("enable-ucs_2-ai")
		UCS_3.GrantCondition("enable-ucs_3-ai")
	end)
end
