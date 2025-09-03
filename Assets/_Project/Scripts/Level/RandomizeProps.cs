using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RandomizeProps : MonoBehaviour
{
    public List<GameObject> props, randProps;
    // Start is called before thefirst frame update
    void Start()
    {
        //var rnd = new System.Random();
        //props.OrderBy(item => rnd.Next());

        /*for (int i = 0; i < 10; i++)
        {
            props[i].SetActive(true);
        }*/
        randProps.Clear();

        int count = Mathf.Min(11, props.Count);

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, props.Count);
            randProps.Add(props[rand]);
            props.RemoveAt(rand);
        }

        foreach (var prop in randProps)
        {
            prop.SetActive(true);
        }
    }
}
