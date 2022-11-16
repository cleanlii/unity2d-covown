using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] GameObject[] pagesObj;
    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;
    [SerializeField] int currentIndex;

    public void LeftButtonClick()
    {
        pagesObj[currentIndex].SetActive(false);
        pagesObj[currentIndex - 1].SetActive(true);
        currentIndex--;
        if (currentIndex == 0)
        {
            leftArrow.interactable = false;
        }
        else
        {
            rightArrow.interactable = true;
        }
    }

    public void RightButtonClick()
    {
        pagesObj[currentIndex].SetActive(false);
        pagesObj[currentIndex + 1].SetActive(true);
        currentIndex++;
        if (currentIndex == pagesObj.Length - 1)
        {
            rightArrow.interactable = false;
        }
        else
        {
            leftArrow.interactable = true;
        }
    }

}