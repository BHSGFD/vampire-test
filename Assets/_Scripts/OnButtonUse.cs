using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnButtonUse
{
    void OnAction();
    void OnEnter();
    void OnLeave();
}

public class OnButtonUse : MonoBehaviour, IOnButtonUse
{
    [SerializeField]
    GameObject Lightning;
    [SerializeField]
    GameObject door;
    bool whatNow = true;
    void IOnButtonUse.OnEnter()
    {
        Debug.Log("SomeWork");
        if (whatNow)
        {
            Lightning.SetActive(true);
         //   door.transform.position += new Vector3(0, 20, 0);
        //    whatNow = false;
        }
     //  else {
     //      Lightning.SetActive(false);
     //      door.transform.position += new Vector3(0, -20, 0);
     //      whatNow = true;
     //  }
      }
    void IOnButtonUse.OnAction()
    {
        door.transform.position += new Vector3(0, 20, 0);
        StartCoroutine(DoorClose());
    }

    IEnumerator DoorClose()
    {
        yield return new WaitForSeconds(3f);
        door.transform.position -= new Vector3(0, 20, 0);
    }
        void IOnButtonUse.OnLeave()
    {
        Lightning.SetActive(false);
    }
     //
}
