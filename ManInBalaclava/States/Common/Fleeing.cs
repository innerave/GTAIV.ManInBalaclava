using ManInBalaclava.Extensions;
using ManInBalaclava.Reactions;

namespace ManInBalaclava.States.Common
{
    public class Fleeing : IState
    {
        public Fleeing(IReaction reaction)
        {
            _reaction = reaction;
        }

        private readonly IReaction _reaction;

        public void Tick()
        {
            if (_reaction.ReactingPed.IsFleeing()) return;
            _reaction.ReactingPed.Task.FleeFromChar(_reaction.Player, false);
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}