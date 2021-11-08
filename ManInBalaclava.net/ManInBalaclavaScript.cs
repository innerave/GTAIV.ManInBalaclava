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
		private bool usingMask;
		private bool usingGloves;
		private readonly AnimationSet maskAnimationSet;
		private readonly AnimationSet glovesAnimationSet;
		private readonly Random random = new Random();
		private int diedCount;
		private int arrestedCount;

		public ManInBalaclavaScript()
		{
			KeyUp += OnKeyUp;
			Tick += OnTick;
			maskAnimationSet = new AnimationSet("playidles_cold");
			glovesAnimationSet = new AnimationSet("amb@atm");
			Interval = 1500;
			diedCount = Game.GetIntegerStatistic(IntegerStatistic.TIMES_DIED);
			arrestedCount = Game.GetIntegerStatistic(IntegerStatistic.TIMES_BUSTED);
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

		private bool NotAllowedNow() =>
			IsPlayerBeingArrested()
			|| Player.Character.isInVehicle()
			|| Player.Character.isDead
			|| !Player.CanControlCharacter
			|| Player.Character.isRagdoll
			|| Player.Character.isSwimming
			|| Player.Character.isSwimming;

		private void ToggleGloves()
		{
			if (NotAllowedNow()) return;
			Player.Character.Animation.Play(glovesAnimationSet, "m_getoutwallet_chest", 2f);
			Game.WaitInCurrentScript(600);
			SetGlovesModel();
			Player.Character.Task.ClearAll();
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!usingMask) return;
			CheckIfDied();
			CheckIfArrested();

			foreach (var ped in GetNearbyPeds(15f)
				.Where(p => !p.isInGroup)
				.Where(p => !p.isRequiredForMission)
				.Where(p => p.RelationshipGroup != RelationshipGroup.Player))
			{
				if (IsFacingPlayer(ped))
				{
					SetPedReactionToMask(ped);
				}
			}
		}

		private void CheckIfDied()
		{
			if (diedCount >= Game.GetIntegerStatistic(IntegerStatistic.TIMES_DIED)) return;
			while (Player.Character.isDead)
			{
				Game.WaitInCurrentScript(100);
			}

			diedCount = Game.GetIntegerStatistic(IntegerStatistic.TIMES_DIED);
			Reset();
		}

		private void CheckIfArrested()
		{
			if (arrestedCount >= Game.GetIntegerStatistic(IntegerStatistic.TIMES_BUSTED)) return;
			//After the player has been completely arrested
			//(i.e., the arrest is counted in the statistics and the player cannot escape),
			//the game thinks that we can still control the character
			//(despite the fact that we cannot move).
			//Thus, we wait until the screen goes off
			//and the player "loses control" according to the logic of the game.
			while (Player.CanControlCharacter)
			{
				Game.WaitInCurrentScript(100);
			}

			arrestedCount = Game.GetIntegerStatistic(IntegerStatistic.TIMES_BUSTED);
			Reset();
		}

		private void Reset()
		{
			// Mask
			Function.Call("SET_CHAR_COMPONENT_VARIATION", Player.Character, 8, 0, 0);
			usingMask = false;
			// Gloves
			Function.Call("SET_CHAR_COMPONENT_VARIATION", Player.Character, 4, 0, 0);
			usingGloves = false;
		}

		private IEnumerable<Ped> GetNearbyPeds(float radius) =>
			World.GetPeds(Player.Character.Position, radius).Where(Exists).Where(p => p != Player.Character);

		private bool IsFacingPlayer(Ped ped) => Function.Call<bool>("IS_CHAR_FACING_CHAR", ped, Player.Character, 90f);

		private static bool IsPlayerBeingArrested() => Function.Call<bool>("IS_PLAYER_BEING_ARRESTED");

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
			if (
				ped.Weapons.AnyHandgun.isPresent
				|| ped.Weapons.AnyShotgun.isPresent
				|| ped.Weapons.AnySMG.isPresent
				|| ped.Weapons.AnyAssaultRifle.isPresent)
			{
				PlaySpeechByCriminal(ped);
			}
			else
			{
				ped.Task.FleeFromChar(Player.Character);
			}
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

		private void SetMaskModel()
		{
			if (usingMask)
			{
				Function.Call("SET_CHAR_COMPONENT_VARIATION", Player.Character, 8, 0, 0);
				usingMask = false;
			}
			else
			{
				Function.Call("SET_CHAR_COMPONENT_VARIATION", Player.Character, 8, 1, 0);
				usingMask = true;
			}
		}

		private void SetGlovesModel()
		{
			if (usingGloves)
			{
				Function.Call("SET_CHAR_COMPONENT_VARIATION", Player.Character, 4, 0, 0);
				usingGloves = false;
			}
			else
			{
				Function.Call("SET_CHAR_COMPONENT_VARIATION", Player.Character, 4, 1, 0);
				usingGloves = true;
			}
		}
	}
}