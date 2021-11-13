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
        private readonly PedTracker _pedTracker;

        public Main()
        {
            if (!File.Exists(Settings.Filename))
            {
                Settings.SetValue("MaskKey", "Keys", Keys.M);
                Settings.SetValue("GlovesKey", "Keys", Keys.G);
                Settings.SetValue("PercentageOfFleeingCiviliansWhoCallThePolice", "Gameplay", 20);
                Settings.Save();
            }

            _maskKey = Settings.GetValueKey("MaskKey", "Keys", Keys.M);
            _glovesKey = Settings.GetValueKey("GlovesKey", "Keys", Keys.G);
            var percentageOfSnitches = Settings.GetValueInteger("PercentageOfFleeingCiviliansWhoCallThePolice", "Gameplay", 20);
            if (percentageOfSnitches is < 0 or > 100)
            {
                percentageOfSnitches = 20;
            }
            _pedTracker = new PedTracker(percentageOfSnitches);
            KeyUp += OnKeyUp;
            Tick += OnTick;
            Interval = 500;
        }
        
        private readonly Keys _maskKey;
        private readonly Keys _glovesKey;

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == _maskKey) Player.ToggleMask();

            if (e.Key == _glovesKey) Player.ToggleGloves();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (Player.IsUsingMask())
                foreach (var ped in Player.Character.GetOthersAliveNearby(20f)
                    .Where(p => !p.isInGroup)
                    .Where(p => p.isAliveAndWell)
                    .WhereIsNotRequiredExceptCops())
                    if (ped.IsLookingAt(Player.Character))
                        _pedTracker.AddIfNotTracked(ped, Player);

            _pedTracker.Update();
        }
    }
}