using System;
using ManInBalaclava.Extensions;
using ManInBalaclava.Reactions;

namespace ManInBalaclava.States.Criminals
{
    public class Threatens : IState
    {
        private readonly Random _random;

        public Threatens(IReaction reaction, Random random)
        {
            _random = random;
            _reaction = reaction;
        }

        private readonly IReaction _reaction;

        public void Tick()
        {
            if (!_reaction.ReactingPed.IsTalking()) _reaction.ReactingPed.SayAmbientSpeech(GetRandomPhrase());
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }

        private string GetRandomPhrase()
        {
            return _random.Next(0, 9) switch
            {
                0 => "GANG_BUMP",
                1 => "FIGHT_RUN",
                2 => "SHIT",
                3 => "DODGE",
                4 => "GANG_BUMP",
                5 => "GENERIC_INSULT",
                6 => "GENERIC_FUCK_OFF",
                7 => "GET_OUT",
                8 => "INTIMIDATE",
                _ => ""
            };
        }
    }
}