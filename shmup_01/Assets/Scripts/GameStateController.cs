using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.GameStates;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

// TODO Use statecontroller
public class GameStateController : IInitializable, ITickable
{
    private GameState _currentState;
    private BaseGameStateController _currentStateController;

    private Dictionary<GameState, BaseGameStateController> gameStates =
        new Dictionary<GameState, BaseGameStateController>();

    public void Initialize()
    {
        gameStates.Add(GameState.InGame, inGameStateController);
        gameStates.Add(GameState.GameOver, gameOverStateController);
        gameStates.Add(GameState.Success, successStateController);

        SetState(GameState.InGame);

        _currentState = GameState.InGame;
        _currentStateController = inGameStateController;
    }

    public void Tick()
    {
        // if (Gamepad.current.aButton.wasPressedThisFrame)
        // {
        // }
    }

    public void SetState(GameState gameState)
    {
        if (_currentStateController != null)
        {
            _currentStateController.IsActive = false;
            _currentStateController.OnExit();
        }

        _currentState = gameState;
        BaseGameStateController newState;
        gameStates.TryGetValue(gameState, out newState);

        if (newState != null)
        {
            _currentStateController = newState;
        }

        _currentStateController.IsActive = true;
        _currentStateController.OnEnter();
    }

    [Inject] private GameOverStateController gameOverStateController;
    [Inject] private SuccessStateController successStateController;
    [Inject] private InGameStateController inGameStateController;
}