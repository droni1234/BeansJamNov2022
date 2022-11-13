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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {   
            // Test 
            print("Maustaste is pressed");
            // Mouse Posi
            mousePosWorld = Input.mousePosition;
            print("Screen Space" + mousePosWorld);
            //ScreenSpace zu WorldSpace
            mousePosWorldSpace = cameraWorld.ScreenToWorldPoint(mousePosWorld);
            //WorldSpace
            print("World Space" + mousePosWorldSpace);
            mousePosWorldSpace2D = new Vector2(mousePosWorldSpace.x,mousePosWorldSpace.y);

            // Raycast2D
            hit = Physics2D.Raycast(mousePosWorldSpace2D, Vector2.zero);

            if(hit.collider != null)
            {
                print("Target hit!");

                    if(hit.collider.gameObject.tag == "Hintergrund")
                    {
                        print("Das ist der Hintergrund!");
                    }

                    else if(hit.collider.gameObject.tag == "DiscoEingang")
                    {
                        print("Das ist die Disco");
                        SceneManager.LoadScene("DiscoRoom");
                    }
                    else if(hit.collider.gameObject.tag == "VipEingang")
                    {
                        print("Das ist die der Vip Bereich");
                        SceneManager.LoadScene("VipRoom");
                    }
                    else if(hit.collider.gameObject.tag == "ToiletteEingang")
                    {
                        print("Das ist die Toilette");
                        SceneManager.LoadScene("ToiletteRoom");
                    }
                    else if(hit.collider.gameObject.tag == "EingangEingang")
                    {
                        print("Das ist der Eingang");
                        SceneManager.LoadScene("EingangRoom");
                    }
            }
            else
            {
                print("miss");
            }
        }
    }
}
