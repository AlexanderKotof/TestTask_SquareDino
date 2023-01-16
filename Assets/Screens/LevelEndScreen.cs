using ScreenSystem.Screens;
using System;
using UnityEngine.UI;

namespace UI.Screens
{
    public class LevelEndScreen : BaseScreen
    {
        public Button continueButton;

        public void SetCallback(Action callback)
        {
            continueButton.onClick.AddListener(callback.Invoke);
        }

        protected override void OnHide()
        {
            continueButton.onClick.RemoveAllListeners();
        }
    }
}