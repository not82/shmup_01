using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class ModifierController
    {
        [Inject] public ModifierSettings ModifierSettings;

        public List<Modifier> Modifiers;

        public void EquipedHandler(ItemSlot itemSlot, DragDrop dragDrop)
        {
            var modifier = ModifierSettings.Modifiers.FirstOrDefault(modifier => modifier.UISkill == dragDrop);
            var newModifierSlot = ModifierSettings.ModifierSlots.FirstOrDefault(slot => slot.ItemSlot == itemSlot);

            // We disequiped it from previous
            Debug.Log("modifier");
            Debug.Log(modifier);
            var previousModifierSlot = GetModifierSlotByModifier(modifier);
            Debug.Log("previousModifierSlot");
            Debug.Log(previousModifierSlot);
            if (previousModifierSlot != null)
            {
                previousModifierSlot.Modifier = null;
                Debug.Log("REMOVE FROM");
                Debug.Log(previousModifierSlot.ShipSiteTransform.name);
                // previousModifierSlot.ShipSiteTransform.DetachChildren();
                for(int i = 0; i < previousModifierSlot.ShipSiteTransform.GetChildCount(); i++)
                {
                    Debug.Log(i);
                    var tr = previousModifierSlot.ShipSiteTransform.GetChild(i);
                    GameObject.Destroy(tr.gameObject);
                }
            }

            // Equip the new slot
            if (newModifierSlot != null)
            {
                // Debug.Log("modifier slot found !");
                // if (modifier != null)
                // {
                //     Debug.Log("modifier found too !");
                // }

                newModifierSlot.Modifier = modifier;
                // Debug.Log(ms.Modifier.RenderPrefab);

                //  Ship render
                if (newModifierSlot.Modifier.RenderPrefab != null)
                {
                    var render = Object.Instantiate(newModifierSlot.Modifier.RenderPrefab, new Vector3(0, 0, 0),
                        Quaternion.identity);
                    render.transform.SetParent(newModifierSlot.ShipSiteTransform.transform);
                    render.transform.localPosition = new Vector3(0, 0, 0);
                }
            }
            else
            {
                Debug.Log("modifier slot not found !");
            }
        }

        public ModifierSlot GetModifierSlotByModifier(Modifier modifier)
        {
            return ModifierSettings.ModifierSlots.FirstOrDefault(slot => slot.Modifier == modifier);
        }
    }
}