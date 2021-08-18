using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;
[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _target) => target = _target;
    public List<T> target;
}

[System.Serializable]
public class Shop
{
    public Shop(string license, string name, string exp, string cost, bool ishaving)
    {
        License = license; Name = name; Exp = exp; Cost = cost; IsHaving = ishaving;
    }
    public string License, Name, Exp, Cost;
    public bool IsHaving;
}
public class ShopManager : MonoBehaviour
{
    #region Singleton
    public static ShopManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public TextAsset ShopDatabase; // 샵 데이터베이스 불러올 텍스트

    public List<Shop> AllShopList, CurShopList; // 리스트

    string Allfilepath;

    int curNum;
    enum License
    {
        N2,
        N1,
        Magic,
        Rare,
        Unique,
        Epic
    }

    License license = License.N2;

    public GameObject[] ShopSlot;

    public Sprite Click, UnClick;

    public Image[] TabImg;

    public GameObject CanBuyPanel;

    public Sprite[] AllShopSprite;
    // Start is called before the first frame update
    void Start()
    {
        string[] shopline = ShopDatabase.text.Substring(0, ShopDatabase.text.Length - 1).Split('\n');

        for (int i = 0; i < shopline.Length; i++)
        {
            string[] cell = shopline[i].Split('\t');

            AllShopList.Add(new Shop(cell[0], cell[1], cell[2], cell[3], cell[4] == "TRUE"));
        }

        Allfilepath = Application.persistentDataPath + "/AllShop.txt";

        print(Allfilepath);

        AllLoadFile(); // 내 리스트 저장
    }

    public void AllLoadFile()
    {
        if (!File.Exists(Allfilepath))
        {
            print("no");
            ResetFile();
        }
        string data = File.ReadAllText(Allfilepath); // 파일 위치에있는 텍스트 읽어와 데이터에 저장
        AllShopList = JsonUtility.FromJson<Serialization<Shop>>(data).target; // Json을 변환하여 데이터 클래스로 바꾸고 올리스트에 넣음
        OnClickTab((int)license);
    }

    public void ResetFile()
    {
        string jsondata = JsonUtility.ToJson(new Serialization<Shop>(AllShopList)); // 데이터 클래스를 Json으로 변환

        File.WriteAllText(Allfilepath, jsondata); // Json데이터를 올파일패스에 씀
        AllLoadFile();
    }

    public void SaveFile()
    {
        string jsondata = JsonUtility.ToJson(new Serialization<Shop>(AllShopList)); //Json으로 변환

        File.WriteAllText(Allfilepath, jsondata); // 파일 위치에 씀

        OnClickTab((int)license);
    }
    public void OnClickTab(int tabnum)
    {
        switch (tabnum)
        {
            case 0:
                license = License.N2;
                break;

            case 1:
                license = License.N1;
                break;

            case 2:
                license = License.Magic;
                break;

            case 3:
                license = License.Rare;
                break;

            case 4:
                license = License.Unique;
                break;

            case 5:
                license = License.Epic;
                break;
        }

        //MyCardList = MyCardList.FindAll(x => x.Type == curType);
        //확인을 위해 전체 카드 리스트로 확인
        CurShopList = AllShopList.FindAll(x => x.License == license.ToString());

        for (int i = 0; i < ShopSlot.Length; i++) // 카드 슬롯만큼 돌림
        {
            //슬롯은 이미지, 이름, 설명 순으로 가지고 있음

            ShopSlot[i].SetActive(i < CurShopList.Count); // 내 카드리스트 수보다 적으면 슬롯을 안보이게함
            ShopSlot[i].transform.GetChild(1).GetComponent<Text>().text = i < CurShopList.Count ? CurShopList[i].Name : "";
            ShopSlot[i].transform.GetChild(2).GetComponent<Text>().text = i < CurShopList.Count ? CurShopList[i].Cost : "";
            print(CurShopList.Count);
            if (i < CurShopList.Count)
            {
                ShopSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = AllShopSprite[AllShopList.FindIndex(x => x.Name == CurShopList[i].Name)];
            }
        }
        
        
        for (int i = 0; i < TabImg.Length; i++)
        {
            TabImg[i].sprite = (i == tabnum) ? Click : UnClick;
        }

    }
    public void CheckLicense(int slotnum)
    {
        curNum = slotnum;
        print(curNum);
        CanBuyPanel.SetActive(true);
        //만약 라이센스가 없다면
        if (PlayerPrefs.GetInt("License") < (int)license)
        {
            CanBuyPanel.transform.GetChild(0).GetComponent<Text>().text = "라이센스가 없습니다.";
            CanBuyPanel.transform.GetChild(1).gameObject.SetActive(false);
        }

        //만약 가지고 있다면
        else
        {
            print(CurShopList[curNum].Name);
            if(CurShopList[curNum].IsHaving == true)
            {
                CanBuyPanel.transform.GetChild(0).GetComponent<Text>().text = "이미 가지고 있는 아티팩트입니다.";
            }
            else
            {
                CanBuyPanel.transform.GetChild(0).GetComponent<Text>().text = "구매하시겠습니까?";
                CanBuyPanel.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

    }

    public void PurchaseCheck()
    {
        if (CanBuyPanel.transform.GetChild(0).GetComponent<Text>().text == "가격이 부족합니다." || CanBuyPanel.transform.GetChild(0).GetComponent<Text>().text == "구매가 완료되었습니다." || CanBuyPanel.transform.GetChild(0).GetComponent<Text>().text == "이미 가지고 있는 아티팩트입니다.")
        {
            CanBuyPanel.SetActive(false);
            return;
        }
        //만약 구매가 안된다면
        if (PlayerPrefs.GetInt("Coin") < Int32.Parse(CurShopList[curNum].Cost))
            CanBuyPanel.transform.GetChild(0).GetComponent<Text>().text = "가격이 부족합니다.";

        //만약 구매가 안된다면
        if (PlayerPrefs.GetInt("Coin") >= Int32.Parse(CurShopList[curNum].Cost))
        {
            CanBuyPanel.transform.GetChild(0).GetComponent<Text>().text = "구매가 완료되었습니다.";
            Profile.instance.UseCoin(Int32.Parse(CurShopList[curNum].Cost));
            CurShopList[curNum].IsHaving = true;
            SaveFile();
        }
    }

}
