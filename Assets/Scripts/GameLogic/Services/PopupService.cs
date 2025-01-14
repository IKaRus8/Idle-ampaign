using JetBrains.Annotations;
using UI.Interfaces;
using UnityEngine;
using Zenject;

namespace GameLogic.Services
{
    [UsedImplicitly]
    public class PopupService
    {
        private readonly DiContainer _container;
        private readonly IUIContainer _uiContainer;

        public PopupService(DiContainer container,
            IUIContainer uiContainer)
        {
            _container = container;
            _uiContainer = uiContainer;
        }

        public void CreatePopup(string popupPath, bool useBackground)
        {
            if (useBackground)
            {
                _uiContainer.ScreenBack.SetActive(true);
            }
            
            var popupAsset = Resources.Load(popupPath);

            _container.InstantiatePrefab(popupAsset, _uiContainer.PopupContainer);
        }

        public void ClosePopup(IPopup popup)
        {
            Object.Destroy(popup.GO);
            
            _uiContainer.ScreenBack.SetActive(false);
        }
    }
}