using System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    // TODO Séparer le script qui gère l'effet du reste de la définition ( et le rendre injecté ) 
    public class Modifier
    {
        public GameObject RenderPrefab;
        public DragDrop UISkill;
        
        public virtual void Fire(Transform SiteTransform)
        {
            Debug.Log("FIRE OLD");
        }
    }
}