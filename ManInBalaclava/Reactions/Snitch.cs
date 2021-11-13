using GTA;
using ManInBalaclava.Extensions;
using ManInBalaclava.States.Common;
using ManInBalaclava.States.Snitch;

namespace ManInBalaclava.Reactions
{
    public class Snitch : BaseReaction
    {
        public Snitch(Ped reactingPed, Player player) : base(reactingPed, player)
        {
            var initial = new Initial();
            var sawBalaclava = new SawBalaclava(this);
            var callingCops = new CallingCops(this);
            var fleeing = new Fleeing(this);
            var finished = new Finished(this);

            AddTransition(initial, sawBalaclava, () => !Player.Character.isInVehicle() && Player.IsUsingMask());

            AddTransition(sawBalaclava, fleeing, () => !ReactingPed.isInVehicle());

            AddTransition(fleeing, callingCops, () => reactingPed.Position.DistanceTo(Player.Character.Position) > 40);

            AddTransition(callingCops, fleeing, () => reactingPed.Position.DistanceTo(Player.Character.Position) < 40);

            AddTransition(callingCops, finished, () => Player.IsWanted());

            StateMachine.AddAnyTransition(finished,
                () => ReactingPed.isDead || ReactingPed.isInjured || ReactingPed.isInCombat ||
                      ReactingPed.isInVehicle());
            StateMachine.SetState(initial);
        }
    }
}