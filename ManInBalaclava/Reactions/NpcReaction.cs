using System;
using GTA;
using ManInBalaclava.States;

namespace ManInBalaclava.Reactions
{
	public abstract class NpcReaction
	{
		public Ped ReactingPed { get; }
		public Player Player { get; }

		public bool NpcWasOriginallyAMissionCharacter { get; }

		public bool Finished { get; set; }

		protected StateMachine StateMachine { get; }

		protected NpcReaction(Ped reactingPed, Player player)
		{
			ReactingPed = reactingPed;
			NpcWasOriginallyAMissionCharacter = ReactingPed.isRequiredForMission;
			if (!NpcWasOriginallyAMissionCharacter)
			{
				ReactingPed.isRequiredForMission = true;
			}

			Player = player;
			StateMachine = new StateMachine();
		}

		public void Update() => StateMachine.Tick();

		protected void AddTransition(IState from, IState to, Func<bool> condition) =>
			StateMachine.AddTransition(from, to, condition);
	}
}