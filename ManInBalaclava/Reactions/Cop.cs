using GTA;
using ManInBalaclava.Extensions;
using ManInBalaclava.States.Common;
using ManInBalaclava.States.Cops;

namespace ManInBalaclava.Reactions
{
    public class Cop : BaseReaction
    {
        public Cop(Ped reactingPed, Player player) : base(reactingPed, player)
        {
            var initial = new Initial();
            var sawBalaclava = new SawBalaclava(this);
            var finished = new Finished(this);
            var startingChase = new StartingChase(this);

            AddTransition(initial, startingChase, () => !Player.IsWanted());

            StateMachine.AddAnyTransition(finished,
                () => ReactingPed.isDead || ReactingPed.isInjured || Player.IsWanted());
            StateMachine.SetState(initial);
        }
    }
}