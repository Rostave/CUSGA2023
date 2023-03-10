using R0.Character;
using R0.ScriptableObjConfig;

namespace R0.SpellRel
{
    public class ElementSpell : Spell
    {
        public override void Apply()
        {
            var data = (SpellData.Instance.GetElementSpellData(spellCat));
            CharaMgr.Instance.activeChara.weapon.AddElement(data.elementType);
        }
    }
}