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

        for (int i = 11; i > 0; i--)
        {
            var rand = Random.Range(0, props.Count -1);
            randProps.Add(props[rand]);
            props.Remove(props[rand]);
        }

        foreach (var prop in randProps)
        {
            prop.SetActive(true);
        }
    }
}
