using System;
using System.Linq;
using GTA;
using ManInBalaclava.Extensions;
using ManInBalaclava.States.Common;
using ManInBalaclava.States.Criminals;

namespace ManInBalaclava.Reactions
{
    public class Criminal : BaseReaction
    {
        public Criminal(Ped reactingPed, Player player, Random random) : base(reactingPed, player)
        {
            var initial = new Initial();
            var sawBalaclava = new SawBalaclava(this);
            var threatens = new Threatens(this, random);
            var startingFight = new StartingFight(this);
            var drivingAway = new DrivingAway(this);
            var fleeing = new Fleeing(this);
            var finished = new Finished(this);

            AddTransition(initial, sawBalaclava, () => !Player.Character.isInVehicle() && Player.IsUsingMask());

            AddTransition(sawBalaclava, fleeing, () => !ReactingPed.isInVehicle() && !ReactingPed.HasAnyFirearm());
            AddTransition(sawBalaclava, drivingAway, () => ReactingPed.isInVehicle() && !ReactingPed.HasAnyFirearm());
            AddTransition(sawBalaclava, threatens,
                () => ReactingPed.IsCloseFrom(player.Character) && player.Character.IsLookingAt(ReactingPed));
            AddTransition(sawBalaclava, startingFight, CanGetHelpFromOtherGangMembers);

            AddTransition(threatens, sawBalaclava,
                () => !ReactingPed.IsCloseFrom(player.Character) || !player.Character.IsLookingAt(ReactingPed));
            AddTransition(threatens, startingFight, CanGetHelpFromOtherGangMembers);

            StateMachine.AddAnyTransition(finished,
                () => ReactingPed.isDead || ReactingPed.isInjured || ReactingPed.IsFarFrom(Player.Character));
            StateMachine.SetState(initial);
        }

        private bool CanGetHelpFromOtherGangMembers()
        {
            var nearby = ReactingPed.GetOthersAliveNearby(20f).ToList();
            return nearby.Any(p => p.PedType == ReactingPed.PedType)
                   && nearby.All(p => p.PedType != PedType.Cop);
        }
    }
}