using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManager = null;

    private int _status = 0; //0 初始状态  1 播放中 2 切换中

    private bool _isPause = false;   //是否暂停

    private bool _isSilence = false; //是否静音

    private float _soundVolume = 1f;  //音量

    private float _speed = 0.5f;    //每秒降音速度
    private AudioClip _currentMusic = null;
    private string _currentMusicPath = null;

    private AudioClip _nextMusic = null;

    private string _nextMusicPath = null;

    private AudioSource _audio = null;

    public static AudioManager GetInstance()
    {
        if (_audioManager == null){
            GameObject sound = new GameObject("Sound");
            Object.DontDestroyOnLoad(sound);
            _audioManager = sound.AddComponent<AudioManager>();
        }
        return _audioManager;
    }

    // Start is called before the first frame update
    void Start()
    {
        _audio = gameObject.AddComponent<AudioSource>();
        _audio.loop = true;
        _audio.playOnAwake = true;
        _audio.volume = _soundVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentMusic == null && _nextMusic != null){
            ChangeAudio();
            return;
        } else if (_currentMusic != null && _nextMusic != null && _status == 1){    //开始降音切换音乐
            _status = 2;
        } else if (_status == 2 && _nextMusic != null){
            if (_soundVolume == 0){
                ChangeAudio();
            } else {
                _soundVolume -= _speed * Time.deltaTime;
                _soundVolume = _soundVolume <= 0 ? 0 : _soundVolume;
                _audio.volume = _soundVolume;
            }
        }
    }

    private void ChangeAudio(){
        _currentMusic = _nextMusic;
        _currentMusicPath = _nextMusicPath;
        _nextMusic = null;
        _nextMusicPath = null;
        _audio.Stop();
        _audio.clip = _currentMusic;
        _audio.Play();
        _status = 1;
        _audio.volume = 1f;
    }

    public void PlayNewAudio(string newAudioPath){
        if (_nextMusicPath != null && _nextMusicPath == newAudioPath){
            return;
        }

        if (_nextMusicPath != null && _currentMusicPath == newAudioPath){
            _nextMusic = _currentMusic;
            _nextMusicPath = _currentMusicPath;
            return;
        }
        _nextMusic = Resources.Load<AudioClip>(newAudioPath);
        _nextMusicPath = newAudioPath;
    }

    public void PlayAudioOnce(AudioClip au,Vector2 pos){
        AudioSource.PlayClipAtPoint(au,pos);
    }
}
