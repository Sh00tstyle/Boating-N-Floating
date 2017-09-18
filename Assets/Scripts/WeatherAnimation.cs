using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherAnimation : MonoBehaviour
{

    public GameObject Rain;
    public GameObject Ship;
    public GameObject ThunderLight;
    public GameObject ThunderParticle;
    public float minThunderLenght = 0.02f;
    public float maxThunderLenght = 0.10f;

    private void Update()
    {
        //Putting rain above the ship
        Rain.transform.position = new Vector3(Ship.transform.position.x, Ship.transform.position.y + 20f, Ship.transform.position.z);
    }

    IEnumerator ThunderStrike()
    {
        ThunderLight.gameObject.SetActive(true);
        ThunderParticle.gameObject.SetActive(true);
        float randNoise = Random.Range(minThunderLenght, maxThunderLenght);
        yield return new WaitForSeconds(randNoise);
        ThunderLight.GetComponent<Light>().enabled = false;
        while (ThunderParticle.GetComponent<AudioSource>().isPlaying == true)
        {
            yield return null;
        }
        ThunderParticle.gameObject.SetActive(false);
        ThunderLight.gameObject.SetActive(false);
        ThunderLight.GetComponent<Light>().enabled = true;
    }

}
