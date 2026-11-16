using UnityEngine;

public class SongClock : MonoBehaviour
{
    [Header("Song Settings")]
    [SerializeField] private float startDelay = 3f;

    private double startTime;

    public double SongTime =>
        Time.timeAsDouble - startTime - startDelay;

    private void Start()
    {
        startTime = Time.timeAsDouble;
    }
}