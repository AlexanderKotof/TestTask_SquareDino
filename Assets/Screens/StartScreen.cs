using ScreenSystem.Screens;
using System;
using UnityEngine.UI;

namespace UI.Screens
{
    public class StartScreen : BaseScreen
    {
        public Button startButton;

        public void SetCallback(Action callback)
        {
            startButton.onClick.AddListener(callback.Invoke);
        }

        protected override void OnHide()
        {
            startButton.onClick.RemoveAllListeners();
        }
    }
}