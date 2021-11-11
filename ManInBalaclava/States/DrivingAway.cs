using ManInBalaclava.Reactions;

namespace ManInBalaclava.States
{
	public class DrivingAway : IState
	{
		private NpcReaction NpcReaction { get; }

		public DrivingAway(NpcReaction npcReaction)
		{
			NpcReaction = npcReaction;
		}

		public void Tick()
		{
		}

		public void OnEnter()
		{
			NpcReaction.ReactingPed.Task.CruiseWithVehicle(NpcReaction.ReactingPed.CurrentVehicle, 40f, false);
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

		public void OnExit()
		{
		}
	}
}