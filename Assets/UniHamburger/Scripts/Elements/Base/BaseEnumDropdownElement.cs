using System;
using nkjzm.UniHamburger.Elements.Interface;
using UnityEngine;

namespace nkjzm.UniHamburger.Elements.Base
{
    /// <summary>
    /// 列挙型ドロップダウン要素の基底クラス
    /// </summary>
    public abstract class BaseEnumDropdownElement : MonoBehaviour, IResettable
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
        public abstract IObservable<T> Setup<T>(string label, T defaultValue, string description = "",
            string salt = "", IObservable<bool> activeUpdated = null) where T : Enum;

        public abstract void ResetParam();
    }
}