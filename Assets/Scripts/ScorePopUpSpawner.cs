using TMPro;
using UnityEngine;

public class ScorePopUpSpawner : MonoBehaviour
{
    public static ScorePopUpSpawner instance;
    public GameObject spawner;

    private void Start()
    {
        instance = this;
    }

    public void SpawnPopup(Vector3 position, string text, bool isCritical)
    {
        var popup = Instantiate(spawner, position, Quaternion.identity);
        var textComp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textComp.text = text;

        popup.GetComponent<ScorePopupAnimation>().SetIsCritical(isCritical);

        Destroy(popup, 2f);
    }
}
