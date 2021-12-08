using UnityEngine;

    public abstract class Minigame : MonoBehaviour
    {
        public static Minigame Current;

        public enum GameState
        {
            NotLaunched = 0, // not inited, exited
            Ready, // Init() end
            Runned, // Playing game
            Paused, // ui showing, etc
            Win,
            Lose
        }
        public GameState gameState;

        public virtual void Launch() // Level.Instantiated Enable ui, prepare init state
        {
            if (Current != null)
                Debug.LogError($"You are launching minigame while another game is launched");
            Current = this;
            gameState = GameState.Ready; 
        }
        public virtual void Run() { gameState = GameState.Runned; } // TapToStart.OnClick Start game event
        public virtual void Restart() { gameState = GameState.Ready; } // Restart minigame call / Set into launched but in some state
        public virtual void OnEvent_Win() { gameState = GameState.Win; }
        public virtual void OnEvent_Lose() { gameState = GameState.Lose; }
        public abstract void OnEvent_RewardTake(bool isX2); // Called from this minigame to add score
        public virtual void Exit() // Disable ui, exit
        {
            if (Current == null)
                Debug.LogError($"You are exiting minigame while no game is launched");
            Current = null;
            gameState = GameState.NotLaunched; 
        }
    }