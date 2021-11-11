using ManInBalaclava.Extensions;
using ManInBalaclava.Reactions;

namespace ManInBalaclava.States
{
	public class Fleeing : IState
	{
		private NpcReaction NpcReaction { get; }

		public Fleeing(NpcReaction npcReaction)
		{
			NpcReaction = npcReaction;
		}

		public void Tick()
		{
			if (NpcReaction.ReactingPed.IsFleeing()) return;
			NpcReaction.ReactingPed.Task.FleeFromChar(NpcReaction.Player, false);
			if (NpcReaction.NpcWasOriginallyAMissionCharacter)
			{
				NpcReaction.ReactingPed.isRequiredForMission = true;
			}
		}

		public void OnEnter()
		{
		}

		public void OnExit()
		{
		}
	}
}