using UnityEngine;

public interface IDamageable
{
    public float Health { set; get;  }

    public bool Targetable { set; get;  }
    public void OnHit(float Damage, Vector2 knockback);
    public void OnHit(float Damage);

    public void Destroyself();

}