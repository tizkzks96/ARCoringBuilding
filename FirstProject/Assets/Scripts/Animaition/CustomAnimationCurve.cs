using System.Collections;
using UnityEngine;

public class CustomAnimationCurve : Singleton<CustomAnimationCurve>
{
    private float uiSize = 400;

    private IEnumerator Scale(bool state, float transitionSpeed, GameObject target)
    {
        RectTransform targetRect = target.GetComponent<RectTransform>();
        if (state == true)
        {
            targetRect.localScale = Vector3.zero;
        }

        while (true)
        {
            //true is open
            if (state == true)
            {

                targetRect.localScale = Vector3.Lerp(targetRect.transform.localScale, Vector3.one * uiSize, Time.deltaTime * transitionSpeed);

                if (Mathf.Abs((Vector2.one).sqrMagnitude - targetRect.transform.localScale.sqrMagnitude) < 5f)
                {
                    targetRect.localScale = Vector3.one * uiSize;
                    break;
                }
            }

            //false is close
            else if (state == false)
            {
                targetRect.localScale = Vector3.Lerp(targetRect.transform.localScale, Vector2.zero, Time.deltaTime * transitionSpeed);
                if (targetRect.transform.localScale.x < .05f)
                {
                    targetRect.transform.localScale = Vector3.zero;
                    break;
                }
            }
            yield return null;
        }

        if (state == false)
        {
            targetRect.transform.localScale = Vector3.one * uiSize;
        }
        yield return null;
    }

    public void ScaleUpAnimationClip(int size, out AnimationClip clip)
    {
        AnimationCurve curve;

        // create a new AnimationClip
        clip = new AnimationClip();
        clip.legacy = true;

        // create a curve to move the GameObject and assign to the clip
        Keyframe[] keys;

        keys = new Keyframe[2];

        curve = new AnimationCurve(keys);

        keys[0] = new Keyframe(0.0f, 0);
        keys[1] = new Keyframe(.5f, size);
        clip.SetCurve("", typeof(RectTransform), "sizeDelta.x", curve);

        keys[0] = new Keyframe(0.0f, 0);
        keys[1] = new Keyframe(.5f, size);
        clip.SetCurve("", typeof(RectTransform), "scale.y", curve);

        keys[0] = new Keyframe(0.0f, 0);
        keys[1] = new Keyframe(.5f, size);
        clip.SetCurve("", typeof(RectTransform), "scale.z", curve);
    }

    public void ScaleDownAnimationClip(int size, out AnimationClip clip)
    {
        AnimationCurve curve;

        // create a new AnimationClip
        clip = new AnimationClip
        {
            legacy = true
        };

        // create a curve to move the GameObject and assign to the clip
        Keyframe[] keys;

        keys = new Keyframe[2];

        curve = new AnimationCurve(keys);

        keys[0] = new Keyframe(0.0f, size);
        keys[1] = new Keyframe(.5f, 0);
        clip.SetCurve("", typeof(RectTransform), "localScale.x", curve);

        keys[0] = new Keyframe(0.0f, size);
        keys[1] = new Keyframe(.5f, 0);
        clip.SetCurve("", typeof(RectTransform), "localScale.y", curve);

        keys[0] = new Keyframe(0.0f, size);
        keys[1] = new Keyframe(.5f, 0);
        clip.SetCurve("", typeof(RectTransform), "localScale.z", curve);
    }
}
