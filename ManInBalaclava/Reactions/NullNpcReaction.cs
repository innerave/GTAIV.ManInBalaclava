using GTA;
using ManInBalaclava.States.Common;

namespace ManInBalaclava.Reactions
{
	public class NullNpcReaction : NpcReaction
	{
		public NullNpcReaction(Ped reactingPed, Player player) : base(reactingPed, player)
		{
			var finished = new Finished(this);
			StateMachine.SetState(finished);
			Finished = true;
		}
	}
}