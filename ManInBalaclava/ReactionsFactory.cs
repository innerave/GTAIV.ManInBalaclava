using System;
using GTA;
using ManInBalaclava.Extensions;
using ManInBalaclava.Reactions;

namespace ManInBalaclava
{
    public class ReactionsFactory
    {
        private readonly Random _random = new();

        private readonly int _percentageOfSnitches;
        public ReactionsFactory(int percentageOfSnitches)
        {
            _percentageOfSnitches = percentageOfSnitches;
        }

        public IReaction CreateReaction(Ped reactingPed, Player player)
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
                    return new Criminal(reactingPed, player, _random);
                case PedType.Cop:
                    return new Cop(reactingPed, player);
                case PedType.Bum:
                case PedType.Prostitute:
                case PedType.CivFemale:
                case PedType.CivMale:
                    if (!player.IsWanted() && _random.Next(0, 101) <= _percentageOfSnitches)
                    {
                        return new Snitch(reactingPed, player);
                    }
                    else
                    {
                        return new Escapee(reactingPed, player);
                    }
                case PedType.Fireman:
                case PedType.Paramedic:
                    if (player.IsWanted())
                    {
                        return new Escapee(reactingPed, player);
                    }
                    else
                    {
                        return new Snitch(reactingPed, player);
                    }
                
                default:
                    return new NullReaction(reactingPed, player);
            }
        }
    }
}