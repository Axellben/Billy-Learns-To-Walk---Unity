using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour {
    public GameObject agentPrefab;
    List<GameObject> agents = new List<GameObject> ();
    List<GameObject> newAgents = new List<GameObject> ();
    public int N = 10;

    private int Generation = 1;

    // Start is called before the first frame update
    void Start () {
        InvokeRepeating ("NextGeneration", 0, 30);
    }

    // IEnumerator Wait (float duration) {
    //     yield return new WaitForSeconds (duration); //Wait
    // }

    // Update is called once per frame
    void Update () {

    }

    private int pickIndex () {
        int index = 0;
        float r = Random.Range (0, 1f);
        while (r > 0) {
            r = r - agents[index].transform.GetComponent<Agent> ().fitness;
            index++;
        }
        if (index != 0)
            index--;
        return index;
    }

    private void CalculateFitness () {
        float sumX = 0;
        float sumY = 0;
        foreach (GameObject obj in agents) {
            sumX += obj.transform.position.x;
            sumY += obj.transform.position.y;
        }

        foreach (GameObject obj in agents) {
            obj.transform.GetComponent<Agent> ().fitness = obj.transform.position.x / sumX;
        }
    }

    private GameObject PickOne () {
        // var random = new Random ();
        // int index = Random.Range (0, agents.Count);
        int index = pickIndex ();
        // print (index);
        GameObject parent = agents[index];
        GameObject child = Instantiate (agentPrefab, transform.position, Quaternion.identity);
        child.GetComponent<Agent> ().nn.Copy (parent.GetComponent<Agent> ().nn);
        child.GetComponent<Agent> ().nn.Mutate (0.1f);
        return child;
    }

    private void NextGeneration () {
        if (agents.Count != 0) {
            CalculateFitness ();
            for (int i = 0; i < N; ++i) {
                newAgents.Add (PickOne ());
            }

            foreach (GameObject obj in agents) {
                Destroy (obj);
            }
            agents.Clear ();

            foreach (GameObject obj in newAgents) {
                agents.Add (obj);
            }
            newAgents.Clear ();
        } else {
            for (int i = 0; i < N; ++i) {
                agents.Add (Instantiate (agentPrefab, transform.position, Quaternion.identity));
            }

        }
        print ("Generation :" + Generation);
        Generation++;

    }
}