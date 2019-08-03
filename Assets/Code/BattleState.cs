using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateManager
{
	private Dictionary<BattleStates, IBattleState> _states;
	private BattleStates _currentState;
	private IBattleState _currentStateHandler;

	public GameObject UnitPrefab;
	public UIFacade UIFacade;
	public Unit[] Units = new Unit[]
	{
		new Unit("Ally1", new string[] { "Light Punch", "Strong Punch" }, Alliances.Ally),
			new Unit("Ally2", new string[] { "Light Heal", "Strong Heal" }, Alliances.Ally),
			new Unit("Foe1", new string[] { "Light Punch", "Strong Punch" }, Alliances.Foe),
			new Unit("Foe2", new string[] { "Light Heal", "Strong Heal" }, Alliances.Foe)
	};

	public BattleStateManager(GameObject unitPrefab, UIFacade uiFacade)
	{
		UnitPrefab = unitPrefab;
		UIFacade = uiFacade;

		_states = new Dictionary<BattleStates, IBattleState>
		{ //
			{ BattleStates.Init, new InitBattleState(this) },
			{ BattleStates.PlayerTurn, new PlayerTurnState(this) },
			{ BattleStates.CPUTurn, new CPUTurnState(this) }
		};
	}

	public void Init()
	{
		ChangeState(BattleStates.Init);
	}

	public void Tick() => _currentStateHandler.Update();

	public void ChangeState(BattleStates newState)
	{
		// Already in state.
		if (_currentState == newState) { return; }

		// Debug.Log($"View Changing state from {_currentState} to {newState}");

		_currentState = newState;

		if (_currentStateHandler != null)
		{
			_currentStateHandler.ExitState();
			_currentStateHandler = null;
		}

		_currentStateHandler = _states[newState];
		_currentStateHandler.EnterState();
	}
}

public interface IBattleState
{
	void EnterState();
	void Update();
	void ExitState();
}

public enum BattleStates
{
	None,
	Init,
	PlayerTurn,
	CPUTurn
}