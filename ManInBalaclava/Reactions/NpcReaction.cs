using System;
using GTA;
using ManInBalaclava.States;

namespace ManInBalaclava.Reactions
{
	public abstract class NpcReaction : IDisposable
	{
		public Ped ReactingPed { get; private set; }

		public Player Player { get; }

		public bool Finished { get; set; }

		protected StateMachine StateMachine { get; }

		protected NpcReaction(Ped reactingPed, Player player)
		{
			ReactingPed = reactingPed;
			Player = player;
			StateMachine = new StateMachine();
		}

		public void Update() => StateMachine.Tick();

		protected void AddTransition(IState from, IState to, Func<bool> condition) =>
			StateMachine.AddTransition(from, to, condition);

		public void Dispose()
		{
			ReactingPed = null;
		}
	}
}