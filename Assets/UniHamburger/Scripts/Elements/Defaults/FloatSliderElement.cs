using System;
using nkjzm.UniHamburger.Elements.Base;
using nkjzm.UniHamburger.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace nkjzm.UniHamburger.Elements.Defaults
{
    /// <summary>
    /// 浮動小数スライダー要素クラス
    /// </summary>
    public sealed class FloatSliderElement : BaseFloatSliderElement
    {
        [SerializeField] private Text Label;
        [SerializeField] private Text Value;
        [SerializeField] private Button Left, Right;
        [SerializeField] private Text Description;
        private IntReactiveProperty offsetValue;
        private float defaultValue;
        private float unit;
        private float min, max;

        public override IObservable<float> Setup(string label, float defaultValue, float unit = 0.1f,
            float min = float.MinValue, float max = float.MaxValue, string format = "F2", string description = "",
            string salt = "", IObservable<bool> activeUpdated = null)
        {
            Label.text = label;
            Description.text = description;
            Description.enabled = !string.IsNullOrEmpty(description);
            this.defaultValue = defaultValue;
            this.unit = unit;
            this.min = min;
            this.max = max;

            Right.onClick.AddListener(AddValue);
            Left.onClick.AddListener(SubtractValue);

            var key = KeyGenerator.CreateKey(label, salt);
            var currentValue = PlayerPrefs.GetFloat(key, defaultValue);
            offsetValue = new IntReactiveProperty((int)Mathf.Round((currentValue - defaultValue) / unit));
            offsetValue.Subscribe(offset =>
            {
                var value = Convert(defaultValue, unit, offset);
                PlayerPrefs.SetFloat(key, value);
                Value.text = value.ToString(format);
                Left.gameObject.SetActive(!IsMin);
                Right.gameObject.SetActive(!IsMax);
            }).AddTo(this);

            activeUpdated?.Subscribe(isEnabled => Left.interactable = Right.interactable = isEnabled).AddTo(this);

            return offsetValue.Select(offset => Convert(defaultValue, unit, offset)).AsObservable();
        }

        private bool IsMin => Convert(defaultValue, unit, offsetValue.Value - 1) < min;
        private bool IsMax => Convert(defaultValue, unit, offsetValue.Value + 1) > max;

        public override void AddValue()
        {
            if (IsMax) return;

            ++offsetValue.Value;
        }

        public override void SubtractValue()
        {
            if (IsMin) return;

            --offsetValue.Value;
        }

        private static float Convert(float defaultValue, float unit, int offset)
        {
            return defaultValue + unit * offset;
        }

        public override void ResetParam() => offsetValue.Value = 0;
    }
}