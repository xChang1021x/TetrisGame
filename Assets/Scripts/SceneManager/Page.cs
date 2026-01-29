using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    public GameObject previousButton;
    public GameObject nextButton;
    public Image pageImage;
    public int pageNumber = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        pageNumber = 1;
        SetPage(pageNumber);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPage(int page)
    {
        if (page == 1)
        {
            previousButton.SetActive(false);
        }
        else
        {
            previousButton.SetActive(true);
        }

        if (page == 3)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }
        pageImage.sprite = Resources.Load<Sprite>("GameInstruction" + page);
    }

    public void NextPage()
    {
        pageNumber++;
        SetPage(pageNumber);
    }

    public void PreviousPage()
    {
        pageNumber--;
        SetPage(pageNumber);
    }

    void OnDisable()
    {
        pageNumber = 1;
    }
}
