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
    public TextAsset ShopDatabase; // 샵 데이터베이스 불러올 텍스트

    public List<Shop> AllShopList, MyShopList, CurShopList; // 리스트

    string Allfilepath;

    public string curType = "노말(N-2)"; // 카드 컬렉션에 존재할 현재 타입

    string[] Type = { "노말(N-2)", "노말(N-1)", "매직", "레어", "유니크", "에픽" }; // 모든 타입

    public GameObject[] ShopSlot;

    public Sprite Click, UnClick;

    public Image[] TabImg;
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
        OnClickTab(curType);
    }

    public void ResetFile()
    {
        string jsondata = JsonUtility.ToJson(new Serialization<Shop>(AllShopList)); // 데이터 클래스를 Json으로 변환
        print(jsondata);
        File.WriteAllText(Allfilepath, jsondata); // Json데이터를 올파일패스에 씀
        AllLoadFile();
    }

    public void SaveFile()
    {
        string jsondata = JsonUtility.ToJson(new Serialization<Shop>(MyShopList)); //내 카드리스트 Json으로 변환

        File.WriteAllText(Allfilepath, jsondata); // 파일 위치에 씀

        OnClickTab(curType);
    }
    public void OnClickTab(string tabname)
    {

        curType = tabname;

        //MyCardList = MyCardList.FindAll(x => x.Type == curType);
        //확인을 위해 전체 카드 리스트로 확인
        CurShopList = AllShopList.FindAll(x => x.License == curType);

        for (int i = 0; i < ShopSlot.Length; i++) // 카드 슬롯만큼 돌림
        {
            //슬롯은 이미지, 이름, 설명 순으로 가지고 있음

            ShopSlot[i].SetActive(i < CurShopList.Count); // 내 카드리스트 수보다 적으면 슬롯을 안보이게함
            ShopSlot[i].transform.GetChild(1).GetComponent<Text>().text = i < CurShopList.Count ? CurShopList[i].Name : "";
            ShopSlot[i].transform.GetChild(2).GetComponent<Text>().text = i < CurShopList.Count ? CurShopList[i].Exp : "";
        }
        int tabnum = 0;
        switch (tabname)
        {
            case "노말(N-2)":
                tabnum = 0;
                break;

            case "노말(N-1)":
                tabnum = 1;
                break;

            case "매직":
                tabnum = 2;
                break;

            case "레어":
                tabnum = 3;
                break;

            case "유니크":
                tabnum = 4;
                break;

            case "에픽":
                tabnum = 5;
                break;
        }

        for (int i = 0; i < TabImg.Length; i++)
        {
            TabImg[i].sprite = (i == tabnum) ? Click : UnClick;
        }

    }
}
