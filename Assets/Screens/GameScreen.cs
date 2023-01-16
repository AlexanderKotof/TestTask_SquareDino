using ScreenSystem.Screens;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class GameScreen : BaseScreen
    {
        public ShootingInputComponent shootingComponent;

        private Action<Vector3> _shootCallback;

        public void SetShootCallback(Action<Vector3> callback)
        {
            _shootCallback = callback;
        }

        protected override void OnShow()
        {
            base.OnShow();

            shootingComponent.Shoot += _shootCallback;
        }

        protected override void OnHide()
        {
            base.OnHide();

            shootingComponent.Shoot -= _shootCallback;
        }

        protected override void OnDestroy()
        {
            _shootCallback = null;
        }
    }
}