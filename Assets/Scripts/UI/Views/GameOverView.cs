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
        //IDK how to get this to not load the main menu
        SceneManager.LoadScene("0_Hub");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("0_Hub");
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
}
