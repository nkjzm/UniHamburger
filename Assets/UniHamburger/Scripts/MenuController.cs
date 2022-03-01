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

        /// <summary>
        /// 初期状態で開く
        /// </summary>
        public bool OpenOnInit
        {
            set => openOnInit = value;
        }

        /// <summary>
        /// 開いている
        /// </summary>
        public bool IsExpand => panelCanvasGroup.interactable;

        public void Start()
        {
            if (openOnInit)
                Expand();
            else
                Minimize();

            button.onClick.AddListener(Switch);
        }

        /// <summary>
        /// 開閉を切り替える
        /// </summary>
        public void Switch()
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