using UnityEngine;

public class FallingNote : MonoBehaviour
{
    private float startTime;
    private float speed;
    private float hitLineY;

    private SongClock songClock;

    public void Initialize(
        float start,
        float fallSpeed,
        float hitY,
        SongClock clock)
    {
        startTime = start;
        speed = fallSpeed;
        hitLineY = hitY;
        songClock = clock;
    }

    private void Update()
    {
        if (songClock == null)
            return;

        float y = hitLineY
            + (startTime - (float)songClock.SongTime) * speed
            + transform.localScale.y / 2f;

        transform.position = new Vector3(
            transform.position.x,
            y,
            transform.position.z
        );
    }
}