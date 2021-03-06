using ManInBalaclava.Reactions;

namespace ManInBalaclava.States.Common
{
    public class Finished : IState
    {
        public Finished(IReaction reaction)
        {
            _reaction = reaction;
        }

        private readonly IReaction _reaction;

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _reaction.Finished = true;
        }

        public void OnExit()
        {
        }
    }
}