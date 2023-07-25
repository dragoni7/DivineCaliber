using System;
using UnityEngine;
using Utils;

namespace dragoni7
{
    public class GameController : Singletone<GameController>
    {
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;
        public GameState CurrentState { get; private set; }
        private void Start()
        {
            ChangeState(GameState.Starting);
        }
        public void ChangeState(GameState newState)
        {
            OnBeforeStateChanged?.Invoke(newState);

            CurrentState = newState;

            switch (newState)
            {
                case GameState.Starting:
                    HandleStarting();
                    break;
                case GameState.GeneratingLevel:
                    HandleGeneratingLevel();
                    break;
                case GameState.SpawningPlayers:
                    HandleSpawningPlayers();
                    break;
                case GameState.SpawningEnemies:
                    HandleSpawningEnemies();
                    break;
                case GameState.PlayingLevel:
                    HandlePlayingLevel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnAfterStateChanged?.Invoke(newState);
        }

        private void HandleStarting()
        {
            ChangeState(GameState.GeneratingLevel);
        }

        private void HandleGeneratingLevel()
        {
            LevelController.Instance.CreateLevel("GenericGeneration", "GenericLevel");
        }

        private void HandleSpawningEnemies()
        {
            LevelController.Instance.FillRooms();
            ChangeState(GameState.SpawningPlayers);
        }

        private void HandleSpawningPlayers()
        {
            PlayerController.Instance.SpawnPlayer("Player1", LevelController.Instance.PlayerSpawnPoint());
            ChangeState(GameState.PlayingLevel);
        }

        private void HandlePlayingLevel()
        {
            
        }

        [Serializable]
        public enum GameState
        {
            Starting = 0,
            GeneratingLevel = 1,
            SpawningPlayers = 2,
            SpawningEnemies = 3,
            PlayingLevel = 4
            // TODO: add more as needed
        }
    }
}

