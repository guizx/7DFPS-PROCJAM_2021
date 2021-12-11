using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsView : MonoBehaviour
{
    public CampaignModel campaignModel;
    public GameObject levelScrollPrefab, levelButtonPrefab;
    public Transform levelScrollParent;
    public List<GameObject> levelsScrolls;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < campaignModel.campaigns.Count; i++)
        {
            levelsScrolls.Add(Instantiate(levelScrollPrefab, levelScrollParent));
        }
        for (int i = 0; i < levelsScrolls.Count; i++)
        {
            for (int j = 0; j < campaignModel.campaigns[i].levels.Count; j++)
            {
                Instantiate(levelButtonPrefab, levelsScrolls[i].transform);
                levelButtonPrefab.GetComponent<LevelButton>().data = campaignModel.campaigns[i].levels[j];
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
