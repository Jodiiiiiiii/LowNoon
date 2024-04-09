using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubAnimator : MonoBehaviour
{
    // Hub Animator is a single-use animation script for 
    [SerializeField] private float _delay;  // How long before we play the animation this object is attached to
    [SerializeField] private float _duration; // How long this animation takes
    [SerializeField] private string _name;  // The name of the animation we're playing
    [SerializeField] private bool _isDestroyed; // Whether this object is destroyed at the end of its animation
    private Animator _animator;
    private void OnEnable()
    {
        GameManager.onHubRevive += PlayAnimation;
    }

    private void OnDisable()
    {
        GameManager.onHubRevive -= PlayAnimation;
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayAnimation()
    {
        StartCoroutine(DoAnimation());
    }

    private IEnumerator DoAnimation()
    {
        yield return new WaitForSeconds(_delay);
        _animator.Play(_name, 0, 0);
        yield return new WaitForSeconds(_duration);
        if( _isDestroyed)
        {
            Destroy(gameObject);
        }
    }
}
