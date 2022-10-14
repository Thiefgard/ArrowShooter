using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private Gradient _staminaGradient;
    [SerializeField] private Image _staminaBar;
    //[SerializeField] private Gradient _powerGradient;
    [SerializeField] private ClassicProgressBar _powerBar;
    [SerializeField] GameObject _startText;
    [SerializeField] GameObject _restartButton;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
        _staminaBar.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Player.Instance.isShooting)
        {
            _powerBar.gameObject.SetActive(true);
            SetPowerBarValue();
        }
        if (!Player.Instance.isShooting)
        {
            _powerBar.gameObject.SetActive(false);
        }
        SetStaminaBarValue();
    }

    private void SetStaminaBarValue()
    {
        _staminaBar.fillAmount = Player.Instance.GetEnergyBarFill();
        _staminaBar.color = _staminaGradient.Evaluate(_staminaBar.fillAmount);
    }
    private void SetPowerBarValue()
    {
        _powerBar.m_FillAmount = Player.Instance.GetPowerBarFill();
    }

    public void ShowStateUI(GameController.GameState state)
    {
        print("state " + state);
        if (state == GameController.GameState.WaitPhase)
        {
            _startText.SetActive(true);
        }
        if (state == GameController.GameState.actionPhase)
        {
            _staminaBar.gameObject.SetActive(true);
            _startText.SetActive(false);
        }
        if(state == GameController.GameState.EndGame)
        {
            _staminaBar.gameObject.SetActive(false);
            _restartButton.SetActive(true);
        }
        
    }
}
