using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace DefaultNamespace
{
    public class ItemSlot : MonoBehaviour, IDropHandler
    {
        [Inject] public ModifierController ModifierController;

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("DROP!");
            if (eventData.pointerDrag != null)
            {
                // eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                //     GetComponent<RectTransform>().anchoredPosition;

                var draggedItemRT = eventData.pointerDrag.GetComponent<RectTransform>();
                var slotRT = GetComponent<RectTransform>();

                draggedItemRT.SetParent(slotRT);
                draggedItemRT.anchoredPosition = Vector2.zero;

                ModifierController.EquipedHandler(this, eventData.pointerDrag.GetComponent<DragDrop>());
            }
        }
    }
}