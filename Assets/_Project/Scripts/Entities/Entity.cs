﻿using System.Collections.Generic;
using UnityEngine;
using static dragoni7.AbstractScriptableEntity;

namespace dragoni7
{
    public class Entity : MonoBehaviour
    {
        public Rigidbody2D rb;
        public List<AbstractAbility> Abilities { get; set; }

        public bool canMove;
        public bool canAttack;
        public virtual void TakeDamage(int damage)
        {

        }
    }
}
