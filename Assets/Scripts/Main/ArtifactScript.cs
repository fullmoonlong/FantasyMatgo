using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ArtifactScript : MonoBehaviour
{
    public List<Shop> MyShopList;
    public List<int> EquipList;

    public GameObject[] CollectionSlot;
    public Image[] EquipSlot;

    public GameObject CanEquipPanel;
    
    int curNum;

    public Sprite selectSprite, basicSprite;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        curNum = 0;
        for(int i=0;i<3;i++)
        {
            if (PlayerPrefs.HasKey("Equip" + i))
            {
                EquipList.Add(PlayerPrefs.GetInt("Equip" + i)); // 값을 가지고 있으면 장착 리스트에 모든 샵 리스트에있는 인덱스를 들고온다.
            }
        }

        
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

        for (int i=0;i< EquipList.Count;i++)
        {
            //PlayerPrefs.GetInt("Equip" + i)
            EquipSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = ShopManager.instance.AllShopSprite[EquipList[i]];
        }
    }

    public void CheckEquip(int num)
    {
        for (int i = 0; i < EquipSlot.Length; i++)
        {
            EquipSlot[i].sprite = basicSprite;
        }

        curNum = num;

        for(int i=0;i<CollectionSlot.Length;i++)
        {
            CollectionSlot[i].transform.GetChild(2).gameObject.SetActive(false);
        }
        CollectionSlot[curNum].transform.GetChild(2).gameObject.SetActive(true);
    }
    public void EquipArtifact()
    {
        
        if (!CheckSameArtifact())
        {
            if (EquipList.Count < 3)// 장착이 3개 이하일 때
            {
                PlayerPrefs.SetInt("Equip" + EquipList.Count, ShopManager.instance.AllShopList.FindIndex(x => x.Name == MyShopList[curNum].Name));
                EquipList.Add(PlayerPrefs.GetInt("Equip" + EquipList.Count));
            }

            else
            {
                for (int i = 0; i < EquipSlot.Length; i++)
                {
                    EquipSlot[i].sprite = selectSprite;
                }
            }
        }

        CollectionSlot[curNum].transform.GetChild(2).gameObject.SetActive(false);

    }

    public void ChangeArtifacgt(int num)
    {
        if(EquipSlot[0].sprite == selectSprite)
        {
            EquipList.RemoveAt(num);
            PlayerPrefs.SetInt("Equip" + num, ShopManager.instance.AllShopList.FindIndex(x => x.Name == MyShopList[curNum].Name));
            EquipList.Insert(num, PlayerPrefs.GetInt("Equip" + num));
            for (int i = 0; i < EquipSlot.Length; i++)
            {
                EquipSlot[i].sprite = basicSprite;
            }
        }
     
    }

    bool CheckSameArtifact()
    {
        for(int i=0;i<EquipList.Count;i++)
        {
            if(EquipList[i] == ShopManager.instance.AllShopList.FindIndex(x => x.Name == MyShopList[curNum].Name))
            {
                return true;
            }
        }
        return false;
    }
}
