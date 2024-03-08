using TMPro;
using UnityEngine;

public class ScorePopupAnimation : MonoBehaviour
{
    [SerializeField] AnimationCurve opacityCurve;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve heightCurve;

    private bool isCritical;

    private TextMeshProUGUI textComp;
    private float lifeTime = 0;
    private Vector3 origin;

    private int randomDirectionZ;

    private void Awake()
    {
        textComp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;

        randomDirectionZ = Random.Range(-2, 2);
    }

    private void Update()
    {
        if (isCritical)
        {
            textComp.color = new Color(1, 0.5f, 0, opacityCurve.Evaluate(lifeTime));
            transform.localScale = new Vector3(5,5,5) * scaleCurve.Evaluate(lifeTime);
            transform.localPosition = origin + new Vector3(0, 1.7f + heightCurve.Evaluate(lifeTime), 0);
        }
        else
        {
            textComp.color = new Color(1, 1, 0, opacityCurve.Evaluate(lifeTime));
            transform.localScale = Vector3.one * scaleCurve.Evaluate(lifeTime);
            
            transform.localPosition = origin + new Vector3(0, 0.7f + heightCurve.Evaluate(lifeTime), randomDirectionZ * heightCurve.Evaluate(lifeTime));

        }

        lifeTime += Time.deltaTime;
    }

    public void SetIsCritical(bool isCritical) { this.isCritical = isCritical; }
}
