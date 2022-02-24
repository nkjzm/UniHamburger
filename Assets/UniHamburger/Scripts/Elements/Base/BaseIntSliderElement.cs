using System;
using nkjzm.UniHamburger.Elements.Interface;
using UnityEngine;

namespace nkjzm.UniHamburger.Elements.Base
{
    /// <summary>
    /// 整数スライダー要素の基底クラス
    /// </summary>
    public abstract class BaseIntSliderElement : MonoBehaviour, ISlider, IResettable
    {
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
        public abstract IObservable<int> Setup(string label, int defaultValue, int unit = 1, int min = int.MinValue,
            int max = int.MaxValue, string description = "", string salt = "", IObservable<bool> activeUpdated = null);

        public abstract void AddValue();
        public abstract void SubtractValue();
        public abstract void ResetParam();
    }
}