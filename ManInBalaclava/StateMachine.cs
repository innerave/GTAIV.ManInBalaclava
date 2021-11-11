using System;
using System.Collections.Generic;
using System.Linq;
using ManInBalaclava.States;

namespace ManInBalaclava
{
	public class StateMachine
	{
		private IState currentState;

		private readonly Dictionary<Type, List<Transition>> transitions = new();
		private List<Transition> currentTransitions = new();
		private readonly List<Transition> anyTransitions = new();

		private static readonly List<Transition> EmptyTransitions = new(0);

		public void Tick()
		{
			var transition = GetTransition();
			if (transition != null)
				SetState(transition.To);

			currentState?.Tick();
		}

		public void SetState(IState state)
		{
			if (state == currentState)
				return;

			currentState?.OnExit();
			currentState = state;

			transitions.TryGetValue(currentState.GetType(), out currentTransitions);
			currentTransitions ??= EmptyTransitions;

			currentState.OnEnter();
		}

		public void AddTransition(IState from, IState to, Func<bool> predicate)
		{
			if (transitions.TryGetValue(from.GetType(), out var list) == false)
			{
				list = new List<Transition>();
				transitions[from.GetType()] = list;
			}

			list.Add(new Transition(to, predicate));
		}

		public void AddAnyTransition(IState state, Func<bool> predicate)
		{
			anyTransitions.Add(new Transition(state, predicate));
		}

		private class Transition
		{
			public Func<bool> Condition { get; }
			public IState To { get; }

			public Transition(IState to, Func<bool> condition)
			{
				To = to;
				Condition = condition;
			}
		}

		private Transition GetTransition()
		{
			foreach (var transition in anyTransitions.Where(transition => transition.Condition()))
				return transition;

			return currentTransitions.FirstOrDefault(transition => transition.Condition());
		}
	}
}