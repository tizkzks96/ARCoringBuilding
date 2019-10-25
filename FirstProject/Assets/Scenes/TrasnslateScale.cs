using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrasnslateScale : MonoBehaviour
{
    public GameObject Target;

    float translateStartTime, transLateValue;
    public float translate_duration = 1f;
    public float easingValue = 0.5f;
    public float startValue = 0.5f;
    public float endValue = 2f;

    float sv;
    bool isPlaying = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           if(!isPlaying) StartCoroutine(Translate_Scale());
        }
    }

    IEnumerator Translate_Scale()
    {
        isPlaying = true;

        translateStartTime = Time.time;
        transLateValue = startValue;
        sv = 0;

        while (sv <= 0.9999f)
        {
            yield return null;

            // time calc
            sv = (Time.time - translateStartTime) / translate_duration;

            /* linear type */
            // var value = = Mathf.Lerp(startValue, endValue, sv);

            /* smooth type */
            // var value = = Mathf.SmoothStep(startValue, endValue, sv);

            /* ease out */
            // var value = (Mathf.Sin(sv * Mathf.PI * 0.5f));

            /* ease in */
            // var value = 1- (Mathf.Cos(sv * Mathf.PI * 0.5f));

            /* ease in out */
            var value = sv * sv * (3.0f - 2.0f * sv);

            /* pow */
            // value = Mathf.Pow(value, easingValue * Time.time );

            transLateValue = Mathf.Lerp(startValue, endValue, value);

            // animation
            Target.transform.localScale = new Vector3(transLateValue, transLateValue, transLateValue);
        }

        isPlaying = false;
    }


}
