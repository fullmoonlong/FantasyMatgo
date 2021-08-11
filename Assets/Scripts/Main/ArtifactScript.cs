using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ArtifactScript : MonoBehaviour
{
    public List<Shop> MyShopList;
    public List<Shop> EquipList;

    public GameObject[] CollectionSlot;
    public Image[] EquipSlot;

    public GameObject CanEquipPanel;
    
    int curNum;

    int equipNum;

    // Start is called before the first frame update
    void Start()
    {
        equipNum = 0;
        curNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Ati").activeSelf)
        {
            LoadCollection();
        }
    }

    public void LoadCollection()
    {
        MyShopList = ShopManager.instance.AllShopList.FindAll(x => x.IsHaving);

        for (int i = 0; i < CollectionSlot.Length; i++) // 카드 슬롯만큼 돌림
        {
            CollectionSlot[i].SetActive(i < MyShopList.Count); // 내 카드리스트 수보다 적으면 슬롯을 안보이게함
            CollectionSlot[i].transform.GetChild(1).GetComponent<Text>().text = i < MyShopList.Count ? MyShopList[i].Name : "";
            if (i < MyShopList.Count)
            {
                CollectionSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = ShopManager.instance.AllShopSprite[ShopManager.instance.AllShopList.FindIndex(x => x.Name == MyShopList[i].Name)];
            }
        }

        EquipList = MyShopList.FindAll(x => x.IsEquip);
        for (int i=0;i< EquipList.Count;i++)
        {
            EquipSlot[i].sprite = ShopManager.instance.AllShopSprite[ShopManager.instance.AllShopList.FindIndex(x => x.Name == EquipList[i].Name)];
        }
    }

    public void CheckEquip(int num)
    {
        curNum = num;

        //GameObject btn = EventSystem.current.currentSelectedGameObject;
        //btn.
        for(int i=0;i<CollectionSlot.Length;i++)
        {
            CollectionSlot[i].transform.GetChild(2).gameObject.SetActive(false);
        }
        CollectionSlot[curNum].transform.GetChild(2).gameObject.SetActive(true);
        //CanEquipPanel.SetActive(true);

        //CanEquipPanel.transform.GetChild(0).GetComponent<Text>().text = "장착 하시겠습니까?";
    }
    public void EquipArtifact()
    {
        
        print(equipNum);
        if(!MyShopList[curNum].IsEquip && equipNum < 3)
        {
            EquipSlot[equipNum].sprite = CollectionSlot[curNum].transform.GetChild(0).GetComponent<Image>().sprite;
            equipNum++;
        }

        MyShopList[curNum].IsEquip = true;
        ShopManager.instance.SaveFile();

        CollectionSlot[curNum].transform.GetChild(2).gameObject.SetActive(false);
    }
}
