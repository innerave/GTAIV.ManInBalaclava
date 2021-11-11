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