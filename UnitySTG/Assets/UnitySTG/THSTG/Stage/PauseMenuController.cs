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
        [SerializeField] private PauseController pauseController;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private Button unpauseButton;

        private void Start()
        {
            unpauseButton.onClick.AddListener(() =>
            {
                pauseController.IsPause = false;
            });
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !pauseController.IsPause)
            {
                pauseController.IsPause = true;
            }
        }

        public void TogglePauseMenu(bool isPause)
        {
            pauseMenu.gameObject.SetActive(isPause);
        }
    }
}
