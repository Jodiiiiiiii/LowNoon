using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverView : View
{
    [SerializeField, Tooltip("Object to be faded in for fade to black death")] private Image _blackImage;
    private const float _fadeToBlackDuration = 3f;
    private float _timer;
    private bool _selectionMade = false;
    [SerializeField] private Animator _sceneTransitionAnimator; // Animator for our scene transition element

    public override void Initialize()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0; // reset timer for fade duration
    }

    // Update is called once per frame
    void Update()
    {   
        if(GameManager.Instance.PlayerData.CrumblingDeath)
        {
            // update transparency of fade to black image
            Color curr = _blackImage.color;
            curr.a = Mathf.Lerp(0, 1, Mathf.Clamp(_timer / _fadeToBlackDuration, 0f, 1f));
            _blackImage.color = curr;

            _timer += Time.deltaTime;
        }
    }

    public void RetryButton()
    {
        if (!_selectionMade)
        {
            _selectionMade = true;
            GameManager.Instance.IsMainMenuLoaded = true; // makes player come out of coffin
            //StopAllCoroutines();
            StartCoroutine(DoLeaveScene());
        }
        
    }

    public void MainMenu()
    {
        if (!_selectionMade)
        {
            _selectionMade = true;  
            GameManager.Instance.IsMainMenuLoaded = false; // makes player go to main menu (not coffin)
            //StopAllCoroutines();
            StartCoroutine(DoLeaveScene());
        }
        
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        // time scale should not be frozen, or player death animation won't play out
    }

    private void OnDisable()
    {
    }

    private IEnumerator DoLeaveScene()
    {
        _sceneTransitionAnimator.Play("StandardExit", 0, 0);
        float timer = 2;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("0_Hub");
        //Debug.Log("Loading to hub");
        yield return null;
    }
}
