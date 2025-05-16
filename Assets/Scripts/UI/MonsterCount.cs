using MyGame.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterCount : MonoBehaviour
{
    public TextMeshProUGUI countText;
    int count = 0;

    private void Start()
    {
        //Transform parentTransform = transform.parent;
        string parentImageName = GetComponentInParent<Image>().sprite.name;
        Debug.Log(parentImageName);

        foreach (string i in MonsterManager.monsterNames)
        {
            if (parentImageName == i)
            {
                count++;
            }
        }

        countText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        countText.text =  "X "+ count.ToString();
    }
}
