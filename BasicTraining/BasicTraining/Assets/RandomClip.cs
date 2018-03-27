using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomClip : MonoBehaviour {
    public List<AudioClip> clips;
    public AudioSource source;

    // Use this for initialization
    void Start () {
        if (clips.Count > 0 && source != null)
        {
            source.clip = clips[Random.Range(0, clips.Count)];
            //Debug.Log(source.clip.name);
            source.Play();
        }   
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
