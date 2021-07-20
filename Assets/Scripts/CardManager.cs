using System.Collections.Generic;
using UnityEngine;
public class CardManager : MonoBehaviour
{
    #region INSTANCE
    public static CardManager instance;
    #endregion

    private int maxCards = 48;

    public Transform parentDeck; // Instantiate �� ���(���� ���� ���� ��)

    private List<GameObject> cardPrefabs; // ī���� ������ ȭ
    private List<GameObject> cardDeck; // �ʵ忡 ���� ī�� �� ����Ʈ ����

    public List<GameObject> myHand; // �÷��̾� �ڽ��� �� ����Ʈ
    public List<GameObject> opponentHand; // ����� �� ����Ʈ
    public List<GameObject> field; // �ʵ� ����Ʈ

    public List<GameObject> myHandScore; // �� ���� ����Ʈ
    public List<GameObject> opponentHandScore; // ��� ���� ����Ʈ

    public List<Vector3> myHandPosition; // �ڽ� ���� ��ġ
    public List<Vector3> opponentHandPosition; // ��� ���� ��ġ
    public Vector3[] fieldPosition; // ī�带 �������� �ʵ� ��ġ

    public Vector3 scoreKingPosition; // 5��
    public Vector3 scoreAnimalPosition; // 9�� �۵�
    public Vector3 scoreFlagPosition;
    public Vector3 scoreSoldierPosition;

    public GameObject ChociePanel;//�ΰ� �� �ϳ� �� �� �ǳ�
    int choiceNum; // �� ī��

    private void Awake()
    {
        instance = this;
        cardPrefabs = new List<GameObject>(); // ������ ����Ʈ �Ҵ�
        cardDeck = new List<GameObject>(); // �� ����Ʈ �Ҵ�

        myHand = new List<GameObject>(); // �� ����Ʈ �Ҵ�
        opponentHand = new List<GameObject>(); // �� ����Ʈ �Ҵ�
        field = new List<GameObject>(); // �ʵ� ����Ʈ �Ҵ�

        myHandScore = new List<GameObject>(); // ���� ����Ʈ �Ҵ�
        opponentHandScore = new List<GameObject>(); // ���� ����Ʈ �Ҵ�
    }

    private void Start()
    {
        //�� ī���� ��ġ ����
        for(int i = 0; i < 8; i++)
        {
            myHandPosition.Add(new Vector3(i - 4, -4.3f, 0));
            opponentHandPosition.Add(new Vector3(i - 4, 4.3f, 0));
        }

        fieldPosition = new[] { new Vector3(-2,2,0),
                                new Vector3(2,2,0),
                                new Vector3(-3,0,0),
                                new Vector3(3,0,0),
                                new Vector3(-2,-2,0),
                                new Vector3(2,-2,0) };

        scoreKingPosition = new Vector3(-8f, -3f, 0f); // 5��
        scoreAnimalPosition = new Vector3(-4f, -3f, 0f); // 9�� �۵�
        scoreFlagPosition = new Vector3(-4f, -1.5f, 0f);
        scoreSoldierPosition = new Vector3(2.5f, -3f, 0f);

        PrefabToCard(); // ������ ������ �����ϴ� ī�带 ����Ʈ�� ��� �����غ� �Ѵ�.
        CreateDeck(); // �÷��̾ �غ��� ī�� 12��, ���� �غ��� ī�� 12�� �� ���� �� 48���� ī�带 ���� �ִ´�.
        ShuffleDeck(); //���� ���´�
        DrawCard(myHand, 6); // ���տ� 6�� �� �̴´�.
        DrawCard(opponentHand, 6);  // ���տ� 6�� �� �̴´�.
        DrawCard(field, 6);
    }

    public void ShuffleDeck()
    {
        int a, b;
        GameObject temp;
        
        for (int i = 0; i < (maxCards / 2); i++)
        {
            a = Random.Range(0, maxCards / 2);
            b = Random.Range(maxCards / 2, maxCards);
            temp = cardDeck[a];
            cardDeck[a] = cardDeck[b];
            cardDeck[b] = temp;
        }
    }

    public void CreateDeck()
    {
        for (int i = 0; i < maxCards; i++) // ȭ���� ����
        {
            cardDeck.Add(Instantiate(cardPrefabs[i], parentDeck));
        }
    }

    public void PrefabToCard()
    {
        for (int i = 0; i < maxCards; i++)
        {
            cardPrefabs.Add(Resources.Load<GameObject>("Prefabs/" + i));
        }
    }

    public void DrawCard(List<GameObject> cardList, int drawAmount)
    {
        if (drawAmount > cardDeck.Count)
        {
            drawAmount = cardDeck.Count;
        }

        for (int i = 0; i < drawAmount; i++)
        {
            cardList.Add(cardDeck[i]);
            SetCardPosition(cardList, cardList.Count - 1);
            cardDeck.RemoveAt(i);
        }
    }

    public void SetCardPosition(List<GameObject> cardList, int index)
    {
        Debug.Log(index);
        if(cardList == myHand)
        {
            cardDeck[index].transform.position = myHandPosition[index];
            cardDeck[index].transform.SetParent(GameObject.Find("MyHand").transform);
        }
        else if(cardList == opponentHand)
        {
            cardDeck[index].transform.position = opponentHandPosition[index];
            cardDeck[index].transform.SetParent(GameObject.Find("OpponentHand").transform);

        }
        else if(cardList == field)
        {
            cardDeck[index].transform.position = fieldPosition[index];
            cardDeck[index].transform.SetParent(GameObject.Find("Field").transform);
        }
    }

    public void SelectCardInHand()
    {

    }
    
    public void SetPosition(List<GameObject> list, GameObject obj)
    {
        obj.transform.position = new Vector3(list[list.Count].transform.position.x + 0.5f, list[list.Count].transform.position.y, list[list.Count].transform.position.z);
    }

    public void ChoicePanel()
    {
        ChociePanel.SetActive(true);
    }

    public void SelectTwoCard(int num)
    {
        choiceNum = num;
        ChociePanel.SetActive(false);
    }

    public Vector3 SetNextPosition(Vector3 position)
    {
        position = new Vector3(position.x + 0.5f, position.y, position.z);
        return position;
    }

}
