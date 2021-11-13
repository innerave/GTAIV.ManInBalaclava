using ManInBalaclava.Extensions;
using ManInBalaclava.Reactions;

namespace ManInBalaclava.States.Cops
{
    public class StartingChase : IState
    {
        public StartingChase(IReaction reaction)
        {
            _reaction = reaction;
        }

        private readonly IReaction _reaction;

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _reaction.Player.SetWantedLevelIfNotWanted();
        }

        public void OnExit()
        {
        }
    }
}