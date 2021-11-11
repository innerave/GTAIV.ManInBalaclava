using GTA;
using GTA.Native;

namespace ManInBalaclava.Extensions
{
	public static class PedExtensions
	{
		public static bool IsLookingAt(this Ped ped, Ped otherPed) =>
			Function.Call<bool>("IS_CHAR_FACING_CHAR", ped, otherPed, 120f);

		public static bool IsFleeing(this Ped ped) => Function.Call<bool>("IS_PED_FLEEING", ped);

		private static bool HasAnyFirearm(this Ped ped) =>
			ped.Weapons.AnyHandgun.isPresent
			|| ped.Weapons.AnyShotgun.isPresent
			|| ped.Weapons.AnySMG.isPresent
			|| ped.Weapons.AnyAssaultRifle.isPresent;

		public static bool IsClimbing(this Ped ped) => Function.Call<bool>("IS_PED_CLIMBING", ped);

		public static bool IsFarFrom(this Ped ped, Ped otherPed) => ped.Position.DistanceTo(otherPed.Position) >= 50;

		public static bool IsCloseFrom(this Ped ped, Ped otherPed)
		{
			var distance = ped.Position.DistanceTo(otherPed.Position);
			return distance <= 15;
		}

		public static bool IsMidwayFrom(this Ped ped, Ped otherPed)
		{
			var distance = ped.Position.DistanceTo(otherPed.Position);
			return distance is > 15 and < 25;
		}
	}
}