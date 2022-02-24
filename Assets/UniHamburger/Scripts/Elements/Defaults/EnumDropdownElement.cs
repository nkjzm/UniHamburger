using System;
using System.Linq;
using nkjzm.UniHamburger.Elements.Base;
using nkjzm.UniHamburger.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace nkjzm.UniHamburger.Elements.Defaults
{
    /// <summary>
    /// 列挙型ドロップダウン要素クラス
    /// </summary>
    public sealed class EnumDropdownElement : BaseEnumDropdownElement
    {
        [SerializeField] private Text label;
        [SerializeField] private Text description;
        [SerializeField] private Dropdown dropdown;
        private IntReactiveProperty currentIndex;
        private int defaultIndex;
        private string key;
        private int[] indexes;
        private string[] selections;

        public override IObservable<T> Setup<T>(string label, T defaultValue, string description = "",
            string salt = "", IObservable<bool> activeUpdated = null)
        {
            this.label.text = label;
            this.description.text = description;
            this.description.enabled = !string.IsNullOrEmpty(description);
            defaultIndex = Convert.ToInt32(defaultValue);
            key = KeyGenerator.CreateKey(label, salt);

            var initialValue = PlayerPrefs.GetInt(key, defaultIndex);
            currentIndex = new IntReactiveProperty(initialValue);

            activeUpdated?.Subscribe(isEnabled => dropdown.interactable = isEnabled).AddTo(this);

            // 選択肢の初期化
            var enums = (Enum.GetValues(typeof(T)) as T[])!.ToArray();
            indexes = enums.Select(e => Convert.ToInt32(e)).ToArray();
            selections = enums.Select(enumValue => enumValue.ToString()).ToArray();

            // dropdownの初期化
            dropdown.options = selections.Select(value => new Dropdown.OptionData(value)).ToList();

            // dropdownの変更時に値を保持
            // 元々設定されている初期選択位置分はスキップ
            dropdown.OnValueChangedAsObservable().Skip(1)
                .Subscribe(dropdownIndex =>
                {
                    var index = Convert.ToInt32(enums[dropdownIndex]);
                    currentIndex.Value = index;
                    PlayerPrefs.SetInt(key, index);
                })
                .AddTo(this);

            // 選択位置の設定
            ResetDropdown();

            return currentIndex.Select(index => (T)Enum.ToObject(typeof(T), index)).AsObservable();
        }

        public override void ResetParam()
        {
            currentIndex.Value = defaultIndex;
            ResetDropdown();
        }

        private void ResetDropdown()
        {
            var selectIndex = Array.IndexOf(indexes, currentIndex.Value);
            dropdown.value = selectIndex >= 0 ? selectIndex : 0;
        }
    }
}