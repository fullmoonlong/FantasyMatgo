using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LicenseScript : MonoBehaviour
{
    public Sprite selectSprite, basicSprite;

    int m_license;
    int selectNum;

    public Image[] LicenseImage;
    // Start is called before the first frame update
    void Start()
    {
        m_license = 0;
        selectNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMyLicense();
    }

    private void CheckMyLicense()
    {
        m_license = PlayerPrefs.GetInt("License");
        for(int i=0;i<6;i++)
        {
            LicenseImage[i].GetComponent<Image>().sprite = (i == m_license) ? selectSprite : basicSprite;
        }
       
    }

    public void ClickLicense(int num)
    {
        selectNum = num;
       
        for(int i=0;i<6;i++)
        {
            LicenseImage[i].transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }
        if(m_license < selectNum)
        {
            LicenseImage[selectNum].transform.GetChild(1).GetChild(0).gameObject.SetActive(true); // 구매 버튼 setactive true;
            LicenseImage[selectNum].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = "구매";
        }   
        //라이센스 버튼을 클릭했을 때
        if (selectNum == m_license) //내가 가지고 있는 라이센스라면
        {
            if(selectNum > 0)
            {
                LicenseImage[selectNum].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                LicenseImage[selectNum].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = "판매";
            }
        }
    }

    public void ClickPurchase()
    {
        if(LicenseImage[selectNum].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text == "구매")
        {
            print(PlayerPrefs.GetInt("Coin"));
         
            if (Int32.Parse(LicenseImage[selectNum].transform.GetChild(2).GetComponent<Text>().text) <= PlayerPrefs.GetInt("Coin")) // 내가 가지고 있는 코인보다 라이센스값이 작으면
            {
                Profile.instance.UseCoin(Int32.Parse(LicenseImage[selectNum].transform.GetChild(2).GetComponent<Text>().text));

                PlayerPrefs.SetInt("License", selectNum);
                m_license = PlayerPrefs.GetInt("License");
                print("구매완료");
                print(m_license);
            }
        }
     
        if(LicenseImage[selectNum].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text == "판매")
        {
            Profile.instance.AddCoin(Int32.Parse(LicenseImage[selectNum].transform.GetChild(2).GetComponent<Text>().text) / 2);

            PlayerPrefs.SetInt("License", selectNum-1);
            m_license = PlayerPrefs.GetInt("License");
        }
    }
}
