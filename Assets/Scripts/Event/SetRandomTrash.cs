using UnityEngine;

public class SetRandomTrash : MonoBehaviour
{
    [SerializeField] private int trashNumber = 2;

    [SerializeField] private GameObject[] trashes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int[] trashIdList = new int[trashNumber];
        for (int i = 0; i < trashNumber; )
        {
            int randomTrash = Random.Range(0, trashes.Length);
            bool isIn = false;
            for (int j = 0; j < trashNumber; j++)
            {
                if (randomTrash == trashIdList[j])
                {
                    isIn = true;
                }
                
                
            }

            if (isIn)
            {
                
            }
            else
            {
                trashIdList[i] = randomTrash;
                i++;
            }
        }

        for (int i = 0; i < trashes.Length; i++)
        {
            bool isIn = false;
            for (int j = 0; j < trashNumber; j++)
            {
                if (i == trashIdList[j])
                {
                    isIn = true;
                }
                
                
            }
            if (isIn)
            {
                
            }
            else
            {
                Destroy(trashes[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
