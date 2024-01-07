using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private float _loadTimer;
    [SerializeField] private float _minLoadTime;
    [SerializeField] private float _fadeInTime;
    [SerializeField] private float _fadeOutTime;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _camera;

    private float _fadeTimer;
    private bool _fadeIn = true;
    private bool _fadeOut = false;
    private bool _unloadingScene = true;
    private bool _jobsDone = false;
    private AsyncOperation _unloadingOp;
    private AsyncOperation _loadingOp;

    private void Start()
    {
        Time.timeScale = 1f;
        _camera = FindAnyObjectByType<Camera>().gameObject;
        _camera.SetActive(false);
    }
    void Update()
    {
        
        _loadTimer += Time.deltaTime;
        _fadeTimer += Time.deltaTime;
        if (_fadeIn)
        {
            if (_loadTimer < _minLoadTime)
            {
                if (_loadTimer < _fadeInTime)
                {
                    _canvasGroup.alpha = Mathf.Lerp(0, 1, _loadTimer / _fadeInTime);
                }
                else
                {
                    _canvasGroup.alpha = 1;
                    if (_unloadingScene)
                    {
                        _unloadingOp = SceneManager.UnloadSceneAsync(LoadingData.SceneToBeUnloaded, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                        _unloadingScene = false;
                    }

                    if (_unloadingOp.isDone)
                    {
                        _loadingOp = SceneManager.LoadSceneAsync(LoadingData.SceneToBeLoaded, LoadSceneMode.Additive);

                        _loadingOp.allowSceneActivation = false;
                        _fadeIn = false;
                    }
                }
            }
        }
        else
        {
            if ((_loadTimer > _minLoadTime) && (Mathf.Approximately(_loadingOp.progress, .9f)))
            {
                _loadingOp.allowSceneActivation = true;
            }

            if (!_fadeOut && _loadingOp.isDone)
            {
                _fadeOut = true;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(LoadingData.SceneToBeLoaded));
                _fadeTimer = 0f;
            }

            if (_fadeOut && (_fadeTimer < _fadeOutTime))
            {
                _canvasGroup.alpha = Mathf.Lerp(1, 0, _fadeTimer / _fadeOutTime);
            }
            else if (_fadeOut && !_jobsDone && (_fadeTimer >= _fadeOutTime))
            {
                _canvasGroup.alpha = 0;
                SceneManager.UnloadSceneAsync(LoadingData.LoadingScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                _jobsDone = true;
            }
        }
        if( _loadingOp != null && _slider != null )
        {
            _slider.value = _loadingOp.progress / 0.9f;
        }
    }
}
