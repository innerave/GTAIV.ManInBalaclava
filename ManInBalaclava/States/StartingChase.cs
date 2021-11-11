using ManInBalaclava.Extensions;
using ManInBalaclava.Reactions;

namespace ManInBalaclava.States
{
	public class StartingChase : IState
	{
		private NpcReaction NpcReaction { get; }

		public StartingChase(NpcReaction npcReaction)
		{
			NpcReaction = npcReaction;
		}

		public void Tick()
		{
		}

		public void OnEnter()
		{
			NpcReaction.Player.SetWantedLevelIfNotWanted();
		}

		public void OnExit()
		{
		}
	}
}