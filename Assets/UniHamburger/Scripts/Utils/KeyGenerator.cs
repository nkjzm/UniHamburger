using System.Diagnostics;

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
            return $"{new StackFrame(1).GetMethod().ReflectedType}+{paramId}+{seed}";
        }
    }
}