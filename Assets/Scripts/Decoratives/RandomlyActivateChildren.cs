using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyActivateChildren : MonoBehaviour
{
    [SerializeField, Tooltip("Number of child objects which will be activated - do not overlap any children if >1")] private int _numToActivate = 1;
    [SerializeField, Tooltip("This list of objects will ALWAYS be included in the list to be activated")] private List<GameObject> _requiredObjsInGroup;

    private List<GameObject> _children = new List<GameObject>();
    private bool _abortGeneration = false;

    // Start is called before the first frame update
    void Start()
    {
        // check for invalid use of script - not enough objects present
        if (_numToActivate > gameObject.transform.childCount) // should prevent infinite loop down below
            AbortGeneration("NumToActivate cannot be larger than number of children. Aborting generation on object: " + gameObject.name);
        //  check for invalid use of script - too many required objects
        if (_requiredObjsInGroup.Count > _numToActivate)
            AbortGeneration("RequiredObjsInGroup count cannot be larger than NumToActivate. Aborting generation on object: " + gameObject.name);

        if (!_abortGeneration) // prevent more severe errors
        {
            // retrieve all child objects
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                _children.Add(gameObject.transform.GetChild(i).gameObject);
                _children[i].SetActive(false); // disable all by default
            }

            // activate numToActivate objects
            List<int> randomNums = new List<int>();
            // enable (and add to randomNums) each required object
            foreach (GameObject obj in _requiredObjsInGroup)
            {
                // check if required obj is actually a child
                if (_children.IndexOf(obj) == -1)
                {
                    AbortGeneration("Object " + obj.name + " not found as child. Aborting generation on object: " + gameObject.name);
                    break;
                }

                // store consumed index and activate
                randomNums.Add(_children.IndexOf(obj));
                _children[_children.IndexOf(obj)].SetActive(true);
            }

            if(!_abortGeneration) // check once again (prevent infinite loop)
            {
                for (int i = 0; i < _numToActivate - _requiredObjsInGroup.Count; i++) // add more randomly to reach desired numToActivate
                {
                    // generate new unique index
                    int num;
                    do num = Random.Range(0, _children.Count);
                    while (randomNums.Contains(num));

                    // store consumed index and activate
                    randomNums.Add(num);
                    _children[num].SetActive(true);
                }
            }        
        }
    }

    private void AbortGeneration(string debugMessage)
    {
        Debug.LogError(debugMessage);
        Destroy(gameObject); // do not continue with generation
        _abortGeneration = true;
    }
}
