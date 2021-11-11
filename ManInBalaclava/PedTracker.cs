using System.Collections.Generic;
using GTA;
using ManInBalaclava.Extensions;
using ManInBalaclava.Reactions;

namespace ManInBalaclava
{
	public class PedTracker
	{
		private readonly Dictionary<Ped, NpcReaction> npcReactions = new();
		private readonly Queue<NpcReaction> finished = new();

		public int Count() => npcReactions.Count;

		public void Update()
		{
			while (finished.Count > 0)
			{
				var npcReaction = finished.Dequeue();
				var reactingPed = npcReaction.ReactingPed;
				npcReactions.Remove(reactingPed);
				if (!npcReaction.NpcWasOriginallyAMissionCharacter)
				{
					reactingPed.isRequiredForMission = false;
				}
			}

			foreach (var reaction in npcReactions)
			{
				if (reaction.Value.Finished)
				{
					finished.Enqueue(reaction.Value);
				}
				else
				{
					reaction.Value.Update();
				}
			}
		}

		public void AddIfNotTracked(Ped reactingPed, Player player)
		{
			npcReactions.TryAdd(reactingPed,
				NpcReactionsFactory.GetReactionForPed(reactingPed, player));
		}
	}
}