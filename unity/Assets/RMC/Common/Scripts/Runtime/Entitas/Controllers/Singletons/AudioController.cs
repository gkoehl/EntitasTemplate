using RMC.Common.Singleton;
using Entitas;
using UnityEngine;
using System.Collections.Generic;


namespace RMC.Common.Entitas.Controllers.Singleton
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
    public class AudioController : SingletonMonobehavior<AudioController> 
	{
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties
        private Group _soundGroup;
        private AudioSource _audioSource;
        private Dictionary<string, AudioClip> _audioClipDictionary;

        // ------------------ Methods

        // ------------------ Non-serialized fields

        // ------------------ Methods

        protected void Start()
        {
            //NOTE: One AudioSource = Limitation of one sound playing concurrently. Ok for demo
            _audioSource = gameObject.AddComponent<AudioSource>();

            //
            _audioClipDictionary = new Dictionary<string, AudioClip>();
            _soundGroup = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Audio));
            _soundGroup.OnEntityAdded += OnSoundEntityAdded;

        }

        protected void OnDestroy()
        {
            _soundGroup.OnEntityAdded -= OnSoundEntityAdded;
        }

        private void OnSoundEntityAdded (Group group, Entity entity, int index, IComponent component) 
        {
            PlaySound(entity.audio.audioClipName, entity.audio.volume);
        }

        private void PlaySound (string audioClipName, float volume)
        {
            AudioClip audioClip = FetchAudioClip(audioClipName);
            _audioSource.PlayOneShot(audioClip, volume);

            //Keep
            //Debug.Log ("PlaySound() " + audioClip.name);
        }


        //cache in RAM, audio clips after first use.
        private AudioClip FetchAudioClip (string audioClipName)
        {
            if (!_audioClipDictionary.ContainsKey(audioClipName))
            {
                AudioClip audioClip = Resources.Load<AudioClip>(audioClipName);
                _audioClipDictionary.Add(audioClipName, audioClip);
            }

            return _audioClipDictionary[audioClipName]; 
        }

    }


}
