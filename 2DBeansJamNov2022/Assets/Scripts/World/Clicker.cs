using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Clicker : MonoBehaviour
{
    public Vector3 mousePosWorld;
    public Camera  cameraWorld;
    public Vector3 mousePosWorldSpace;
    public Vector2 mousePosWorldSpace2D;
    RaycastHit2D hit;
    
    //Animator
    public Animator transitionAnim;
    public Animator musicAnim;
    public float transitionTime = 0;

    public AudioClip music;

    
    // Start is called before the first frame update
    void Start()
    {
        musicAnim = MusicTransition.instance.GetComponent<Animator>();
        musicAnim?.SetTrigger("FadeIn");
        musicAnim.GetComponent<AudioSource>().clip = music;
        musicAnim.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Test 
            //print("Maustaste is pressed");
            // Mouse Posi
            mousePosWorld = Input.mousePosition;
            //print("Screen Space" + mousePosWorld);
            //ScreenSpace zu WorldSpace
            mousePosWorldSpace = cameraWorld.ScreenToWorldPoint(mousePosWorld);
            //WorldSpace
            //print("World Space" + mousePosWorldSpace);
            mousePosWorldSpace2D = new Vector2(mousePosWorldSpace.x,mousePosWorldSpace.y);

            // Raycast2D
            hit = Physics2D.Raycast(mousePosWorldSpace2D, Vector2.zero);

            if(hit.collider != null)
            {
                //print("Target hit!");

                    if(hit.collider.gameObject.tag == "Hintergrund")
                    {
                        //print("Das ist der Hintergrund!");
                    }

                    else if(hit.collider.gameObject.tag == "DiscoEingang")
                    {
                        //print("Das ist die Disco");
                        
                        LoadDisco();
                        
                    }
                    else if(hit.collider.gameObject.tag == "VipEingang")
                    {
                        //print("Das ist die der Vip Bereich");
                        
                        LoadVip();
                    }
                    else if(hit.collider.gameObject.tag == "ToiletteEingang")
                    {
                        //print("Das ist die Toilette");
                        
                        LoadToilette();
                    }
                    else if(hit.collider.gameObject.tag == "EingangEingang")
                    {
                        //print("Das ist der Eingang");
                        
                        LoadEingang();
                        
                    }
                    else if(hit.collider.gameObject.tag == "Kaputt")
                    {
                        hit.collider.gameObject.SetActive(false);
                    }
            }
            else
            {
                //print("miss");
            }

        }
        
        
    }

    public void LoadDisco()
    {
        StartCoroutine(LoadLevel("DiscoRoom"));
        //print("Load");
    }

    public void LoadToilette()
    {
        StartCoroutine(LoadLevel("ToiletteRoom"));
    }

    public void LoadEingang()
    {
        StartCoroutine(LoadLevel("EingangRoom"));
    }
    public void LoadVip()
    {
        StartCoroutine(LoadLevel("VipRoom"));
    }

    IEnumerator LoadLevel(string levelIndex)
            {
                transitionAnim?.SetTrigger("end");
                musicAnim?.SetTrigger("FadeOut");
                yield return new WaitForSeconds(transitionTime);
                SceneManager.LoadScene(levelIndex);
                
            }

}
