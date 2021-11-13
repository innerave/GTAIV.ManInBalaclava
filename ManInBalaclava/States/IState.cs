namespace ManInBalaclava.States
{
    public interface IState
    {
        void Tick();
        void OnEnter();
        void OnExit();
    }
}