using UnityEngine;
using Entitas;
using UnityEngine.UI;
using System;

namespace RMC.EntitasTemplate.Entitas.Controllers
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class CanvasController : MonoBehaviour
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
				private Group _gameScore;

		// ------------------ Methods


        public Text _scoreText;
        public Button _restartButton;
		public Button _pauseButton;

		// ------------------ Non-serialized fields
		private Group _scoreGroup;
		// ------------------ Methods

        protected void Start()
        {
            _scoreGroup = Pools.pool.GetGroup(Matcher.AllOf (Matcher.Game, Matcher.Score));

            //listen
            _scoreGroup.GetSingleEntity().OnComponentReplaced += OnComponentReplaced;

            //set first value
			var scoreComponent = _scoreGroup.GetSingleEntity().score;
			SetScore (scoreComponent.whiteScore, scoreComponent.blackScore);

			_restartButton.onClick.AddListener (OnRestartButtonClicked);
			_pauseButton.onClick.AddListener (OnPauseButtonClicked);
        }

        private void OnComponentReplaced(Entity entity, int index, IComponent previousComponent, IComponent newComponent)
        {
           	SetScore(entity.score.whiteScore, entity.score.blackScore);
        }

        private void SetScore(int whiteScore, int blackScore)
        {
            _scoreText.text = string.Format ("White: {0}    Black: {1}", whiteScore, blackScore);
        }

        protected void OnDestroy()
        {
			_restartButton.onClick.RemoveListener (OnRestartButtonClicked);
			_pauseButton.onClick.RemoveListener (OnPauseButtonClicked);
        }


		private void OnRestartButtonClicked()
        {
           GameController.Instance.Restart();
        }
		private void OnPauseButtonClicked()
        {
           GameController.Instance.TogglePause();
        }

	}
}
