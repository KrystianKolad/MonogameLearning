using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using MonogameLearning.Engine.States;

namespace MonogameLearning.Engine.Sound
{
    public class SoundManager
    {
        private int _soundtrackIndex = -1;
        private List<SoundEffectInstance> _soundtracks = new List<SoundEffectInstance>();
        private Dictionary<Type, SoundBankItem> _soundBank = new Dictionary<Type, SoundBankItem>();

        public void SetSoundtrack(List<SoundEffectInstance> tracks)
        {
            _soundtracks = tracks;
            _soundtrackIndex = _soundtracks.Count - 1;
        }

        public void RegisterSound(BaseGameStateEvent gameEvent, SoundEffect sound)
        {
            RegisterSound(gameEvent, sound, 1.0f, 0.0f, 0.0f);
        }

        public void RegisterSound(BaseGameStateEvent gameEvent, SoundEffect sound, float volume, float pitch, float pan)
        {
            _soundBank.Add(gameEvent.GetType(), new SoundBankItem(sound, new SoundAttributes(volume, pitch, pan)));
        }

        public void PlaySoundtrack()
        {
            var nbTracks = _soundtracks.Count;
            if (nbTracks <= 0 )
            {
                return;
            }

            var currentTrack = _soundtracks[_soundtrackIndex];
            var nextTrack = _soundtracks[(_soundtrackIndex +1) % nbTracks];
            if (currentTrack.State.Equals(SoundState.Stopped))
            {
                nextTrack.Play();
                _soundtrackIndex++;
                if (_soundtrackIndex >= _soundtracks.Count)
                {
                    _soundtrackIndex = 0;
                }
            }
        }

        public void OnNotify(BaseGameStateEvent gameEvent)
        {
            if (_soundBank.TryGetValue(gameEvent.GetType(), out var soundBankItem))
            {
                soundBankItem.Sound.Play();
            }
        }
    }
}