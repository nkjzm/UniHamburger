#if !UNITY_WEBGL || UNITY_EDITOR
using System.Diagnostics;
#endif

namespace nkjzm.UniHamburger.Utils
{
    /// <summary>
    /// キー生成器
    /// </summary>
    public static class KeyGenerator
    {
        /// <summary>
        /// キーを生成する
        /// </summary>
        public static string CreateKey(string paramId, string seed = "")
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return $"{paramId}+{seed}";
#else
            return $"{new StackFrame(1).GetMethod().ReflectedType}+{paramId}+{seed}";
#endif
        }
    }
}