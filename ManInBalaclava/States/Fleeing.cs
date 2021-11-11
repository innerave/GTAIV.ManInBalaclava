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
			// If you scare away some characters that were marked by the game as needed for the mission
			// (for example, hot dog sellers and waiters),
			// the game removes this flag after receiving a new task for the NPC
			// This hack is needed to prevent this, and to keep tracking the NPC.
			// Not a very good design, but hey, that's how the game works.
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