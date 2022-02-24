using System;
using nkjzm.UniHamburger.Elements.Base;
using nkjzm.UniHamburger.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace nkjzm.UniHamburger.Elements.Defaults
{
    /// <summary>
    /// 整数スライダー要素クラス
    /// </summary>
    public sealed class IntSliderElement : BaseIntSliderElement
    {
        [SerializeField] private Text label;
        [SerializeField] private Text Value;
        [SerializeField] private Button Left, Right;
        [SerializeField] private Text description;
        private IntReactiveProperty currentValue;
        private int defaultValue;
        private int unit, min, max;

        public override IObservable<int> Setup(string label, int defaultValue, int unit = 1, int min = int.MinValue,
            int max = int.MaxValue, string description = "", string salt = "", IObservable<bool> activeUpdated = null)
        {
            this.label.text = label;
            this.description.text = description;
            this.description.enabled = !string.IsNullOrEmpty(description);
            this.defaultValue = defaultValue;
            this.unit = unit;
            this.min = min;
            this.max = max;

            Right.onClick.AddListener(AddValue);
            Left.onClick.AddListener(SubtractValue);

            var key = KeyGenerator.CreateKey(label, salt);
            currentValue = new IntReactiveProperty(PlayerPrefs.GetInt(key, defaultValue));
            currentValue.Subscribe(value =>
            {
                PlayerPrefs.SetInt(key, value);
                Value.text = value.ToString();
                Left.gameObject.SetActive(value > min);
                Right.gameObject.SetActive(value < max);
            }).AddTo(this);

            activeUpdated?.Subscribe(isEnabled => Left.interactable = Right.interactable = isEnabled).AddTo(this);

            return currentValue;
        }

        public override void AddValue()
        {
            currentValue.Value = Mathf.Clamp(currentValue.Value + unit, min, max);
        }

        public override void SubtractValue()
        {
            currentValue.Value = Mathf.Clamp(currentValue.Value - unit, min, max);
        }

        public override void ResetParam()
        {
            currentValue.Value = defaultValue;
        }
    }
}