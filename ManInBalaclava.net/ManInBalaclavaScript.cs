using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using KeyEventArgs = GTA.KeyEventArgs;

namespace ManInBalaclava.net
{
	public class ManInBalaclavaScript : Script
	{
		private readonly Keys maskKey;
		private readonly Keys glovesKey;
		private readonly AnimationSet maskAnimationSet;
		private readonly AnimationSet glovesAnimationSet;
		private readonly Random random = new Random();

		public ManInBalaclavaScript()
		{
			KeyUp += OnKeyUp;
			Tick += OnTick;
			maskAnimationSet = new AnimationSet("playidles_cold");
			glovesAnimationSet = new AnimationSet("amb@atm");
			Interval = 1500;
			if (!File.Exists(Settings.Filename))
			{
				Settings.SetValue("MaskKey", "Keys", Keys.M);
				Settings.SetValue("GlovesKey", "Keys", Keys.G);
				Settings.Save();
			}

			maskKey = Settings.GetValueKey("MaskKey", "Keys", Keys.M);
			glovesKey = Settings.GetValueKey("GlovesKey", "Keys", Keys.G);
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == maskKey)
			{
				ToggleMask();
			}

			if (e.Key == glovesKey)
			{
				ToggleGloves();
			}
		}

		private void ToggleMask()
		{
			if (NotAllowedNow()) return;
			Player.Character.Animation.Play(maskAnimationSet, "play_collar", 1.5f);
			Game.WaitInCurrentScript(1500);
			SetMaskModel();
			Player.Character.Task.ClearAll();
		}

		private void ToggleGloves()
		{
			if (NotAllowedNow()) return;
			Player.Character.Animation.Play(glovesAnimationSet, "m_getoutwallet_chest", 2f);
			Game.WaitInCurrentScript(600);
			SetGlovesModel();
			Player.Character.Task.ClearAll();
			Player.Character.FleeByVehicle(Player.Character.CurrentVehicle);
		}

		private bool NotAllowedNow() =>
			IsPlayerBeingArrested()
			|| Player.Character.isInVehicle()
			|| Player.Character.isDead
			|| !Player.CanControlCharacter
			|| Player.Character.isRagdoll
			|| Player.Character.isSwimming
			|| Player.Character.isSwimming;

		private static bool IsPlayerBeingArrested() => Function.Call<bool>("IS_PLAYER_BEING_ARRESTED");

		private void SetMaskModel()
		{
			Function.Call("SET_CHAR_COMPONENT_VARIATION", Player.Character, 8,
				UsingMask() ? 0 : 1, 0);
		}

		private void SetGlovesModel()
		{
			Function.Call("SET_CHAR_COMPONENT_VARIATION", Player.Character, 4,
				UsingGloves() ? 0 : 1, 0);
		}

		private bool UsingMask() => Function.Call<int>("GET_CHAR_DRAWABLE_VARIATION", Player.Character, 8) == 1;

		private bool UsingGloves() => Function.Call<int>("GET_CHAR_DRAWABLE_VARIATION", Player.Character, 4) == 1;

		private void OnTick(object sender, EventArgs e)
		{
			if (!UsingMask()) return;
			foreach (var ped in GetNearbyPeds(15f)
				.Where(p => !p.isInGroup)
				.Where(p => !p.isRequiredForMission))
			{
				if (IsFacingPlayer(ped))
				{
					SetPedReactionToMask(ped);
				}
			}
		}

		private IEnumerable<Ped> GetNearbyPeds(float radius) =>
			World.GetPeds(Player.Character.Position, radius).Where(Exists).Where(p => p != Player.Character);

		private bool IsFacingPlayer(Ped ped) => Function.Call<bool>("IS_CHAR_FACING_CHAR", ped, Player.Character, 90f);

		private void SetPedReactionToMask(Ped ped)
		{
			switch (ped.PedType)
			{
				case PedType.Cop:
					SetWantedLevelIfNotWanted();
					break;
				case PedType.Gang_Korean:
				case PedType.Gang_AfricanAmerican:
				case PedType.Gang_Jamaican:
				case PedType.Gang_IrishGang:
				case PedType.Gang_RussianGang:
				case PedType.RussianMob:
				case PedType.ItalianMafia:
				case PedType.TheLost:
				case PedType.AngelsOfDeath:
				case PedType.AlbanianGang:
				case PedType.Gang_PuertoRican:
				case PedType.Gang_ChineseJapanese:
				case PedType.Criminal:
					SetCriminalReactionToMask(ped);
					break;
				default:
					SetCivilianReactionToMask(ped);
					break;
			}
		}

		private void SetWantedLevelIfNotWanted()
		{
			if (Player.WantedLevel != 0) return;
			Player.WantedLevel = 1;
		}

		private void SetCriminalReactionToMask(Ped ped)
		{
			if (
				ped.isInCombat
				|| ped.isInMeleeCombat
				|| Player.Character.isInVehicle()
				|| ped.isInVehicle()
				|| ped.isInjured
				|| !ped.isAliveAndWell) return;
			if (AnyFirearmIsPresent(ped))
			{
				var otherNearbyPeds = GetNearbyPeds(30f).Where(p => p != ped).ToList();

				if (
					otherNearbyPeds.Any(p => p.PedType == ped.PedType)
					&& otherNearbyPeds.All(p => p.PedType != PedType.Cop))
				{
					ped.Task.FightAgainst(Player.Character);
				}
				else
				{
					PlaySpeechByCriminal(ped);
				}
			}
			else
			{
				ped.Task.FleeFromChar(Player.Character);
			}
		}

		private static bool AnyFirearmIsPresent(Ped ped)
		{
			return ped.Weapons.AnyHandgun.isPresent
			|| ped.Weapons.AnyShotgun.isPresent
			|| ped.Weapons.AnySMG.isPresent
			|| ped.Weapons.AnyAssaultRifle.isPresent;
		}

		private void PlaySpeechByCriminal(Ped ped)
		{
			switch (random.Next(0, 11))
			{
				case 1:
					ped.SayAmbientSpeech("GENERIC_CURSE");
					break;
				case 5:
					ped.SayAmbientSpeech("GENERIC_INSULT");
					break;
				case 10:
					ped.SayAmbientSpeech("GENERIC_FUCK_OFF");
					break;
			}
		}

		private void SetCivilianReactionToMask(Ped ped)
		{
			if (ped.isInCombat || ped.isInMeleeCombat || Player.Character.isInVehicle()) return;
			if (ped.isInVehicle())
			{
				ped.Task.CruiseWithVehicle(ped.CurrentVehicle, 40f, false);
			}
			else
			{
				ped.Task.FleeFromChar(Player.Character, true);
			}
		}
	}
}