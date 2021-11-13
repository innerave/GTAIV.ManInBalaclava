using GTA;
using ManInBalaclava.Extensions;
using ManInBalaclava.Reactions;

namespace ManInBalaclava.States.Snitch
{
    public class CallingCops : IState
    {
        private const int RequiredTime = 6000;

        private Timer _callTimer;

        public CallingCops(IReaction reaction)
        {
            _reaction = reaction;
        }

        private readonly IReaction _reaction;

        public void Tick()
        {
            if (_callTimer.ElapsedTime > RequiredTime) _reaction.Player.SetWantedLevelIfNotWanted();
        }

        public void OnEnter()
        {
            _callTimer = new Timer();
            _callTimer.Start();
            _reaction.ReactingPed.Task.UseMobilePhone();
        }

        public void OnExit()
        {
            _callTimer.Stop();
            _reaction.ReactingPed.Task.PutAwayMobilePhone();
        }
    }
}