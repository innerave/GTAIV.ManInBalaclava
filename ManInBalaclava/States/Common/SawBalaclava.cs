using ManInBalaclava.Reactions;

namespace ManInBalaclava.States.Common
{
	public class SawBalaclava : IState
	{
		private NpcReaction NpcReaction { get; }

		public SawBalaclava(NpcReaction npcReaction)
		{
			NpcReaction = npcReaction;
		}

		public void Tick()
		{
		}

		public void OnEnter()
		{
		}

		public void OnExit()
		{
		}
	}
}