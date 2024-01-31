using Audio.Controller;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Utils.Constants;

namespace Audio.Service
{
    public class AudioService : MonoBehaviour
    {
        private Transform _audioContainer;
        private AudioSourceController _audioSourceController;

        private void Start()
        {
            _audioContainer = GameObject.Find(GameConstants.AUDIO_CONTAINER).transform;
            CreateAudioSourceController();
        }

        public async UniTask SoundTheWord(string text)
        {
            AudioClip audioClip = await GetAudio(text);
            _audioSourceController.PlayOneShot(audioClip);
        }

        private void CreateAudioSourceController()
        {
            GameObject audioSourceObject = Resources.Load<GameObject>(GameConstants.SOUND_SOURCE);
            GameObject audioSourceInstance = Instantiate(audioSourceObject, _audioContainer, false);
            _audioSourceController = audioSourceInstance.AddComponent<AudioSourceController>();
        }
        
        private async UniTask<AudioClip> GetAudio(string text)
        {
            string endedUrl;
            if (SystemInfo.deviceType == DeviceType.Desktop) {
                endedUrl = GameConstants.URL_GOOGLE_TRANSLATE_PC + text + "&tl=" + "en";
            }
            else {
                endedUrl = GameConstants.URL_GOOGLE_TRANSLATE_ANDROID + text + "&tl=" + "en";
            }
            using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(endedUrl, AudioType.MPEG))
            {
                await webRequest.SendWebRequest().ToUniTask();
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    return DownloadHandlerAudioClip.GetContent(webRequest);
                }
            }

            Debug.LogError($"Dont load audioClip. Text={text}. Url={endedUrl}");
            return null;
        }
    }
}