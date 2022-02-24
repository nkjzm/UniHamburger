using UnityEngine;
using UnityEngine.UI;

namespace nkjzm.UniHamburger
{
    /// <summary>
    /// メニューコントローラー
    /// </summary>
    public sealed class MenuController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private CanvasGroup panelCanvasGroup;
        [SerializeField] private bool openOnInit;
        private float initAlpha;
        private bool isOpen;

        private void Start()
        {
            if (openOnInit)
                Expand();
            else
                Minimize();

            button.onClick.AddListener(Switch);
        }

        private void Switch()
        {
            if (panelCanvasGroup.interactable)
                Minimize();
            else
                Expand();
        }

        private void Minimize()
        {
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.interactable = panelCanvasGroup.blocksRaycasts = false;
        }

        private void Expand()
        {
            panelCanvasGroup.alpha = 1f;
            panelCanvasGroup.interactable = panelCanvasGroup.blocksRaycasts = true;
        }
    }
}