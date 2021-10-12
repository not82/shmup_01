using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class ModifierSettings : IInitializable
    {
        public List<ModifierSlot> ModifierSlots;
        public List<Modifier> Modifiers;

        [Inject(Id = "UI/Slots/TL")] public ItemSlot TopLeftSlot;
        [Inject(Id = "UI/Slots/TC")] public ItemSlot TopCenterSlot;
        [Inject(Id = "UI/Slots/TR")] public ItemSlot TopRightSlot;
        [Inject(Id = "UI/Slots/BL")] public ItemSlot BottomLeftSlot;
        [Inject(Id = "UI/Slots/BC")] public ItemSlot BottomCenterSlot;
        [Inject(Id = "UI/Slots/BR")] public ItemSlot BottomRightSlot;

        [Inject(Id = "Ship/Sites/TL")] public Transform TopLeftSiteTransform;
        [Inject(Id = "Ship/Sites/TC")] public Transform TopCenterSiteTransform;
        [Inject(Id = "Ship/Sites/TR")] public Transform TopRightSiteTransform;
        [Inject(Id = "Ship/Sites/BL")] public Transform BottomLeftSiteTransform;
        [Inject(Id = "Ship/Sites/BC")] public Transform BottomCenterSiteTransform;
        [Inject(Id = "Ship/Sites/BR")] public Transform BottomRightSiteTransform;


        [Inject(Id = "UI/Skills/Laser1")] public DragDrop laser1Skill;
        [Inject(Id = "UI/Skills/Laser2")] public DragDrop laser2Skill;
        [Inject(Id = "UI/Skills/Flame")] public DragDrop flameSkill;

        [Inject] private DiContainer _diContainer;

        // [Inject] private Bullet.Pool bulletFactory;
        // [Inject] private Bullet.Pool2 bulletFactory2;

        public void Initialize()
        {
            ModifierSlots = new List<ModifierSlot>();
            ModifierSlots.Add(new ModifierSlot() {ItemSlot = TopLeftSlot, ShipSiteTransform = TopLeftSiteTransform});
            ModifierSlots.Add(new ModifierSlot() {ItemSlot = TopCenterSlot, ShipSiteTransform = TopCenterSiteTransform});
            ModifierSlots.Add(new ModifierSlot() {ItemSlot = TopRightSlot, ShipSiteTransform = TopRightSiteTransform});
            ModifierSlots.Add(new ModifierSlot() {ItemSlot = BottomLeftSlot, ShipSiteTransform = BottomLeftSiteTransform});
            ModifierSlots.Add(new ModifierSlot() {ItemSlot = BottomCenterSlot, ShipSiteTransform = BottomCenterSiteTransform});
            ModifierSlots.Add(new ModifierSlot() {ItemSlot = BottomRightSlot, ShipSiteTransform = BottomRightSiteTransform});
            Modifiers = new List<Modifier>();

            var laser1Modifier = _diContainer.Instantiate<Laser1Modifier>();
            // laser1Modifier.RenderPrefab = (GameObject) Resources.Load("Prefabs/Bullet", typeof(GameObject));
            laser1Modifier.UISkill = laser1Skill;
            Modifiers.Add(laser1Modifier);

            var laser2Modifier = _diContainer.Instantiate<Laser2Modifier>();
            // laser2Modifier.RenderPrefab = (GameObject) Resources.Load("Prefabs/Bullet", typeof(GameObject));
            laser2Modifier.UISkill = laser2Skill;
            Modifiers.Add(laser2Modifier);

            Modifiers.Add(new Modifier()
            {
                RenderPrefab = (GameObject) Resources.Load("Prefabs/Flame", typeof(GameObject)), UISkill = flameSkill
            });
        }
    }
}