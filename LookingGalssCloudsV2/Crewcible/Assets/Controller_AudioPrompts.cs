using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_AudioPrompts : MonoBehaviour
{
    Model_Game gameModel;
    Model_Play playModel;

    AudioSource left;
    AudioSource right;
    AudioSource both;

    void Start()
    {
        gameModel = ServiceLocator.instance.Model.GetComponent<Model_Game>();
        playModel = ServiceLocator.instance.Model.GetComponent<Model_Play>();

        GameObject g = new GameObject("AudioChild");
        left = g.AddComponent<AudioSource>();
        right = g.AddComponent<AudioSource>();
        both = g.AddComponent<AudioSource>();

        left.panStereo = -.65f;
        right.panStereo = .65f;

        left.loop = right.loop = both.loop = false;
        left.playOnAwake = right.playOnAwake = both.playOnAwake = false;

        SCG_EventManager.instance.Register<Event_Audio>(EventHandler);
    }

    int LD_PortNum;
    int LD_StbdNum;
    int CriticalNum;
    int GunNum;
    int LanceNum;
    int RocketsNum;
    int ShieldNum;
    int ThrustersNum;


    public void EventHandler (SCG_Event e)
    {
        Event_Audio a = e as Event_Audio;

        if (a != null)
        {
            if (a.type == AudioEvent.LD_Port)
            {
                if (LD_PortNum == 0)
                {
                    left.PlayOneShot(gameModel.LD_Port[0]);
                    LD_PortNum = 1;
                }
                else
                {
                    left.PlayOneShot(gameModel.LD_Port[1]);
                    LD_PortNum = 0;
                }
            }
            else if (a.type == AudioEvent.LD_Stbd)
            {
                if (LD_StbdNum == 0)
                {
                    right.PlayOneShot(gameModel.LD_Stbd[0]);
                    LD_StbdNum = 1;
                }
                else
                {
                    right.PlayOneShot(gameModel.LD_Stbd[1]);
                    LD_StbdNum = 0;
                }
            }
            else if (a.type == AudioEvent.Contact)
            {
                both.PlayOneShot(gameModel.Contact_General);
            }
            else if (a.type == AudioEvent.CriticalError)
            {
                int num = Random.Range(0, gameModel.CriticalError.Length);

                while (num == CriticalNum)
                {
                    num = Random.Range(0, gameModel.CriticalError.Length);
                }

                CriticalNum = num;
                both.PlayOneShot(gameModel.CriticalError[CriticalNum]);
            }
            else if (a.type == AudioEvent.Danger)
            {
                both.PlayOneShot(gameModel.Danger_JC);
            }
            else if (a.type == AudioEvent.O_Guns)
            {
                int num = Random.Range(0, gameModel.Online_Guns.Length);

                while (num == GunNum)
                {
                    num = Random.Range(0, gameModel.Online_Guns.Length);
                }

                GunNum = num;
                if (playModel.leftStation == Stations.Guns)
                {
                    if (left.isPlaying)
                        left.Stop();
                    left.clip=(gameModel.Online_Guns[GunNum]);
                    left.Play();
                }
                else
                {
                    if (right.isPlaying)
                        right.Stop();
                    right.clip = (gameModel.Online_Guns[GunNum]);
                    right.Play();
                }
            }
            else if (a.type == AudioEvent.O_Lance)
            {
                int num = Random.Range(0, gameModel.Online_Lance.Length);

                while (num == LanceNum)
                {
                    num = Random.Range(0, gameModel.Online_Lance.Length);
                }

                LanceNum = num;
                if (playModel.leftStation == Stations.Lance)
                {
                    if (left.isPlaying)
                        left.Stop();
                    left.clip = (gameModel.Online_Lance[LanceNum]);
                    left.Play();
                }
                else
                {
                    if (right.isPlaying)
                        right.Stop();
                    right.clip = (gameModel.Online_Lance[LanceNum]);
                    right.Play();
                }
            }
            else if (a.type == AudioEvent.O_Rockets)
            {
                int num = Random.Range(0, gameModel.Online_Rockets.Length);

                while (num == RocketsNum)
                {
                    num = Random.Range(0, gameModel.Online_Rockets.Length);
                }

                RocketsNum = num;
                if (playModel.leftStation == Stations.Rockets)
                {
                    if (left.isPlaying)
                        left.Stop();
                    left.clip = (gameModel.Online_Rockets[RocketsNum]);
                    left.Play();
                }
                else
                {
                    if (right.isPlaying)
                        right.Stop();
                    right.clip = (gameModel.Online_Rockets[RocketsNum]);
                    right.Play();
                }
            }
            else if (a.type == AudioEvent.O_Shield)
            {
                int num = Random.Range(0, gameModel.Online_Shields.Length);

                while (num == ShieldNum)
                {
                    num = Random.Range(0, gameModel.Online_Shields.Length);
                }

                ShieldNum = num;
                if (playModel.leftStation == Stations.Shield)
                {
                    if (left.isPlaying)
                        left.Stop();
                    left.clip = (gameModel.Online_Shields[ShieldNum]);
                    left.Play();
                }
                else
                {
                    if (right.isPlaying)
                        right.Stop();
                    right.clip = (gameModel.Online_Shields[ShieldNum]);
                    right.Play();
                }

            }
            else if (a.type == AudioEvent.O_Thrusters)
            {
                int num = Random.Range(0, gameModel.Online_Thrusters.Length);

                while (num == ThrustersNum)
                {
                    num = Random.Range(0, gameModel.Online_Thrusters.Length);
                }

                ThrustersNum = num;
                if (playModel.leftStation == Stations.Thrusters)
                {
                    if (left.isPlaying)
                        left.Stop();
                    left.clip = (gameModel.Online_Thrusters[ThrustersNum]);
                    left.Play();
                }
                else
                {
                    if (right.isPlaying)
                        right.Stop();
                    right.clip = (gameModel.Online_Thrusters[ThrustersNum]);
                    right.Play();
                }
            }
            else if (a.type == AudioEvent.Welcome)
            {
                both.PlayOneShot(gameModel.Welcome);
            }
            else if (a.type == AudioEvent.W_Guns)
            {
                if (playModel.leftStation == Stations.Guns)
                    left.PlayOneShot(gameModel.Warning_Guns);
                else
                    right.PlayOneShot(gameModel.Warning_Guns);
            }
            else if (a.type == AudioEvent.W_Lance)
            {
                if (playModel.leftStation == Stations.Lance)
                    left.PlayOneShot(gameModel.Warning_Lance);
                else
                    right.PlayOneShot(gameModel.Warning_Lance);
            }
            else if (a.type == AudioEvent.W_Rockets)
            {
                if (playModel.leftStation == Stations.Rockets)
                    left.PlayOneShot(gameModel.Warning_Rockets);
                else
                    right.PlayOneShot(gameModel.Warning_Rockets);
            }
            else if (a.type == AudioEvent.W_Shield)
            {
                if (playModel.leftStation == Stations.Shield)
                    left.PlayOneShot(gameModel.Warning_Shields);
                else
                    right.PlayOneShot(gameModel.Warning_Shields);
            }
            else if (a.type == AudioEvent.W_Thrusters)
            {
                if (playModel.leftStation == Stations.Thrusters)
                    left.PlayOneShot(gameModel.Warning_Thrusters);
                else
                    right.PlayOneShot(gameModel.Warning_Thrusters);
            }
            else if (a.type == AudioEvent.W_System)
            {
                both.PlayOneShot(gameModel.Warning_System);
            }
        }
    }
}
//Need success for level completion
//Need reboot sounds
//Need game over

public enum AudioEvent { Welcome, W_Guns, W_Lance, W_Rockets, W_Shield, W_Thrusters, W_System, O_Guns, O_Lance, O_Rockets, O_Shield, O_Thrusters, LD_Port, LD_Stbd, Danger, CriticalError, Contact }
