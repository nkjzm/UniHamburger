using System;
using nkjzm.UniHamburger.Elements.Interface;
using UnityEngine;

namespace nkjzm.UniHamburger.Elements.Base
{
    /// <summary>
    /// トグル要素の基底クラス
    /// </summary>
    public abstract class BaseToggleElement : MonoBehaviour, IResettable
    {
        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="label"> 表示するラベル </param>
        /// <param name="defaultValue"> 初期値 </param>
        /// <param name="description"> 詳細 </param>
        /// <param name="salt"> 保存用のソルト </param>
        /// <param name="activeUpdated"> 有効化切り替え </param>
        /// <returns> 値の変更通知 </returns>
        public abstract IObservable<bool> Setup(string label, bool defaultValue, string description = "",
            string salt = "", IObservable<bool> activeUpdated = null);

        public abstract void ResetParam();
    }
}