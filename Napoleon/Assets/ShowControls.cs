using UnityEngine;

public class ShowControls : MonoBehaviour
{
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.sr = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void checkIfShouldBeDeleted()
    {
        if (sr.color.a > 0)
        {
            
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        sr.color = new Color(1, 1, 1, sr.color.a - 0.1f * Time.deltaTime);
        checkIfShouldBeDeleted();
    }
}
