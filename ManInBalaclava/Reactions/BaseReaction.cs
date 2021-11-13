using System;
using GTA;
using ManInBalaclava.States;

namespace ManInBalaclava.Reactions
{
    public abstract class BaseReaction : IReaction
    {
        protected BaseReaction(Ped reactingPed, Player player)
        {
            ReactingPed = reactingPed;
            Player = player;
            StateMachine = new StateMachine();
        }

        public Ped ReactingPed { get; }

        public Player Player { get; }

        public bool Finished { get; set; }

        protected StateMachine StateMachine { get; }

        public void Update()
        {
            StateMachine.Tick();
        }

        protected void AddTransition(IState from, IState to, Func<bool> condition)
        {
            StateMachine.AddTransition(from, to, condition);
        }
    }
}