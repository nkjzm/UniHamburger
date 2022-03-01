using nkjzm.UniHamburger.Elements.Defaults;
using nkjzm.UniHamburger.Tests.Elements.Base;
using UnityEngine.UI;

namespace nkjzm.UniHamburger.Tests.Elements.Defaults
{
    public class TestEnumDropdownElement : TestBaseEnumDropdownElement<EnumDropdownElement>
    {
        protected override string FileName => "EnumDropdownElement";

        protected override void ChangeElementValue(int value)
        {
            target.GetComponentInChildren<Dropdown>().value = value;
        }

        protected override int GetSelectedIndex()
        {
            return target.GetComponentInChildren<Dropdown>().value;
        }
    }
}