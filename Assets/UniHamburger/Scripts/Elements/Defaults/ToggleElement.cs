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
    /// トグル要素クラス
    /// </summary>
    public sealed class ToggleElement : BaseToggleElement
    {
        [SerializeField] private Text Label;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Text Description;
        private BoolReactiveProperty currentValue;
        private bool defaultValue;

        // 命名微妙かもしれない
        public override IObservable<bool> Setup(string label, bool defaultValue, string description = "",
            string salt = "", IObservable<bool> activeUpdated = null)
        {
            Label.text = label;
            this.defaultValue = defaultValue;
            Description.text = description;
            Description.enabled = !string.IsNullOrEmpty(description);

            var key = KeyGenerator.CreateKey(label, salt);
            currentValue = new BoolReactiveProperty(PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1);
            currentValue.Subscribe(isOn => PlayerPrefs.SetInt(key, isOn ? 1 : 0));

            // Toggle初期化
            toggle.isOn = currentValue.Value;
            // Toggleに変化があったら代入
            toggle.OnValueChangedAsObservable()
                .Subscribe(value => currentValue.Value = value)
                .AddTo(this);

            activeUpdated?.Subscribe(isEnabled =>
                    toggle.GetComponentsInChildren<Button>().ToList()
                        .ForEach(button => button.interactable = isEnabled))
                .AddTo(this);

            return currentValue.AsObservable();
        }

        public override void ResetParam()
        {
            toggle.isOn = defaultValue;
        }
    }
}