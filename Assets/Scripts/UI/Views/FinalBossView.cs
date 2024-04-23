using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FinalBossView : View
{
    [SerializeField] private TextMeshProUGUI tutorial;
    [SerializeField] private GameObject blackPanel;

    [SerializeField] private Animator _sceneTransitionAnimator; // Animator for our scene transition element
    public override void Initialize()
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText()
    {
        tutorial.gameObject.SetActive(true);
    }

    public void FadeText()
    {
        StartCoroutine(FadeOutElement(tutorial.gameObject.GetComponent<CanvasGroup>(), .1f));
    }

    public void ShowPanel()
    {
        blackPanel.SetActive(true);
    }

    public void FadePanel()
    {
        StartCoroutine(FadeOutElement(blackPanel.GetComponent<CanvasGroup>(), .4f));
    }

    public void LeaveSceneTransition()
    {
        _sceneTransitionAnimator.Play("StandardExit", 0, 0);
    }

    private IEnumerator FadeOutElement(CanvasGroup group, float rate)
    {
        while(group.alpha > 0)
        {
            group.alpha -= Time.deltaTime * rate;
            yield return null;
        }
    }
}
