using System;
using Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    public class PuzzleUIManager : DestoryableSingleton<PuzzleUIManager>
    {
        public Toggle pauseToggle;
        
        public void ShowPauseMenu()
        {
            if (pauseToggle.isOn) PuzzleManager.Instance.PlayResume();
            else PuzzleManager.Instance.PlayPause();
        }
    }
}
