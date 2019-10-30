using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : Singleton<ObjectDestroy>
{
    public GameObject target;

    public void DestroyObejct()
    {
        if(target != null)
        {
            GameObject temp = Instantiate(MainUI.Instance.destroyParticle, target.transform);
            temp.transform.localPosition = Vector3.zero + Vector3.up * 2;
            temp.transform.localScale = Vector3.one * 15;
            if(target.transform.GetChild(1).GetComponent<Outline>() != null)
            {
                target.transform.GetChild(1).GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;

            }
            StartCoroutine(CreateDelay(target, 5.0f));

        }
    }

    public IEnumerator CreateDelay_(GameObject target, float time)
    {
        float sp = 0;
        while (sp < time)
        {
            yield return null;

            sp += Time.deltaTime;
            target.transform.GetChild(1).transform.localPosition = new Vector3(target.transform.GetChild(1).transform.localPosition.x, -sp * 2, target.transform.GetChild(1).transform.localPosition.z);
        }
        Destroy(target);
        //target.SetActive(true);
    }

    public IEnumerator CreateDelay(GameObject target, float time)
    {
        float sp = 0;
        while (sp < time)
        {
            yield return null;

            sp += Time.deltaTime;
            target.transform.GetChild(1).transform.localPosition = new Vector3(target.transform.GetChild(1).transform.localPosition.x, -sp*2, target.transform.GetChild(1).transform.localPosition.z); 
        }
        Destroy(target);
        //target.SetActive(true);
    }

}
