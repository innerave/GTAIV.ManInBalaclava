using GTA;
using ManInBalaclava.Extensions;
using ManInBalaclava.States.Common;

namespace ManInBalaclava.Reactions
{
	/// <summary>
	///     The reaction of this NPC is to escape from the masked player
	/// </summary>
	public class Escapee : BaseReaction
    {
        public Escapee(Ped reactingPed, Player player) : base(reactingPed, player)
        {
            var initial = new Initial();
            var sawBalaclava = new SawBalaclava(this);
            var drivingAway = new DrivingAway(this);
            var fleeing = new Fleeing(this);
            var finished = new Finished(this);
            
            AddTransition(initial, sawBalaclava, () => Player.IsUsingMask());

            AddTransition(sawBalaclava, fleeing, () => !ReactingPed.isInVehicle());
            AddTransition(sawBalaclava, drivingAway, ReactingPed.isInVehicle);

            StateMachine.AddAnyTransition(finished,
                () => ReactingPed.isDead || ReactingPed.isInjured || ReactingPed.isInCombat ||
                      ReactingPed.IsFarFrom(Player.Character));
            StateMachine.SetState(initial);
        }
    }
}