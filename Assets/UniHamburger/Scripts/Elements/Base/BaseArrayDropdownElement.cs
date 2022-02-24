using System;
using nkjzm.UniHamburger.Elements.Interface;
using UnityEngine;

namespace nkjzm.UniHamburger.Elements.Base
{
    /// <summary>
    /// 配列ドロップダウン要素の基底クラス
    /// </summary>
    public abstract class BaseArrayDropdownElement : MonoBehaviour, IResettable
    {
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
        public abstract IObservable<string> Setup(string label, string defaultValue, Func<string[]> getSelection,
            string description = "", string salt = "", IObservable<bool> activeUpdated = null);

        public abstract void ResetParam();

        /// <summary>
        /// ドロップダウンを初期化
        /// </summary>
        public abstract void InitDropdown();
    }
}