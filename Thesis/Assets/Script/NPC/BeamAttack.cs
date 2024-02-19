using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LineRenderer linerenderer;
    [SerializeField] private Transform firePoint;

    [Header("VFX")]
    [SerializeField] private GameObject startVFX;
    [SerializeField] private GameObject endVFX;

    private Quaternion rotation;
    private List<ParticleSystem> particles = new List<ParticleSystem>();

    void OnEnable()
    {
        startVFX.SetActive(false);
        endVFX.SetActive(false);
        AddToList();
        DisableBeam();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnableBeam();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            UpdateBeam();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            DisableBeam();
        }

        RotateToTarget();
    }

    private void EnableBeam()
    {
        linerenderer.enabled = true;

        startVFX.SetActive(true);
        endVFX.SetActive(true);

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();
        }
    }

    private void UpdateBeam()
    {
        var mousePos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        linerenderer.SetPosition(0, (Vector2)firePoint.position);
        startVFX.transform.position = (Vector2)firePoint.position;

        linerenderer.SetPosition(1, mousePos);

        Vector2 direction = mousePos - (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, direction.magnitude);
        if (hit)
        {
            linerenderer.SetPosition(1, hit.point);
        }
        endVFX.transform.position = linerenderer.GetPosition(1);
    }

    private void DisableBeam()
    {
        linerenderer.enabled = false;

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
        
    }

    private void RotateToTarget()
    {
        Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rotation;
    }

    private void AddToList()
    {
        for(int i = 0; i < startVFX.transform.childCount; i++)
        {
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps != null)
            {
                particles.Add(ps);
            }
        }

        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
        }
    }
}
