using System;
using System.Collections.Generic;
using System.Linq;

namespace ManInBalaclava.States
{
    public class StateMachine
    {
        private static readonly List<Transition> EmptyTransitions = new(0);
        private readonly List<Transition> _anyTransitions = new();

        private readonly Dictionary<Type, List<Transition>> _transitions = new();
        private IState _currentState;
        private List<Transition> _currentTransitions = new();

        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null)
                SetState(transition.To);

            _currentState?.Tick();
        }

        public void SetState(IState state)
        {
            if (state == _currentState)
                return;

            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
            _currentTransitions ??= EmptyTransitions;

            _currentState.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (_transitions.TryGetValue(from.GetType(), out var list) == false)
            {
                list = new List<Transition>();
                _transitions[from.GetType()] = list;
            }

            list.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions.Where(transition => transition.Condition()))
                return transition;

            return _currentTransitions.FirstOrDefault(transition => transition.Condition());
        }

        private class Transition
        {
            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }

            public Func<bool> Condition { get; }
            public IState To { get; }
        }
    }
}