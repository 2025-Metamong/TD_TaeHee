using UnityEngine;
using UnityEngine.UI;

public class MonsterInfo : MonoBehaviour
{
    public GameObject panel;

    private Button monsterInfoBtn;

    void Start()
    {
        monsterInfoBtn = GetComponent<Button>();
        monsterInfoBtn.onClick.AddListener(MonsterInfoClicked);
    }
    void MonsterInfoClicked()
    {
        panel.SetActive(true); // UI ∫∏¿Ã±‚
        //this.gameObject.SetActive(false);
    }
}
