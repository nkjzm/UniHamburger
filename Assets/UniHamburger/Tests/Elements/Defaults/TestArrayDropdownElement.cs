using nkjzm.UniHamburger.Elements.Defaults;
using nkjzm.UniHamburger.Tests.Elements.Base;
using UnityEngine.UI;

namespace nkjzm.UniHamburger.Tests.Elements.Defaults
{
    public class TestArrayDropdownElement : TestBaseArrayDropdownElement<ArrayDropdownElement>
    {
        protected override string FileName => "ArrayDropdownElement";

        protected override void ChangeElementValue(int value)
        {
            target.GetComponentInChildren<Dropdown>().value = value;
        }

        protected override int GetElementCount()
        {
            return target.GetComponentInChildren<Dropdown>().options.Count;
        }

        protected override int GetSelectedIndex()
        {
            return target.GetComponentInChildren<Dropdown>().value;
        }
    }
}