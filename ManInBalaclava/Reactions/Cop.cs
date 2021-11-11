using GTA;
using ManInBalaclava.Extensions;
using ManInBalaclava.States;
using ManInBalaclava.States.Common;

namespace ManInBalaclava.Reactions
{
	public class Cop : NpcReaction
	{
		public Cop(Ped reactingPed, Player player) : base(reactingPed, player)
		{
			var initial = new Initial();
			var sawBalaclava = new SawBalaclava(this);
			var finished = new Finished(this);
			var startingChase = new StartingChase(this);

			AddTransition(initial, sawBalaclava, () => Player.IsUsingMask() && !Player.IsWanted());
			AddTransition(sawBalaclava, startingChase, () => true);

			StateMachine.AddAnyTransition(finished, () => !ReactingPed.isAliveAndWell || Player.IsWanted());
			StateMachine.SetState(initial);
		}
	}
}