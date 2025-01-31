using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UnitySTG.THSTG.Stage
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private PauseController _pauseController;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Button _unpauseButton;

        private void Start()
        {
            _unpauseButton.onClick.AddListener(() =>
            {
                _pauseController.IsPause = false;
            });
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !_pauseController.IsPause)
            {
                _pauseController.IsPause = true;
            }
        }

        public void TogglePauseMenu(bool isPause)
        {
            _pauseMenu.gameObject.SetActive(isPause);
        }
    }
}
