using GTA;
using ManInBalaclava.Reactions;

namespace ManInBalaclava
{
	public static class NpcReactionsFactory
	{
		public static NpcReaction GetReactionForPed(Ped reactingPed, Player player)
		{
			switch (reactingPed.PedType)
			{
				case PedType.Dealer:
				case PedType.Criminal:
				case PedType.Gang_PuertoRican:
				case PedType.Gang_ChineseJapanese:
				case PedType.Gang_Korean:
				case PedType.Gang_AfricanAmerican:
				case PedType.Gang_Jamaican:
				case PedType.Gang_IrishGang:
				case PedType.Gang_RussianGang:
				case PedType.RussianMob:
				case PedType.ItalianMafia:
				case PedType.TheLost:
				case PedType.AngelsOfDeath:
				case PedType.AlbanianGang:
					return new NullNpcReaction(reactingPed, player);
				case PedType.Cop:
					return new Cop(reactingPed, player);
				case PedType.Bum:
				case PedType.Prostitute:
				case PedType.CivFemale:
				case PedType.CivMale:
					return new Escapee(reactingPed, player);
				case PedType.Fireman:
				case PedType.Paramedic:
					return new NullNpcReaction(reactingPed, player);
				default:
					return new NullNpcReaction(reactingPed, player);
			}
		}
	}
}