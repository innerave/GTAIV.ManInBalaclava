using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GTA;
using ManInBalaclava.Extensions;
using KeyEventArgs = GTA.KeyEventArgs;

namespace ManInBalaclava
{
	public class Main : Script
	{
		private Keys MaskKey { get; }
		private Keys GlovesKey { get; }

		private readonly PedTracker pedTracker;

		public Main()
		{
			if (!File.Exists(Settings.Filename))
			{
				Settings.SetValue("MaskKey", "Keys", Keys.M);
				Settings.SetValue("GlovesKey", "Keys", Keys.G);
				Settings.Save();
			}

			MaskKey = Settings.GetValueKey("MaskKey", "Keys", Keys.M);
			GlovesKey = Settings.GetValueKey("GlovesKey", "Keys", Keys.G);
			pedTracker = new PedTracker();
			KeyUp += OnKeyUp;
			Tick += OnTick;
			Interval = 500;
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == MaskKey)
			{
				Player.ToggleMask();
			}

			if (e.Key == GlovesKey)
			{
				Player.ToggleGloves();
			}
		}

		private void OnTick(object sender, EventArgs e)
		{
			Game.DisplayText($"Tracked: {pedTracker.Count()}");
			if (Player.IsUsingMask())
			{
				foreach (var ped in GetNearbyPeds(20f)
					.Where(p => !p.isInGroup)
					.Where(p => p.isAliveAndWell)
					.Where(p => !p.isRequiredForMission))
				{
					if (ped.IsLookingAt(Player.Character))
					{
						pedTracker.AddIfNotTracked(ped, Player);
					}
				}
			}

			pedTracker.Update();
		}

		private IEnumerable<Ped> GetNearbyPeds(float radius) =>
			World.GetPeds(Player.Character.Position, radius).Where(Exists).Where(p => p != Player.Character);
	}
}