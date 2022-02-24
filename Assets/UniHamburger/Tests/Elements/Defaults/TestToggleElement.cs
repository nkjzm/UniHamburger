using nkjzm.UniHamburger.Elements.Defaults;
using nkjzm.UniHamburger.Tests.Elements.Base;
using UnityEngine.UI;

namespace nkjzm.UniHamburger.Tests.Elements.Defaults
{
    public class TestToggleElement : TestBaseToggleElement<ToggleElement>
    {
        protected override string FileName => "ToggleElement";

        protected override void Switch()
        {
            var toggle = target.GetComponentInChildren<Toggle>();
            toggle.isOn = !toggle.isOn;
        }
    }
}