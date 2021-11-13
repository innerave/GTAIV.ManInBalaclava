using ManInBalaclava.Reactions;

namespace ManInBalaclava.States.Common
{
    public class DrivingAway : IState
    {
        public DrivingAway(IReaction reaction)
        {
            _reaction = reaction;
        }

        private readonly IReaction _reaction;

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _reaction.ReactingPed.Task.CruiseWithVehicle(_reaction.ReactingPed.CurrentVehicle, 40f, false);
        }

        public void OnExit()
        {
        }
    }
}