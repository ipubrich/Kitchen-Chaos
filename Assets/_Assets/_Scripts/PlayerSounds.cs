using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footStepTimer;
    private float footStepTimerMax = .25f;
    void Awake()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        footStepTimer -= Time.deltaTime;
        if (footStepTimer < 0f)
        {
            footStepTimer = footStepTimerMax;
            if (player.IsWalking())
            {
                // audio
                float volume = 1f;
                SoundManager.Instance.PlayFootStepsSound(player.transform.position, volume);
            }
            
        }
    }
}
