using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Code
{
    public class StartCutscene : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;
        [SerializeField] AudioClip _elevatorRunning;
        [SerializeField] private AudioClip _elevatorCrash;
        [SerializeField] private AudioClip _elevatorTTS;
        [SerializeField] private Elevator _elevator;
        [SerializeField] CinemachineImpulseSource _impulseSource;
        
        private void Start()
        {
            StartCoroutine(ElevatorCutscene());
        }
        
        IEnumerator ElevatorCutscene()
        {
            _audioSource.clip = _elevatorRunning;
            _audioSource.loop = true;
            _audioSource.Play();
            yield return new WaitForSeconds(30f);
            _audioSource.loop = false;
            _audioSource.clip = _elevatorCrash;
            _audioSource.Play();
            _impulseSource.GenerateImpulse();
            yield return new WaitForSeconds(7f);
            _elevator.OpenElevator();
            _audioSource.clip = _elevatorTTS;
            _audioSource.Play();
            Timer.timerActive = true;
        }
    }
}