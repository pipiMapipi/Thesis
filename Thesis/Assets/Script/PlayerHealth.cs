using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool Targetable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    [Header("Health")]
    [SerializeField] private float _health = 3f;

    [Header("Target")]
    [SerializeField] private bool _targetable = true;
    public void Destroyself()
    {
        throw new System.NotImplementedException();
    }

    public void OnHit(float Damage, Vector2 knockback)
    {
        throw new System.NotImplementedException();
    }

    public void OnHit(float Damage)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
