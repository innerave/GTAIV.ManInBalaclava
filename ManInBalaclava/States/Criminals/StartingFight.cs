using ManInBalaclava.Reactions;

namespace ManInBalaclava.States.Criminals
{
    public class StartingFight : IState
    {
        public StartingFight(IReaction reaction)
        {
            _reaction = reaction;
        }

        private readonly IReaction _reaction;

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _reaction.ReactingPed.Task.FightAgainst(_reaction.Player.Character);
        }

        public void OnExit()
        {
        }
    }
}