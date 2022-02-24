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
    /// 配列ドロップダウン要素クラス
    /// </summary>
    public sealed class ArrayDropdownElement : BaseArrayDropdownElement
    {
        [SerializeField] private Text label;
        [SerializeField] private Text description;
        [SerializeField] private Dropdown dropdown;
        [SerializeField] private Button refresh;
        private StringReactiveProperty currentValue;
        private string defaultValue;
        private Func<string[]> getSelection;
        private string[] selections;
        private string key;

        public override IObservable<string> Setup(string label, string defaultValue, Func<string[]> getSelection,
            string description = "", string salt = "", IObservable<bool> activeUpdated = null)
        {
            this.label.text = label;
            this.description.text = description;
            this.description.enabled = !string.IsNullOrEmpty(description);
            this.getSelection = getSelection;
            this.defaultValue = defaultValue;
            key = KeyGenerator.CreateKey(label, salt);

            var initialValue = PlayerPrefs.GetString(key, defaultValue);
            currentValue = new StringReactiveProperty(initialValue);

            activeUpdated?.Subscribe(isEnabled => dropdown.GetComponent<Button>().interactable = isEnabled).AddTo(this);

            // dropdown初期化
            InitDropdown();

            // 選択肢の更新
            refresh.OnClickAsObservable().Subscribe(_ => InitDropdown()).AddTo(this);

            return currentValue.AsObservable();
        }

        public override void InitDropdown()
        {
            // 選択肢の初期化
            selections = getSelection();

            // 重複チェック
            if (selections.Distinct().Count() != selections.Length) throw new InvalidOperationException();

            // dropdownの初期化
            dropdown.options = selections.Select(value => new Dropdown.OptionData(value)).ToList();

            // dropdownの変更時に値を保持
            // 元々設定されている初期選択位置分はスキップ
            dropdown.OnValueChangedAsObservable().Skip(1)
                .Subscribe(value =>
                {
                    currentValue.Value = selections[value];
                    PlayerPrefs.SetString(key, selections[value]);
                })
                .AddTo(this);

            // 選択位置の設定
            ResetDropdown();
        }

        public override void ResetParam()
        {
            currentValue.Value = defaultValue;
            ResetDropdown();
        }

        private void ResetDropdown()
        {
            var index = Array.IndexOf(selections, currentValue.Value);
            dropdown.value = index >= 0 ? index : 0;
        }
    }
}