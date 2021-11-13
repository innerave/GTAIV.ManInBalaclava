using System.Collections.Generic;
using GTA;
using ManInBalaclava.Reactions;

namespace ManInBalaclava
{
    public class PedTracker
    {
        private readonly Dictionary<Ped, IReaction> _reactions = new();
        private readonly ReactionsFactory _factory;
        private readonly Queue<IReaction> _finished = new();

        public PedTracker(int percentageOfSnitches)
        {
            _factory = new ReactionsFactory(percentageOfSnitches);
        }

        public void Update()
        {
            foreach (var reaction in _reactions)
                if (reaction.Value.Finished)
                    _finished.Enqueue(reaction.Value);
                else
                    reaction.Value.Update();

            while (_finished.Count > 0)
            {
                var reaction = _finished.Dequeue();
                var reactingPed = reaction.ReactingPed;
                _reactions.Remove(reactingPed);
                if (reactingPed.PedType != PedType.Cop)
                {
                    reactingPed.isRequiredForMission = false;
                }
            }
        }

        public void AddIfNotTracked(Ped reactingPed, Player player)
        {
            if (_reactions.ContainsKey(reactingPed)) return;
            IReaction reaction = _factory.CreateReaction(reactingPed, player);
            if (reactingPed.PedType != PedType.Cop)
            {
                reactingPed.isRequiredForMission = true;
            }
            _reactions.Add(reactingPed, reaction);
        }
    }
}