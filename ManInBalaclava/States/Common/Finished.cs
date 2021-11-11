using ManInBalaclava.Reactions;

namespace ManInBalaclava.States.Common
{
	public class Finished : IState
	{
		private NpcReaction NpcReaction { get; }

		public Finished(NpcReaction npcReaction)
		{
			NpcReaction = npcReaction;
		}

		public void Tick()
		{
		}

		public void OnEnter()
		{
			NpcReaction.Finished = true;
		}

		public void OnExit()
		{
		}
	}
}