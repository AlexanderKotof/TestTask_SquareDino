using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Screens
{
    public class ShootingInputComponent : MonoBehaviour, IPointerDownHandler
    {
        public event Action<Vector3> Shoot;
        public void OnPointerDown(PointerEventData eventData)
        {
            Shoot?.Invoke(eventData.position);
        }
    }
}