using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Native;

namespace ManInBalaclava.Extensions
{
    public static class PedExtensions
    {
        public static bool IsLookingAt(this Ped ped, Ped otherPed)
        {
            return Function.Call<bool>("IS_CHAR_FACING_CHAR", ped, otherPed, 120f);
        }

        public static bool IsFleeing(this Ped ped)
        {
            return Function.Call<bool>("IS_PED_FLEEING", ped);
        }

        public static IEnumerable<Ped> GetOthersAliveNearby(this Ped ped, float radius)
        {
            return World.GetPeds(ped.Position, radius).Where(p => p.Exists()).Where(p => p != ped)
                .Where(p => p.isAliveAndWell);
        }

        public static bool HasAnyFirearm(this Ped ped)
        {
            return ped.Weapons.AnyHandgun.isPresent
                   || ped.Weapons.AnyShotgun.isPresent
                   || ped.Weapons.AnySMG.isPresent
                   || ped.Weapons.AnyAssaultRifle.isPresent;
        }

        public static bool IsClimbing(this Ped ped)
        {
            return Function.Call<bool>("IS_PED_CLIMBING", ped);
        }

        public static bool IsFarFrom(this Ped ped, Ped otherPed)
        {
            return ped.Position.DistanceTo(otherPed.Position) >= 50;
        }

        public static bool IsTalking(this Ped ped)
        {
            return Function.Call<bool>("IS_AMBIENT_SPEECH_PLAYING", ped);
        }

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
        
        public static IEnumerable<Ped> WhereIsNotRequiredExceptCops(this IEnumerable<Ped> peds)
        {
            foreach (var ped in peds)
            {
                switch (ped.isRequiredForMission)
                {
                    case true when ped.PedType == PedType.Cop:
                        yield return ped;
                        break;
                    case false:
                        yield return ped;
                        break;
                }
            }
        }
    }
}