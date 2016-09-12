using UnityEngine;
using System.Collections;

public class LostAndFound : MonoBehaviour {

    //This is the location your "lost and found" objects will appear in. Use an empty game object.
    //If you will have a LOT of objects appearing here, they'll pop inside one another and shoot across the room!
    //To fix that, you can give the "lost and found" transform an animation where it moves through a small area or have it alter its position each time it is used as quick "hack" fixes. :)
    public Transform lostandfound;

    public float heightMax = 2;
    public double heightMin = -1.5;

    Rigidbody rb;

    public AudioSource soundSource;
    public AudioClip lostandfoundSound;

    //these are typically particle effects of some sort, saved as part of a game object that destroys itself over time.
    public GameObject spawningGraphic;
    public GameObject leavingGraphic;


    //Hack fix to prevent objects from playing their sound when they first spawn. Checked on objects already in scene, not checked on spawning objects/prefabs.
    public bool dontPlaySound;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
        soundSource = GetComponent<AudioSource>();


        //assigns the lostandfoundSound
        if(lostandfoundSound != null)
        { 
        soundSource.clip = lostandfoundSound;
        }
        else
        {
            //Warns you if you didn't pick a sound
            Debug.Log("You forgot to assign a sound to thelostandfoundSound");
        }
    }
	
	// Update is called once per frame
	void Update () {

        //checks to see if the object has gone "too high" or "too low" based on the heightMax and heightMin variables.
        //If it is above the heightMax or below the heightMin, it will play its leavingGraphic, play its lostandfoundSound, lose velocity, then appear in the lostandfound position.
        if(gameObject.transform.position.y > heightMax || gameObject.transform.position.y < heightMin)
        {
            soundSource.clip = lostandfoundSound;
            Instantiate(leavingGraphic, gameObject.transform.position, gameObject.transform.rotation);
            soundSource.Play();
            gameObject.transform.position = lostandfound.transform.position;
            rb.velocity = new Vector3(0, 0, 0);
            Instantiate(leavingGraphic, gameObject.transform.position, gameObject.transform.rotation);


        }


        /*   

        //Debug code, can be used to spawn the 
    
        if(Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(leavingGraphic, gameObject.transform.position, gameObject.transform.rotation); 
        }
        */
       
	}

    void OnTriggerEnter(Collider other)
    {

        //Sometimes objects in VR get "out of bounds", especially if you can teleport.
        //Create trigger box colliders around your environment where appropriate and tag them as "boundary".
        //When the object hits the boundary, it will go to the lost and found.
        if(other.gameObject.tag == "boundary")
        {
            soundSource.clip = lostandfoundSound;
            Instantiate(leavingGraphic, gameObject.transform.position, gameObject.transform.rotation);
            gameObject.transform.position = lostandfound.transform.position;
            rb.velocity = new Vector3(0, 0, 0);
            soundSource.Play();
            Instantiate(leavingGraphic, gameObject.transform.position, gameObject.transform.rotation);

        }
    }


    //If you have objects already in the scene being enabled, you may want them to play the sound when they appear!
    //This is really a totally separate feature, but I found this feature was needed on most things that needed the "lost and found" script anyway.
    void OnEnable()
    {

        if(dontPlaySound == false)
        { 
        soundSource.clip = lostandfoundSound;
        Instantiate(leavingGraphic, gameObject.transform.position, gameObject.transform.rotation);
        
        soundSource.Play();
        }
        dontPlaySound = true;
    }

}
