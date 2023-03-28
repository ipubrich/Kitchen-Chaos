using UnityEngine;
using System.Collections;

public class LogoAnimatorUI : MonoBehaviour
{
    [SerializeField] private GameObject[] letterObjects;
    [SerializeField] private GameObject[] scaleObjects;
    [SerializeField] private float bounceHeight = 1f;
    [SerializeField] private float bounceSpeed = 1f;
    [SerializeField] private float letterDelayTime = 0.5f;
    [SerializeField] private float scaleInTime = 1f;
    [SerializeField] private float scaleOutTime = 1f;
    [SerializeField] private Vector3 scaleTarget = Vector3.one;

    private Vector3[] letterStartPositions;
    private Vector3[] scaleStartScales;

    private int currentLetterIndex = 0;
    private bool isBouncing = false;

    private void Start()
    {
        // Get initial positions and scales
        letterStartPositions = new Vector3[letterObjects.Length];
        for (int i = 0; i < letterObjects.Length; i++)
        {
            letterStartPositions[i] = letterObjects[i].transform.position;
        }

        scaleStartScales = new Vector3[scaleObjects.Length];
        for (int i = 0; i < scaleObjects.Length; i++)
        {
            scaleStartScales[i] = scaleObjects[i].transform.localScale;
        }

        // Start the bouncing coroutine
        StartCoroutine(BounceLetters());
    }

    private IEnumerator BounceLetters()
    {
        while (true)
        {
            if (!isBouncing)
            {
                // Start bouncing the current letter up
                isBouncing = true;
                iTween.MoveTo(letterObjects[currentLetterIndex], iTween.Hash("position", letterStartPositions[currentLetterIndex] + new Vector3(0, bounceHeight, 0), "time", bounceSpeed / 2, "easeType", iTween.EaseType.easeOutSine));
                yield return new WaitForSeconds(bounceSpeed / 2);

                // Start bouncing the current letter down and return it to the original position
                iTween.MoveTo(letterObjects[currentLetterIndex], iTween.Hash("position", letterStartPositions[currentLetterIndex], "time", bounceSpeed / 2, "easeType", iTween.EaseType.easeInSine));

                // Move to the next letter
                currentLetterIndex++;
                if (currentLetterIndex >= letterObjects.Length)
                {
                    // Start scaling the scale objects
                    for (int i = 0; i < scaleObjects.Length; i++)
                    {
                        iTween.ScaleTo(scaleObjects[i], iTween.Hash("scale", scaleTarget, "time", scaleInTime, "easeType", iTween.EaseType.easeInOutSine));
                    }

                    // Wait for the scaling animation to finish
                    yield return new WaitForSeconds(scaleInTime);

                    // Reverse scale the scale objects
                    for (int i = 0; i < scaleObjects.Length; i++)
                    {
                        iTween.ScaleTo(scaleObjects[i], iTween.Hash("scale", scaleStartScales[i], "time", scaleOutTime, "easeType", iTween.EaseType.easeInOutSine));
                    }

                    // Reset the index and wait for the scale animation to finish
                    currentLetterIndex = 0;
                    yield return new WaitForSeconds(scaleOutTime);
                }

                yield return new WaitForSeconds(letterDelayTime);
                isBouncing = false;
            }
            else
            {
                yield return null;
            }
        }
    }
}
