using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject m_IngredientPrefab;
    public IngredientDescriptor[] m_FoodIngredients;

    [Space(10)]
    public ObjectSocket[] m_FoodBubbleSockets;
    public ObjectSocket m_FoodBubbleSpawnPoint;

    private Animator m_CustomerAnimator;
    private void Start()
    {
        m_CustomerAnimator = GetComponentInChildren<Animator>();
    }

    public void ShowFoodOrder()
    {
        for (int i = 0; i < m_FoodIngredients.Length; ++i)
        {
            var IngredientInstance = GameObject.Instantiate(m_IngredientPrefab, m_FoodBubbleSpawnPoint.Socket);
            IngredientInstance.transform.localPosition = Vector3.zero;
            IngredientInstance.transform.localRotation = Quaternion.identity;
            IngredientInstance.GetComponentInChildren<SpriteRenderer>().sprite = m_FoodIngredients[i].m_IngredientSprite;
            m_FoodBubbleSockets[i].Stack(m_FoodBubbleSpawnPoint.RemoveObj());
        }
        m_FoodBubbleSpawnPoint.m_DestroyObjects = true;
    }

    [SerializeField]
    private Timer m_EatingTimer;
    [SerializeField]
    private Timer m_PatienceTimer;
    private ObjectSocket m_FoodSocket;
    public void ReceiveFood(ObjectSocket p_FoodSocket)
    {
        var PlateContents = p_FoodSocket.PeekObj().GetComponent<Plate>();
        if (PlateContents == null)
        {
            return;
        }
        if (PlateContents.IngredientCount != m_FoodIngredients.Length)
        {
            return;
        }
        int FullMask = 0x0;
        int CheckedMask = 0x0;
        for (int i = 0; i < m_FoodIngredients.Length; ++i)
        {
            //BAD APPROACH, BUT DUE TO SMALL SIZE SHOULD WORK FAST
            FullMask |= (1 << i);
            for (int j = 0; j < PlateContents.IngredientCount; ++j)
            {
                if ((CheckedMask & (1 << j)) != 0x0)
                {
                    continue;
                }

                if (m_FoodIngredients[i].m_IngredientName !=
                    PlateContents.m_IngredientSockets[j].PeekObj()
                    .GetComponent<IngredientContainer>().m_IngredientDescription.m_IngredientName)
                {
                    continue;
                }

                CheckedMask |= (1 << j);
                break;
            }
        }

        if ((FullMask ^ CheckedMask) != 0x0)
        {
            return;
        }

        m_PatienceTimer.Stop();
        m_EatingTimer.Play();
        m_FoodSocket = p_FoodSocket;
        m_FoodSocket?.Lock();

        m_CustomerAnimator?.SetBool("Eating", true);
        m_CustomerAnimator?.SetTrigger("StartEat");
    }
    public void FinishFood()
    {
        m_FoodSocket?.Unlock();
        m_CustomerAnimator?.SetBool("Eating", false);

        m_FoodSocket.PeekObj()?.gameObject?.GetComponent<Plate>()?.FoodDone();

        for (int i = 0; i < m_FoodBubbleSockets.Length; ++i)
        {
            m_FoodBubbleSpawnPoint.ForceStack(m_FoodBubbleSockets[i].RemoveObj());
        }
    }
}
