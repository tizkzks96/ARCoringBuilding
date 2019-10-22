using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomySystem : Singleton<EconomySystem>
{

    /// <summary>
    /// target 오브젝트에 일정 간격으로 증가하는 게이지 부착해서 시작
    /// </summary>
    /// <param name="target"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator IncreseGage(GameObject target, float time, float increseMoney)
    {
        GameObject gage = target.transform.Find("Canvas").transform.Find("Image").gameObject;
        Image image = gage.GetComponent<Image>();

        while (true)
        {
            if (image.fillAmount < 1)
            {
                image.fillAmount += 1.0f / time * Time.deltaTime;
            }
            else
            {
                image.fillAmount = 0;

                GameManager.Money += increseMoney;
                GameManager.Instance.moneyText.text = GameManager.Money.ToString();
            }
            yield return null;
        }
    }
}
