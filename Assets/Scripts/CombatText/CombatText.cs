using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private Text text;

    [SerializeField] private float lifeTime;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    // Animates combat text
    private void Move()
    {
        transform.Translate(Vector2.up * (speed * Time.deltaTime));
    }

    // Hides (fade effect) the combat text 
    private IEnumerator FadeOut()
    {
        var startAlpha = text.color.a;

        var rate = 1.0f / lifeTime;

        var progress = 0.0f;

        while (progress < 1.0)
        {
            Color temp = text.color;

            temp.a = Mathf.Lerp(startAlpha, 0, progress);

            text.color = temp;

            progress += rate * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}
