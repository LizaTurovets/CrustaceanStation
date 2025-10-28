using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DisplayScrpt : MonoBehaviour
{
    private float speed = 16.0f;
    private Vector3 offPos = new Vector3(0, -6.0f, -2);
    private Vector3 onPos = new Vector3(0, 1.0f, -2);
    private bool moving = false;
    // pause the game when displayed == true
    [SerializeField] private bool displayed;
    
    void Start()
    {
        displayed = false;
        transform.position = offPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!displayed)
            {
                moving = true;
            } 
            
            if (displayed)               
            {
                moving = true;

            }
        }

        if (moving) 
        {
            // bring the screen down
            if (displayed)
            {
                var step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, offPos, step);
                if (transform.position.y <= offPos.y || Mathf.Abs(transform.position.y - offPos.y) < 0.0001f)
                {
                    moving = false;
                    displayed = false;
                }
            }

            // bring the screen up
            else if (!displayed)
            {
                var step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, onPos, step);
                if (transform.position.y >= onPos.y || Mathf.Abs(transform.position.y - onPos.y) < 0.0001f)
                {
                    moving = false;
                    displayed = true;
                }
            }
        }
    }

    public void DisplayOff()
    {
        if (displayed) moving = true;
    }


    public void Back2menu() 
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
