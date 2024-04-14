using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBulletDestroy : MonoBehaviour
{
    [SerializeField] ParticleSystem _particle;
    [SerializeField] GameObject _bullet;
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DoDestroy());
    }

    private IEnumerator DoDestroy()
    {
        Destroy(_bullet);
        _particle.Stop();
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
