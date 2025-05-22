using MyGame.Managers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoPanel : MonoBehaviour
{
    public GameObject monsterInfoBtn;
    public GameObject imageTemplate;

    private Button XBtn;

    private int flag = 0;
    void Start()
    {
        XBtn = GetComponentInChildren<Button>();
        XBtn.onClick.AddListener(XBtnClicked);

        Transform contentPanel = this.transform;

        // existed image delete
        Image[] images = GetComponentsInChildren<Image>();

        foreach (Image image in images)
        {
            if (image.gameObject.name.StartsWith("Monster"))
            {
                //Debug.Log(image.gameObject.name);
                Destroy(image.gameObject); //Monster image firnd and delete
            }
        }


        //show monster image
        foreach (string imageName in MonsterManager.monsterNames)
        {
            Sprite sprite = Resources.Load<Sprite>($"Image/{imageName}");

            if (sprite != null)
            {
                GameObject newImage = Instantiate(imageTemplate, contentPanel);
                newImage.GetComponent<Image>().sprite = sprite;
                newImage.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"[ImageListManager] '{imageName}' 이미지가 Resources/Image 폴더에 없습니다.");
            }

        }
    }
    void XBtnClicked()
    {
        this.gameObject.SetActive(false);
        monsterInfoBtn.SetActive(true);
    }
}
