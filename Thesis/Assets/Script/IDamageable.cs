using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDamageable
{
    public float Health { set; get;  }

    public float MaxHealth { set; get; }

    public bool Targetable { set; get;  }

    public bool Invincible { set; get;  }

    public float Density { set; get; }

    public float Aggro { set; get; }
    public void OnHit(float Damage, Vector2 knockback);
    public void OnHit(float Damage);

    public void OnHit(Vector2 knockback);
    public void Destroyself();

}