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

    public void ScaleDownAnimationClip(float size, out AnimationClip clip)
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

    public IEnumerator RotationTargetAnimation(GameObject target, float speed = 90)
    {
        print("start co");
        float rv = 0;
        //float duration = 0;

        while (target.activeSelf == true)
        {
            yield return null;

            rv += Time.deltaTime * speed;
            //duration += Mathf.Abs(rv);

            target.transform.localRotation =
                    Quaternion.Euler(target.transform.localRotation.x, target.transform.localRotation.y + rv, target.transform.localRotation.z);
        }
    }

    public IEnumerator ScaleDownTargetAnimation(GameObject target, float speed, float time)
    {
        float sv = 0;
        float duration = 0;
        float tick = target.transform.localScale.x / time;

        while (target.transform.localScale.sqrMagnitude < 00.00001f)
        {
            yield return null;
            sv += Time.deltaTime * speed;
            duration += Mathf.Abs(sv);

            target.transform.localScale =
                    new Vector3(target.transform.localScale.x - tick, target.transform.localScale.y - tick, target.transform.localScale.z - tick);
        }
        target.SetActive(false);
    }

    public void TransformRotationLeftAnimationClip(Vector3 eulerAngle, out AnimationClip clip)
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

        keys[0] = new Keyframe(0.0f, eulerAngle.x);
        keys[1] = new Keyframe(.5f, 0);
        clip.SetCurve("", typeof(RectTransform), "rotation.x", curve);

        keys[0] = new Keyframe(0.0f, eulerAngle.y);
        keys[1] = new Keyframe(.5f, 0);
        clip.SetCurve("", typeof(RectTransform), "rotation.y", curve);

        keys[0] = new Keyframe(0.0f, eulerAngle.z);
        keys[1] = new Keyframe(.5f, 0);
        clip.SetCurve("", typeof(RectTransform), "rotation.z", curve);
    }

    public void PlayAnimation(Animation anim, string clipName)
    {
        // update the clip to a change the red color
        //curve = AnimationCurve.Linear(0.0f, 1.0f, 2.0f, 0.0f);
        //clip.SetCurve("", typeof(Material), "_Color.r", curve);
        //clip.wrapMode = WrapMode.Loop;
        // now animate the GameObject
        //anim.AddClip(clip, clip.name);

        anim.clip = anim.GetClip(clipName);
        anim.Play();
    }
    public void InitAnimationClip()
    {
        //ScaleUpAnimationClip(400, out scaleUpclip);
        //ScaleDownAnimationClip(400, out scaleDownclip);
    }
}
