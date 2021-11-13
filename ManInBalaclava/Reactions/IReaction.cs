using GTA;

namespace ManInBalaclava.Reactions
{
    public interface IReaction
    {
        public Ped ReactingPed { get; }

        public Player Player { get; }

        public bool Finished { get; set; }

        public void Update();
    }
}