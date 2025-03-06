using PilloPlay.Core;
using UnityEngine;

public class SplatterTransformer : MonoBehaviour
{
    private Pillo pilloRef;
    private bool isConnected => pilloRef != null;
    [SerializeField] private float pilloPressure => isConnected ? pilloRef.pressure : 0f;

    [SerializeField] private float transparencyDecrease;
    [SerializeField] private float growthSpeed;
    [SerializeField] private bool justSpawned = true;
    [SerializeField] private float pressureGrowthMultiplier = 10;

    public void Setup(Pillo p)
    {
        pilloRef = p;
    }

    void Start()
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        PaintSplatterHandler psh = PaintSplatterHandler.Instance;
        
        spr.sprite = psh.Splatters[Random.Range(0, psh.Splatters.Count)];
        spr.color = psh.Colors[Random.Range(0, psh.Colors.Count)];
    }

    void Update()
    {
        if (pilloPressure > 0f && justSpawned && isConnected) 
        {
            float scaleIncrease = growthSpeed * MathAE.RemapFloat(pilloPressure, 0, 1, 0, pressureGrowthMultiplier) * Time.deltaTime;
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(scale.x + scaleIncrease, scale.y + scaleIncrease);
        }

        Color spriteColor = GetComponent<SpriteRenderer>().color;
        Color currentColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, spriteColor.a - transparencyDecrease * Time.deltaTime);
        
        if (pilloPressure <= 0f && isConnected && currentColor.a > 0) 
        {
            justSpawned = false;
            GetComponent<SpriteRenderer>().color = currentColor;
        }

        if (currentColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
