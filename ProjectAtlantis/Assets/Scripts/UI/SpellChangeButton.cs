using Gameplay.Combat.Spells;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SpellChangeButton : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler,
        IEndDragHandler
    {
        [SerializeField] private BaseSpell spell;

        public void OnPointerClick(PointerEventData eventData)
        {
            // TODO: Message with drag and drop notification && Open Small Window with description.
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // TODO: Instantiate Image that follows cursor.
        }

        public void OnDrag(PointerEventData eventData)
        {
            // TODO: Move Image along your cursor.
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // TODO: Swap Spell with much logic.
        }
    }
}