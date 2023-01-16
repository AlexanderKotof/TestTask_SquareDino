using ScreenSystem.Screens;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class GameScreen : BaseScreen
    {
        public ShootingInputComponent shootingComponent;

        private ShootController _shootController;

        public void SetController(ShootController shootController)
        {
            _shootController = shootController;
            shootingComponent.Shoot += _shootController.Shoot;
        }

        protected override void OnHide()
        {
            base.OnHide();

            shootingComponent.Shoot -= _shootController.Shoot;
            _shootController = null;
        }
    }
}