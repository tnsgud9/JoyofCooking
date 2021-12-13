using System;
using Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    public class PuzzleUIManager : DestoryableSingleton<PuzzleUIManager>
    {
        public Toggle pauseToggle;

        public void Awake()
        {
        }

        public void ShowPauseMenu()
        {
            if (pauseToggle.isOn) PuzzleManager.Instance.PlayResume();
            else PuzzleManager.Instance.PlayPause();
        }

        public void ResetButton()
        {
            Debug.Log("Restart");
            PuzzleManager.Instance.PlayRestart();
        }

        public void BackButton()
        {
            
            Debug.Log("Back");
            //backButton Scene Events;
        }
    }
}
