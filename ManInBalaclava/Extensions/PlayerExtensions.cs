using GTA;
using GTA.Native;

namespace ManInBalaclava.Extensions
{
    public static class PlayerExtensions
    {
        public static bool IsBeingArrested(this Player player)
        {
            return Function.Call<bool>("IS_PLAYER_BEING_ARRESTED");
        }

        public static void SetWantedLevelIfNotWanted(this Player player)
        {
            if (player.WantedLevel != 0) return;
            player.WantedLevel = 1;
        }

        public static bool IsWanted(this Player player)
        {
            return player.WantedLevel > 0;
        }

        public static bool IsUsingMask(this Player player)
        {
            return Function.Call<int>("GET_CHAR_DRAWABLE_VARIATION", player.Character, 8) == 1;
        }

        private static bool IsUsingGloves(this Player player)
        {
            return Function.Call<int>("GET_CHAR_DRAWABLE_VARIATION", player.Character, 4) == 1;
        }

        private static void SetMaskModel(this Player player)
        {
            Function.Call("SET_CHAR_COMPONENT_VARIATION", player.Character, 8,
                IsUsingMask(player) ? 0 : 1, 0);
        }

        private static void SetGlovesModel(this Player player)
        {
            Function.Call("SET_CHAR_COMPONENT_VARIATION", player.Character, 4,
                IsUsingGloves(player) ? 0 : 1, 0);
        }

        private static bool CanToggleNow(this Player player)
        {
            return !player.IsBeingArrested()
                   && !player.Character.isInVehicle()
                   && !player.Character.isDead
                   && player.CanControlCharacter
                   && !player.Character.isRagdoll
                   && !player.Character.isSwimming
                   && !player.Character.IsClimbing();
        }

        public static void ToggleMask(this Player player)
        {
            if (!CanToggleNow(player)) return;
            player.Character.Animation.Play(new AnimationSet("playidles_cold"), "play_collar", 1.5f);
            Game.WaitInCurrentScript(1500);
            SetMaskModel(player);
            player.Character.Task.ClearAll();
        }

        public static void ToggleGloves(this Player player)
        {
            if (!CanToggleNow(player)) return;
            player.Character.Animation.Play(new AnimationSet("amb@atm"), "m_getoutwallet_chest", 2f);
            Game.WaitInCurrentScript(600);
            SetGlovesModel(player);
            player.Character.Task.ClearAll();
        }
    }
}