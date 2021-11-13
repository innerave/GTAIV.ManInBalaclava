using ManInBalaclava.Reactions;

namespace ManInBalaclava.States.Common
{
    public class SawBalaclava : IState
    {
        public SawBalaclava(IReaction reaction)
        {
            _reaction = reaction;
        }

        private IReaction _reaction;

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