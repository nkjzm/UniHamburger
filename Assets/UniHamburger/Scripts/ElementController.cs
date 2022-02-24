using System;
using System.Collections.Generic;
using nkjzm.UniHamburger.Elements.Base;
using nkjzm.UniHamburger.Elements.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace nkjzm.UniHamburger
{
    /// <summary>
    /// 生成する要素のコントローラー
    /// </summary>
    public sealed class ElementController : MonoBehaviour
    {
        [SerializeField] private BaseIntSliderElement intIntSliderElementPrefab;
        [SerializeField] private BaseFloatSliderElement floatIntSliderElementPrefab;
        [SerializeField] private BaseToggleElement toggleElementPrefab;
        [SerializeField] private BaseArrayDropdownElement arrayDropdownItemPrefab;
        [SerializeField] private BaseEnumDropdownElement enumDropdownElementPrefab;
        [SerializeField] private Transform contents;
        [SerializeField] private Button resetButton;
        private readonly List<IResettable> resettableList = new List<IResettable>();

        private void Start()
        {
            resetButton.onClick.AddListener(ResetAllElements);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="label"> 表示するラベル </param>
        /// <param name="defaultValue"> 初期値 </param>
        /// <param name="unit"> 変更単位 </param>
        /// <param name="min"> 最小値 </param>
        /// <param name="max"> 最大値 </param>
        /// <param name="description"> 詳細 </param>
        /// <param name="salt"> 保存用のソルト </param>
        /// <param name="activeUpdated"> 有効化切り替え </param>
        /// <returns> 値の変更通知 </returns>
        public IObservable<int> CreateIntItem(string label, int defaultValue, int unit = 1, int min = int.MinValue,
            int max = int.MaxValue, string description = "", string salt = "", IObservable<bool> activeUpdated = null)
        {
            var item = Instantiate(intIntSliderElementPrefab, contents);
            resettableList.Add(item);
            return item.Setup(label, defaultValue, unit, min, max, description, salt, activeUpdated);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="label"> 表示するラベル </param>
        /// <param name="defaultValue"> 初期値 </param>
        /// <param name="unit"> 変更単位 </param>
        /// <param name="min"> 最小値 </param>
        /// <param name="max"> 最大値 </param>
        /// <param name="format"> 浮動小数の表示フォーマット </param>
        /// <param name="description"> 詳細 </param>
        /// <param name="salt"> 保存用のソルト </param>
        /// <param name="activeUpdated"> 有効化切り替え </param>
        /// <returns> 値の変更通知 </returns>
        public IObservable<float> CreateFloatItem(string label, float defaultValue, float unit = 0.1f,
            float min = float.MinValue, float max = float.MaxValue, string format = "F2", string description = "",
            string salt = "", IObservable<bool> activeUpdated = null)
        {
            var item = Instantiate(floatIntSliderElementPrefab, contents);
            resettableList.Add(item);
            return item.Setup(label, defaultValue, unit, min, max, format, description, salt, activeUpdated);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="label"> 表示するラベル </param>
        /// <param name="defaultValue"> 初期値 </param>
        /// <param name="description"> 詳細 </param>
        /// <param name="salt"> 保存用のソルト </param>
        /// <param name="activeUpdated"> 有効化切り替え </param>
        /// <returns> 値の変更通知 </returns>
        public IObservable<bool> CreateBoolItem(string label, bool defaultValue, string description = "",
            string salt = "", IObservable<bool> activeUpdated = null)
        {
            var item = Instantiate(toggleElementPrefab, contents);
            resettableList.Add(item);
            return item.Setup(label, defaultValue, description, salt, activeUpdated);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="label"> 表示するラベル </param>
        /// <param name="defaultValue"> 初期値 </param>
        /// <param name="getSelection"> ラベル配列 </param>
        /// <param name="description"> 詳細 </param>
        /// <param name="salt"> 保存用のソルト </param>
        /// <param name="activeUpdated"> 有効化切り替え </param>
        /// <returns> 値の変更通知 </returns>
        public IObservable<string> CreateSelectionItem(string label, string defaultValue, Func<string[]> getSelection,
            string description = "", string salt = "", IObservable<bool> activeUpdated = null)
        {
            var item = Instantiate(arrayDropdownItemPrefab, contents);
            resettableList.Add(item);
            return item.Setup(label, defaultValue, getSelection, description, salt, activeUpdated);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="label"> 表示するラベル </param>
        /// <param name="defaultValue"> 初期値 </param>
        /// <param name="description"> 詳細 </param>
        /// <param name="salt"> 保存用のソルト </param>
        /// <param name="activeUpdated"> 有効化切り替え </param>
        /// <returns> 値の変更通知 </returns>
        public IObservable<T> CreateEnumItem<T>(string label, T defaultValue, string description = "",
            string salt = "", IObservable<bool> activeUpdated = null) where T : Enum
        {
            var item = Instantiate(enumDropdownElementPrefab, contents);
            resettableList.Add(item);
            return item.Setup(label, defaultValue, description, salt, activeUpdated);
        }

        /// <summary>
        /// 全ての要素をリセットする
        /// </summary>
        public void ResetAllElements()
        {
            foreach (var resettable in resettableList) resettable.ResetParam();
        }
    }
}